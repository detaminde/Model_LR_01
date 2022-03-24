using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Model_LR_01
{
    public partial class Form2 : Form
    {
        
        List<string> formula_string;
        Form1 curr_form1;
        public Form2(Form1 form1)
        {
            InitializeComponent();   
            formula_string = new List<string>();
            curr_form1 = form1;
        }

        public void setFormula(List<string> currFormula)
        {
            //проверить формулу на какие-то несостыковки
            curr_form1.formula = formula_string;
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            curr_form1.Show();
            this.Close();
        }

        private void approve_button_Click(object sender, EventArgs e)
        {
            setFormula(formula_string);
            curr_form1.setFormulaTextBox();
            curr_form1.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            formula_string.Add(((Button)sender).Text);
            Formula_textBox.Text += ((Button)sender).Text;
        }
    }
}
