using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Model_LR_01
{

    
    public partial class Form1 : Form
    {
        MyStack<char> stack;
        public List<string> formula { get; set; }
        public List<char> PostfixForm { get; set; }
        public List<char> CurrentForm { get; set; }

        List<int> numbersInFormula;

        ManualResetEventSlim limiter = new ManualResetEventSlim(true);

        int[,] decisionTable =
            {
                { 4, 1, 1, 1, 1, 1, 1, 5, 1, 6 },
                { 2, 2, 2, 1, 1, 1, 1, 2, 1, 6 },
                { 2, 2, 2, 1, 1, 1, 1, 2, 1, 6 },
                { 2, 2, 2, 2, 2, 1, 1, 2, 1, 6 },
                { 2, 2, 2, 2, 2, 1, 1, 2, 1, 6 },
                { 2, 2, 2, 2, 2, 2, 1, 2, 1, 6 },
                { 5, 1, 1, 1, 1, 1, 1, 3, 1, 6 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 2, 2, 2, 2, 2, 2, 1, 2, 5, 6 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

        string buffer;

        Dictionary<char, string> numbers_A2;
        public Form1()
        {
            InitializeComponent();
            PostfixForm = new List<char>();
            CurrentForm = new List<char>();
            stack = new MyStack<char>();
            numbersInFormula = new List<int>();
            buffer = "";
            numbers_A2 = new Dictionary<char, string>();
            formula = new List<string>();
            for (int i = 0; i < 80; i++)
            {
                stack_listBox.Items.Add((char)17);
                cursor_listBox.Items.Add((char)17);
            }
        }
        private void setFormula_button_Click(object sender, EventArgs e)
        {
            if(formula.Count != 0)
            {
                formula.Clear();
                InfixFormula_textBox.Clear();
                CurrentForm.Clear();
            }
            label19.Hide();
            this.Hide();
            Form2 form2 = new Form2(this);
            form2.Show();
        }
        private int getType(char currentSymbol)
        {
            if (currentSymbol == ' ')
            {
                return 0;
            }
            else if (currentSymbol == '+')
            {
                return 1;
            }
            else if (currentSymbol == '-')
            {
                return 2;
            }
            else if (currentSymbol == '*')
            {
                return 3;
            }
            else if (currentSymbol == '/')
            {
                return 4;
            }
            else if (currentSymbol == '^')
            {
                return 5;
            }
            else if (currentSymbol == '(')
            {
                return 6;
            }
            else if (currentSymbol == ')')
            {
                return 7;
            }
            else if ((currentSymbol >= 65) && (currentSymbol <= 90))
            {
                return 8;
            }
            else if (currentSymbol >= 97)
            {
                return 9;
            }
            else
                return 0;
        }
        public void setFormulaTextBox()
        {
            if(!String.IsNullOrWhiteSpace(InfixFormula_textBox.Text))
            {
                InfixFormula_textBox.Text = "";
            }

            foreach (string word in formula)
            {
                InfixFormula_textBox.Text += word;
            }
        }
        private void Start_button_Click(object sender, EventArgs e)
        {
            FunctionsToLetters_A1();
            NumbersToLetters_A2();
            if (CurrentForm.Count == 0)
            {
                label19.Show();
                return;
            }
            if(PostfixForm.Count != 0)
            {
                PostfixForm.Clear();
                PostfixFormula_textBox1.Clear();
                CurrentFormula_textBox.Clear();
            }

            CheckForIllegalCrossThreadCalls = false;
            
            toPostfix();
        }

        async private void toPostfix()
        {
            await Task.Run(() =>
            {
                foreach (char item in CurrentForm)
                {
                    CurrentFormula_textBox.Text += item;
                }
                int counter = 0;
                stack.Push(' ');
                CurrentForm.Add(' ');
                TextBox backColorChanged = textBox3;
                char currentSymbolInStack = ' ';
                int typeCurrentSymbolInStack = 0;
                int typeCurrentSymbolInInput = 0;
                int command = 0;
                TextBox[,] textBoxes =
                {
                    { textBox23, textBox22, textBox21, textBox20, textBox19, textBox18, textBox17, textBox16, textBox15, textBox14 },
                    { textBox34, textBox33, textBox32, textBox31, textBox30, textBox29, textBox28, textBox27, textBox26, textBox25 },
                    { textBox45, textBox44, textBox43, textBox42, textBox41, textBox40, textBox39, textBox38, textBox37, textBox36 },
                    { textBox56, textBox55, textBox54, textBox53, textBox52, textBox51, textBox50, textBox49, textBox48, textBox47 },
                    { textBox67, textBox66, textBox65, textBox64, textBox63, textBox62, textBox61, textBox60, textBox59, textBox58 },
                    { textBox78, textBox77, textBox76, textBox75, textBox74, textBox73, textBox72, textBox71, textBox70, textBox69 },
                    { textBox89, textBox88, textBox87, textBox86, textBox85, textBox84, textBox83, textBox82, textBox81, textBox80 },
                    { textBox100, textBox99, textBox98, textBox97, textBox96, textBox95, textBox94, textBox93, textBox92, textBox91 },
                    { textBox100, textBox99, textBox98, textBox97, textBox96, textBox95, textBox94, textBox93, textBox92, textBox91 },
                    { textBox100, textBox99, textBox98, textBox97, textBox96, textBox95, textBox94, textBox93, textBox92, textBox91 },
                };
                int stackPosition = 0;
                int stackPrePos = 0;
                bool stackChanged = false;
                while (CurrentForm.Count > counter)
                {
                    // попробовать сделать так: добавить переменную, которая отслеживает позицию последней добавленной переменной в визуальный стек

                    limiter.Wait();

                    stackPosition = stack_listBox.Items.Count - stack.Count;

                    textBoxes[typeCurrentSymbolInStack, typeCurrentSymbolInInput].BackColor = Color.White;

                    currentSymbolInStack = stack.Peek();
                    typeCurrentSymbolInStack = getType(currentSymbolInStack);
                    typeCurrentSymbolInInput = getType(CurrentForm[counter]);
                    command = decisionTable[typeCurrentSymbolInStack, typeCurrentSymbolInInput];
                    if (command == 1)
                    {
                        stack.Push(CurrentForm[counter]);
                        textBoxes[typeCurrentSymbolInStack, typeCurrentSymbolInInput].BackColor = Color.Aqua;

                        stack_listBox.Items[stackPosition] = stack.Peek();
                        cursor_listBox.Items[stackPrePos] = (char)17;
                        cursor_listBox.Items[stackPosition] = "--->";

                        stackChanged = true;
                    }
                    else if (command == 2)
                    {
                        PostfixForm.Add(stack.Pop());
                        counter--;
                        textBoxes[typeCurrentSymbolInStack, typeCurrentSymbolInInput].BackColor = Color.Aqua;

                        /*stack_listBox.Items[stackPrePos] = (char)17;
                        int curr_item = cursor_listBox.Items.IndexOf("--->");
                        cursor_listBox.Items[curr_item] = (char)17;
                        cursor_listBox.Items[stackPosition] = "--->";*/

                        stack_listBox.Items[stackPosition] = (char)17;
                        cursor_listBox.Items[stackPrePos] = (char)17;
                        cursor_listBox.Items[stackPosition] = "--->";

                        stackChanged = true;
                    }
                    else if (command == 3)
                    {
                        stack.Pop();
                        textBoxes[typeCurrentSymbolInStack, typeCurrentSymbolInInput].BackColor = Color.Aqua;

                        stack_listBox.Items[stackPosition] = (char)17;
                        cursor_listBox.Items[stackPrePos] = (char)17;
                        cursor_listBox.Items[stackPosition] = "--->";

                        stackChanged = true;
                    }
                    else if (command == 4)
                    {
                        foreach (var item in PostfixForm)
                        {
                            PostfixFormula_textBox1.Text += item;
                        }
                        textBoxes[typeCurrentSymbolInStack, typeCurrentSymbolInInput].BackColor = Color.LightGreen;

                        stackChanged = false;
                    }
                    else if (command == 5)
                    {
                        textBoxes[typeCurrentSymbolInStack, typeCurrentSymbolInInput].BackColor = Color.Red;
                        throw new InvalidOperationException("Ошибка обработки стека");
                    }
                    else if (command == 6)
                    {
                        PostfixForm.Add(CurrentForm[counter]);
                        textBoxes[typeCurrentSymbolInStack, typeCurrentSymbolInInput].BackColor = Color.Aqua;

                        stackChanged = false;
                    }
                    counter++;

                    if(stackChanged)
                    {
                        stackPrePos = stackPosition;
                    }

                    if (Stepmode_radioButton.Checked)
                        limiter.Reset();
                    else
                    {
                        Thread.Sleep(Step_trackBar.Value * 50);
                    }
                }
            });
        }
        // методы перевода из string в char

        // перевод функций в алфавит А1
        private void FunctionsToLetters_A1()  
        {
            int counter = 0;
            
            while(formula.Count > counter)
            {
                if(formula[counter].Length > 1)
                {
                    if(formula[counter] == "^")
                    {
                        CurrentForm.Add('Z');
                    }
                    if (formula[counter] == "sin")
                    {
                        CurrentForm.Add('A');
                    }
                    if(formula[counter] == "cos")
                    {
                        CurrentForm.Add('B');
                    }
                    if(formula[counter] == "tg")
                    {
                        CurrentForm.Add('C');
                    }
                    if(formula[counter] == "ctg")
                    {
                        CurrentForm.Add('D');
                    }
                    if(formula[counter] == "ln")
                    {
                        CurrentForm.Add('E');
                    }
                    if(formula[counter] == "lg")
                    {
                        CurrentForm.Add('F');
                    }
                    if(formula[counter] == "exp")
                    {
                        CurrentForm.Add('G');
                    }
                    if(formula[counter] == "sqr")
                    {
                        CurrentForm.Add('H');
                    }
                    if(formula[counter] == "sqrt")
                    {
                        CurrentForm.Add('J');
                    }
                    if(formula[counter] == "arcsin")
                    {
                        CurrentForm.Add('K');
                    }
                    if(formula[counter] == "arccos")
                    {
                        CurrentForm.Add('L');
                    }
                    if(formula[counter] == "arctg")
                    {
                        CurrentForm.Add('L');
                    }
                    if(formula[counter] == "arcctg")
                    {
                        CurrentForm.Add('M');
                    }
                }
                else
                {
                    CurrentForm.Add(formula[counter].First());
                }
                counter++;
            }
            CurrentForm.Add(' ');
        }
        // перевод чисел в алфавит А2
        private void NumbersToLetters_A2() 
        {
            int counter = 0;
            int currentCharNum = 97;
            List<char> buffer_formula = new List<char>();
            while (CurrentForm.Count - 1 > counter)
            {
                if (((CurrentForm[counter + 1] >= 40) && (CurrentForm[counter + 1] <= 47) && (CurrentForm[counter + 1] != 40) && (CurrentForm[counter + 1] != 46) && (CurrentForm[counter] != 41))
                    || (CurrentForm[counter + 1] == 94)
                    || (CurrentForm[counter + 1] == 32)
                      
                    )
                {
                    buffer += CurrentForm[counter];
                    numbers_A2.Add((char)currentCharNum, buffer);

                    buffer_formula.Add((char)currentCharNum);

                    buffer = "";
                    currentCharNum++;
                    counter++;
                }
                
                else
                {
                    if((CurrentForm[counter] >= 48) && (CurrentForm[counter] <= 57) || (CurrentForm[counter] == 46))
                        buffer += CurrentForm[counter];
                    else
                        buffer_formula.Add(CurrentForm[counter]);
                    counter++;
                }
            }
            CurrentForm.Clear();
            CurrentForm = buffer_formula;
        }

        private void Stepmode_radioButton_CheckedChanged(object sender, EventArgs e)
        {

            if(Stepmode_radioButton.Checked)
            {
                nextTact_button.Show();
            }
            else
            {
                nextTact_button.Hide();
            }
        }

        private void Automode_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if(Automode_radioButton.Checked)
            {
                nextTact_button.Hide();
            }
            else
            {
                nextTact_button.Show();
            }
        }

        private void nextTact_button_Click(object sender, EventArgs e)
        {
            limiter.Set();
        }

        private void Step_trackBar_ValueChanged(object sender, EventArgs e)
        {
            label18.Text = (Step_trackBar.Value * 50).ToString() + " мс";
        }
    }
}

/*// последний символ в стэке - пробел
                if (stack.Peek() == 32) 
                {
                    if(CurrentForm[counter] == 32)
                    {
                        counter = CurrentForm.Count;
                        textBox23.BackColor = Color.Aqua;
                        backColorChanged = textBox23;
                        break;
                    }
                    if(CurrentForm[counter] == 41)
                    {
                        counter = CurrentForm.Count;
                        error_label.Text = "Ошибка скобочной структуры";
                        textBox16.BackColor = Color.Red;
                        backColorChanged = textBox16;
                        break;
                    }
                    else if(CurrentForm[counter] >= 97)
                    {
                        PostfixForm.Add(CurrentForm[counter]);
                        textBox14.BackColor = Color.Aqua;
                        backColorChanged = textBox14;
                    }
                    else //если последний вошешдший символ - знак операции, открывающая скобка, функция
                    {
                        stack.Push(CurrentForm[counter]);
                        if (CurrentForm[counter] == 40)
                        {
                            textBox17.BackColor = Color.Aqua;
                            backColorChanged = textBox17;
                        }
                        else if (CurrentForm[counter] == 42)
                        {
                            textBox20.BackColor = Color.Aqua;
                            backColorChanged = textBox20;
                        }
                        else if (CurrentForm[counter] == 43)
                        {
                            textBox23.BackColor = Color.Aqua;
                            backColorChanged = textBox23;
                        }
                        else if (CurrentForm[counter] == 45)
                        {
                            textBox21.BackColor = Color.Aqua;
                            backColorChanged = textBox21;
                        }
                        else if (CurrentForm[counter] == 47)
                        {
                            textBox19.BackColor = Color.Aqua;
                            backColorChanged = textBox19;
                        }
                        else if (CurrentForm[counter] == 94)
                        {
                            textBox18.BackColor = Color.Aqua;
                            backColorChanged = textBox18;
                        }
                        else if ((CurrentForm[counter] >= 65) && (CurrentForm[counter] <= 90))
                        {
                            textBox15.BackColor = Color.Aqua;
                            backColorChanged = textBox15;
                        }

                    }
                }
                // последний символ в стэке - "+"
                else if(stack.Peek() == 43)
                {
                    if (CurrentForm[counter] >= 97)
                    {
                        PostfixForm.Add(CurrentForm[counter]);
                        textBox25.BackColor = Color.Aqua;
                        backColorChanged = textBox25;
                    }

                    else if(CurrentForm[counter] == 32)
                    {
                        PostfixForm.Add(stack.Pop());
                        textBox34.BackColor = Color.Aqua;
                        backColorChanged = textBox34;
                    }
                    else if(CurrentForm[counter] == 43)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                        textBox33.BackColor = Color.Aqua;
                        backColorChanged = textBox33;
                    }
                    else if(CurrentForm[counter] == 45)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox32.BackColor = Color.Aqua;
                        backColorChanged = textBox32;
                    }
                    else if(CurrentForm[counter] == 41)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox27.BackColor = Color.Aqua;
                        backColorChanged = textBox27;
                    }

                    else if(CurrentForm[counter] == 42)
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox31.BackColor = Color.Aqua;
                        backColorChanged = textBox31;
                    }
                    else if (CurrentForm[counter] == 47)
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox30.BackColor = Color.Aqua;
                        backColorChanged = textBox30;
                    }
                    else if (CurrentForm[counter] == 94)
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox29.BackColor = Color.Aqua;
                        backColorChanged = textBox29;
                    }
                    else if (CurrentForm[counter] == 40)
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox28.BackColor = Color.Aqua;
                        backColorChanged = textBox28;
                    }
                    else if ((CurrentForm[counter] >= 65) && (CurrentForm[counter] <= 90))
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox26.BackColor = Color.Aqua;
                        backColorChanged = textBox26;
                    }

                }
                // последний символ в стэке - "-"
                else if(stack.Peek() == 45)
                {
                    if (CurrentForm[counter] >= 97)
                    {
                        PostfixForm.Add(CurrentForm[counter]);
                        textBox36.BackColor = Color.Aqua;
                        backColorChanged = textBox36;
                    }

                    else if (CurrentForm[counter] == 32)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox45.BackColor = Color.Aqua;
                        backColorChanged = textBox45;
                    }
                    else if (CurrentForm[counter] == 43)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox44.BackColor = Color.Aqua;
                        backColorChanged = textBox44;
                    }
                    else if (CurrentForm[counter] == 45)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox43.BackColor = Color.Aqua;
                        backColorChanged = textBox43;
                    }
                    else if (CurrentForm[counter] == 41)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox38.BackColor = Color.Aqua;
                        backColorChanged = textBox38;
                    }

                    else if (CurrentForm[counter] == 42)
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox42.BackColor = Color.Aqua;
                        backColorChanged = textBox42;
                    }
                    else if (CurrentForm[counter] == 47)
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox41.BackColor = Color.Aqua;
                        backColorChanged = textBox41;
                    }
                    else if (CurrentForm[counter] == 94)
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox40.BackColor = Color.Aqua;
                        backColorChanged = textBox40;
                    }
                    else if (CurrentForm[counter] == 40)
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox39.BackColor = Color.Aqua;
                        backColorChanged = textBox39;
                    }
                    else if ((CurrentForm[counter] >= 65) && (CurrentForm[counter] <= 90))
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox37.BackColor = Color.Aqua;
                        backColorChanged = textBox37;
                    }
                }
                // последний символ в стэке - "*"
                else if (stack.Peek() == 42)
                {
                    if (CurrentForm[counter] >= 97)
                    {
                        PostfixForm.Add(CurrentForm[counter]);
                        textBox47.BackColor = Color.Aqua;
                        backColorChanged = textBox47;
                    }

                    else if (CurrentForm[counter] == 32)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox56.BackColor = Color.Aqua;
                        backColorChanged = textBox56;
                    }
                    else if (CurrentForm[counter] == 43)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox55.BackColor = Color.Aqua;
                        backColorChanged = textBox55;
                    }
                    else if (CurrentForm[counter] == 45)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox54.BackColor = Color.Aqua;
                        backColorChanged = textBox54;
                    }
                    else if (CurrentForm[counter] == 41)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox49.BackColor = Color.Aqua;
                        backColorChanged = textBox49;
                    }

                    else if (CurrentForm[counter] == 42)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox53.BackColor = Color.Aqua;
                        backColorChanged = textBox53;
                    }
                    else if (CurrentForm[counter] == 47)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox52.BackColor = Color.Aqua;
                        backColorChanged = textBox52;
                    }
                    else if (CurrentForm[counter] == 94)
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox51.BackColor = Color.Aqua;
                        backColorChanged = textBox51;
                    }
                    else if (CurrentForm[counter] == 40)
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox50.BackColor = Color.Aqua;
                        backColorChanged = textBox50;
                    }
                    else if ((CurrentForm[counter] >= 65) && (CurrentForm[counter] <= 90))
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox48.BackColor = Color.Aqua;
                        backColorChanged = textBox48;
                    }
                }
                //последний символ в стэке - "/"
                else if (stack.Peek() == 47)
                {
                    if (CurrentForm[counter] >= 97)
                    {
                        PostfixForm.Add(CurrentForm[counter]);
                        textBox58.BackColor = Color.Aqua;
                        backColorChanged = textBox58;
                    }

                    else if (CurrentForm[counter] == 32)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox67.BackColor = Color.Aqua;
                        backColorChanged = textBox67;
                    }
                    else if (CurrentForm[counter] == 43)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox66.BackColor = Color.Aqua;
                        backColorChanged = textBox66;
                    }
                    else if (CurrentForm[counter] == 45)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox65.BackColor = Color.Aqua;
                        backColorChanged = textBox65;
                    }
                    else if (CurrentForm[counter] == 41)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox60.BackColor = Color.Aqua;
                        backColorChanged = textBox60;
                    }

                    else if (CurrentForm[counter] == 42)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox64.BackColor = Color.Aqua;
                        backColorChanged = textBox64;
                    }
                    else if (CurrentForm[counter] == 47)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox63.BackColor = Color.Aqua;
                        backColorChanged = textBox63;
                    }
                    else if (CurrentForm[counter] == 94)
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox62.BackColor = Color.Aqua;
                        backColorChanged = textBox62;
                    }
                    else if (CurrentForm[counter] == 40)
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox61.BackColor = Color.Aqua;
                        backColorChanged = textBox61;
                    }
                    else if ((CurrentForm[counter] >= 65) && (CurrentForm[counter] <= 90))
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox59.BackColor = Color.Aqua;
                        backColorChanged = textBox59;
                    }
                }
                // последний символ в стэке - "^"
                else if (stack.Peek() == 94)
                {
                    if (CurrentForm[counter] >= 97)
                    {
                        PostfixForm.Add(CurrentForm[counter]);
                        textBox69.BackColor = Color.Aqua;
                        backColorChanged = textBox69;
                    }

                    else if (CurrentForm[counter] == 32)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox78.BackColor = Color.Aqua;
                        backColorChanged = textBox78;
                    }
                    else if (CurrentForm[counter] == 43)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox77.BackColor = Color.Aqua;
                        backColorChanged = textBox77;
                    }
                    else if (CurrentForm[counter] == 45)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox76.BackColor = Color.Aqua;
                        backColorChanged = textBox76;
                    }
                    else if (CurrentForm[counter] == 41)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox71.BackColor = Color.Aqua;
                        backColorChanged = textBox71;
                    }

                    else if (CurrentForm[counter] == 42)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox75.BackColor = Color.Aqua;
                        backColorChanged = textBox75;
                    }
                    else if (CurrentForm[counter] == 47)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox74.BackColor = Color.Aqua;
                        backColorChanged = textBox74;
                    }
                    else if (CurrentForm[counter] == 94)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox73.BackColor = Color.Aqua;
                        backColorChanged = textBox73;
                    }
                    else if (CurrentForm[counter] == 40)
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox72.BackColor = Color.Aqua;
                        backColorChanged = textBox72;
                    }
                    else if ((CurrentForm[counter] >= 65) && (CurrentForm[counter] <= 90))
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox70.BackColor = Color.Aqua;
                        backColorChanged = textBox70;
                    }
                }
                // последний символ в стэке - "("
                else if (stack.Peek() == 40)
                {
                    if(CurrentForm[counter] == 32)
                    {
                        counter = CurrentForm.Count;
                        textBox89.BackColor = Color.Red;
                        backColorChanged = textBox89;
                        break;
                    }
                    else if(CurrentForm[counter] == 41)
                    {
                        CurrentForm.RemoveAt(counter);
                        //stack.Pop();
                        textBox82.BackColor = Color.Aqua;
                        backColorChanged = textBox82;
                    }
                    else if(CurrentForm[counter] >= 97)
                    {
                        PostfixForm.Add(CurrentForm[counter]);
                        textBox80.BackColor = Color.Aqua;
                        backColorChanged = textBox80;
                    }
                    else //если последний вошешдший символ - знак операции, открывающая скобка, функция
                    {
                        stack.Push(CurrentForm[counter]);
                        if (CurrentForm[counter] == 40)
                        {
                            textBox83.BackColor = Color.Aqua;
                            backColorChanged = textBox83;
                        }
                        else if (CurrentForm[counter] == 42)
                        {
                            textBox86.BackColor = Color.Aqua;
                            backColorChanged = textBox86;
                        }
                        else if (CurrentForm[counter] == 43)
                        {
                            textBox88.BackColor = Color.Aqua;
                            backColorChanged = textBox88;
                        }
                        else if (CurrentForm[counter] == 45)
                        {
                            textBox87.BackColor = Color.Aqua;
                            backColorChanged = textBox89;
                        }
                        else if (CurrentForm[counter] == 47)
                        {
                            textBox85.BackColor = Color.Aqua;
                            backColorChanged = textBox85;
                        }
                        else if (CurrentForm[counter] == 94)
                        {
                            textBox84.BackColor = Color.Aqua;
                            backColorChanged = textBox84;
                        }
                        else if ((CurrentForm[counter] >= 65) && (CurrentForm[counter] <= 90))
                        {
                            textBox82.BackColor = Color.Aqua;
                            backColorChanged = textBox82;
                        }

                    }
                }
                // последний символ в стэке - функция
                else if ((stack.Peek() >= 65) && (stack.Peek() <= 90))
                {
                    if (CurrentForm[counter] >= 97)
                    {
                        PostfixForm.Add(CurrentForm[counter]);
                        textBox91.BackColor = Color.Aqua;
                        backColorChanged = textBox91;
                    }

                    else if (CurrentForm[counter] == 32)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox90.BackColor = Color.Aqua;
                        backColorChanged = textBox90;
                    }
                    else if (CurrentForm[counter] == 43)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox99.BackColor = Color.Aqua;
                        backColorChanged = textBox99;
                    }
                    else if (CurrentForm[counter] == 45)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox98.BackColor = Color.Aqua;
                        backColorChanged = textBox98;
                    }
                    else if (CurrentForm[counter] == 41)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox93.BackColor = Color.Aqua;
                        backColorChanged = textBox93;
                    }

                    else if (CurrentForm[counter] == 42)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox97.BackColor = Color.Aqua;
                        backColorChanged = textBox97;
                    }
                    else if (CurrentForm[counter] == 47)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox96.BackColor = Color.Aqua;
                        backColorChanged = textBox96;
                    }
                    else if (CurrentForm[counter] == 94)
                    {
                        PostfixForm.Add(stack.Pop());
                       stack.Push(CurrentForm[counter]);
                       textBox95.BackColor = Color.Aqua;
                        backColorChanged = textBox95;
                    }
                    else if (CurrentForm[counter] == 40)
                    {
                        stack.Push(CurrentForm[counter]);
                        textBox94.BackColor = Color.Aqua;
                        backColorChanged = textBox94;
                    }
                    else if ((CurrentForm[counter] >= 65) && (CurrentForm[counter] <= 90))
                    {
                        stack.Push(CurrentForm[counter]);
                        counter = CurrentForm.Count;
                        textBox92.BackColor = Color.Red;
                        backColorChanged = textBox92;
                    }
                }
                counter++;*/