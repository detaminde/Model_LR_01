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

        string buffer;

        Dictionary<char, string> numbers_A2;
        /* Dictionary<char, string> functions_A1;*/

        public Form1()
        {
            InitializeComponent();
            PostfixForm = new List<char>();
            CurrentForm = new List<char>();
            stack = new MyStack<char>();
            numbersInFormula = new List<int>();
            buffer = "";
            /*functions_A1 = new Dictionary<char, string>()
            {
                { 'A', "sin" },
                { 'B', "cos" },
                { 'C', "tg" },
                { 'D', "ctg" },
                { 'E', "ln" },
                { 'F', "lg" },
                { 'G', "^" },
                { 'H', "exp" },
                { 'I', "sqr" },
                { 'J', "sqrt" },
                { 'K', "arcsin" },
                { 'L', "arccos" },
                { 'M', "arctg" },
                { 'N', "arcctg" }
            };*/
            numbers_A2 = new Dictionary<char, string>();
        }

        private void setFormula_button_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2(this);
            form2.Show();
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
            foreach (char item in CurrentForm)
            {
                CurrentFormula_textBox.Text += item;
            }
            
            //здесь должен быть цикл, описанный в лекционных материалах
             int counter = 0;
             stack.Push(' ');
             CurrentForm.Add(' ');
             TextBox backColorChanged = textBox3;
            char currentSymbolInStack = ' ';
             while(CurrentForm.Count > counter)
             {
                // пересмотреть проход цикла: нужно ли, чтобы именно после каждой итерации счётчик прибавлялся
                // добавить пользовательское отображение
                // удалить из каждой функции, где PostfixForm.Add(stack.Pop)    -   stack.Push(CurrentForm[counter]); и добавить вместо него остановку счётчика

                currentSymbolInStack = stack.Peek();
                 // последний символ в стэке - пробел
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
                        counter++;
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
                         textBox33.BackColor = Color.Aqua;
                         backColorChanged = textBox33;
                     }
                     else if(CurrentForm[counter] == 45)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox32.BackColor = Color.Aqua;
                         backColorChanged = textBox32;
                     }
                     else if(CurrentForm[counter] == 41)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox27.BackColor = Color.Aqua;
                         backColorChanged = textBox27;
                     }

                     else if(CurrentForm[counter] == 42)
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox31.BackColor = Color.Aqua;
                         backColorChanged = textBox31;
                         counter++;
                     }
                     else if (CurrentForm[counter] == 47)
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox30.BackColor = Color.Aqua;
                         backColorChanged = textBox30;
                         counter++;
                     }
                     else if (CurrentForm[counter] == 94)
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox29.BackColor = Color.Aqua;
                         backColorChanged = textBox29;
                         counter++;
                     }
                     else if (CurrentForm[counter] == 40)
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox28.BackColor = Color.Aqua;
                         backColorChanged = textBox28;
                         counter++;
                     }
                     else if ((CurrentForm[counter] >= 65) && (CurrentForm[counter] <= 90))
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox26.BackColor = Color.Aqua;
                         backColorChanged = textBox26;
                         counter++;
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
                        textBox45.BackColor = Color.Aqua;
                         backColorChanged = textBox45;
                     }
                     else if (CurrentForm[counter] == 43)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox44.BackColor = Color.Aqua;
                         backColorChanged = textBox44;
                     }
                     else if (CurrentForm[counter] == 45)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox43.BackColor = Color.Aqua;
                         backColorChanged = textBox43;
                     }
                     else if (CurrentForm[counter] == 41)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox38.BackColor = Color.Aqua;
                         backColorChanged = textBox38;
                     }

                     else if (CurrentForm[counter] == 42)
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox42.BackColor = Color.Aqua;
                         backColorChanged = textBox42;
                         counter++;
                     }
                     else if (CurrentForm[counter] == 47)
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox41.BackColor = Color.Aqua;
                         backColorChanged = textBox41;
                         counter++;
                     }
                     else if (CurrentForm[counter] == 94)
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox40.BackColor = Color.Aqua;
                         backColorChanged = textBox40;
                         counter++;
                     }
                     else if (CurrentForm[counter] == 40)
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox39.BackColor = Color.Aqua;
                         backColorChanged = textBox39;
                         counter++;
                     }
                     else if ((CurrentForm[counter] >= 65) && (CurrentForm[counter] <= 90))
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox37.BackColor = Color.Aqua;
                         backColorChanged = textBox37;
                         counter++;
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
                        textBox56.BackColor = Color.Aqua;
                         backColorChanged = textBox56;
                     }
                     else if (CurrentForm[counter] == 43)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox55.BackColor = Color.Aqua;
                         backColorChanged = textBox55;
                     }
                     else if (CurrentForm[counter] == 45)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox54.BackColor = Color.Aqua;
                         backColorChanged = textBox54;
                     }
                     else if (CurrentForm[counter] == 41)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox49.BackColor = Color.Aqua;
                         backColorChanged = textBox49;
                     }

                     else if (CurrentForm[counter] == 42)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox53.BackColor = Color.Aqua;
                         backColorChanged = textBox53;
                     }
                     else if (CurrentForm[counter] == 47)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox52.BackColor = Color.Aqua;
                         backColorChanged = textBox52;
                     }
                     else if (CurrentForm[counter] == 94)
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox51.BackColor = Color.Aqua;
                         backColorChanged = textBox51;
                         counter++;
                     }
                     else if (CurrentForm[counter] == 40)
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox50.BackColor = Color.Aqua;
                         backColorChanged = textBox50;
                         counter++;
                     }
                     else if ((CurrentForm[counter] >= 65) && (CurrentForm[counter] <= 90))
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox48.BackColor = Color.Aqua;
                         backColorChanged = textBox48;
                         counter++;
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
                        textBox67.BackColor = Color.Aqua;
                         backColorChanged = textBox67;
                     }
                     else if (CurrentForm[counter] == 43)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox66.BackColor = Color.Aqua;
                         backColorChanged = textBox66;
                     }
                     else if (CurrentForm[counter] == 45)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox65.BackColor = Color.Aqua;
                         backColorChanged = textBox65;
                     }
                     else if (CurrentForm[counter] == 41)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox60.BackColor = Color.Aqua;
                         backColorChanged = textBox60;
                     }

                     else if (CurrentForm[counter] == 42)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox64.BackColor = Color.Aqua;
                         backColorChanged = textBox64;
                     }
                     else if (CurrentForm[counter] == 47)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox63.BackColor = Color.Aqua;
                         backColorChanged = textBox63;
                     }
                     else if (CurrentForm[counter] == 94)
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox62.BackColor = Color.Aqua;
                         backColorChanged = textBox62;
                         counter++;
                     }
                     else if (CurrentForm[counter] == 40)
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox61.BackColor = Color.Aqua;
                         backColorChanged = textBox61;
                         counter++;
                     }
                     else if ((CurrentForm[counter] >= 65) && (CurrentForm[counter] <= 90))
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox59.BackColor = Color.Aqua;
                         backColorChanged = textBox59;
                         counter++;
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
                        textBox78.BackColor = Color.Aqua;
                         backColorChanged = textBox78;
                     }
                     else if (CurrentForm[counter] == 43)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox77.BackColor = Color.Aqua;
                         backColorChanged = textBox77;
                     }
                     else if (CurrentForm[counter] == 45)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox76.BackColor = Color.Aqua;
                         backColorChanged = textBox76;
                     }
                     else if (CurrentForm[counter] == 41)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox71.BackColor = Color.Aqua;
                         backColorChanged = textBox71;
                     }

                     else if (CurrentForm[counter] == 42)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox75.BackColor = Color.Aqua;
                         backColorChanged = textBox75;
                     }
                     else if (CurrentForm[counter] == 47)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox74.BackColor = Color.Aqua;
                         backColorChanged = textBox74;
                     }
                     else if (CurrentForm[counter] == 94)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox73.BackColor = Color.Aqua;
                         backColorChanged = textBox73;
                     }
                     else if (CurrentForm[counter] == 40)
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox72.BackColor = Color.Aqua;
                         backColorChanged = textBox72;
                         counter++;
                     }
                     else if ((CurrentForm[counter] >= 65) && (CurrentForm[counter] <= 90))
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox70.BackColor = Color.Aqua;
                         backColorChanged = textBox70;
                         counter++;
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
                        counter++;
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
                        textBox90.BackColor = Color.Aqua;
                         backColorChanged = textBox90;
                     }
                     else if (CurrentForm[counter] == 43)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox99.BackColor = Color.Aqua;
                         backColorChanged = textBox99;
                     }
                     else if (CurrentForm[counter] == 45)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox98.BackColor = Color.Aqua;
                         backColorChanged = textBox98;
                     }
                     else if (CurrentForm[counter] == 41)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox93.BackColor = Color.Aqua;
                         backColorChanged = textBox93;
                     }

                     else if (CurrentForm[counter] == 42)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox97.BackColor = Color.Aqua;
                         backColorChanged = textBox97;
                     }
                     else if (CurrentForm[counter] == 47)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox96.BackColor = Color.Aqua;
                         backColorChanged = textBox96;
                     }
                     else if (CurrentForm[counter] == 94)
                     {
                         PostfixForm.Add(stack.Pop());
                        textBox95.BackColor = Color.Aqua;
                         backColorChanged = textBox95;
                     }
                     else if (CurrentForm[counter] == 40)
                     {
                         stack.Push(CurrentForm[counter]);
                         textBox94.BackColor = Color.Aqua;
                         backColorChanged = textBox94;
                         counter++;
                     }
                     else if ((CurrentForm[counter] >= 65) && (CurrentForm[counter] <= 90))
                     {
                         stack.Push(CurrentForm[counter]);
                         counter = CurrentForm.Count;
                         textBox92.BackColor = Color.Red;
                         backColorChanged = textBox92;
                     }
                 }


                 if (Automode_radioButton.Checked)
                 {
                     Thread.Sleep(50);
                 }
                 else if(Stepmode_radioButton.Checked)
                 {
                     Thread.Sleep(Step_trackBar.Value * 5);
                 }
                 backColorChanged.BackColor = Color.White;
             }

             while (stack.Peek() != ' ')
             {
                 PostfixForm.Add(stack.Pop());
             }
             foreach (var item in PostfixForm)
             {
                 PostfixFormula_textBox1.Text += item;
             }





        }
        //функция перевода из string в char для функций
        private void FunctionsToLetters_A1() // проверить 
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
        private void NumbersToLetters_A2() // Перевод чисел в зашифрованные буквы алфавита А2
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
    }
}
