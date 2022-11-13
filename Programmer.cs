using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class frmProgrammerCalc : Form
    {
        public frmProgrammerCalc()
        {
            InitializeComponent();
        }

        private void ProgrammerClose(object sender, FormClosedEventArgs e)
        {
            foreach (Form form in Application.OpenForms) //checks through all forms for hidden menu form
            {
                if (form is frmMode)
                {
                    form.Show(); //reshows menu
                    break;
                }
            }
        }

        private void TextUpdate(object sender, EventArgs e)
        {
            bool Error = false;
            char[] Characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(); //characters for bases up to 36
            int BaseI = Decimal.ToInt32(updwnBaseI.Value); //base input
            string Input = txtInput.Text.ToUpper(); //gets input in upper form
            int DenaryConv = 0;
            char Digit;
            int Addition;
            for (int Power = 0; Power < Input.Length; Power++) //converting to denary //each digit index will be a power
            {
                Digit = Input[Input.Length - 1 - Power]; //grabs digits from right to left
                if (Array.IndexOf(Characters, Digit) >= BaseI || Array.IndexOf(Characters, Digit) == -1)
                {
                    Error = true;
                    break;
                }
                Addition = Array.IndexOf(Characters, Digit); //takes multiplier by getting index of digit in characters
                for (int Loop = 0; Loop < Power; Loop++)
                {
                    Addition *= BaseI; //multiplies by base for each power
                }
                DenaryConv += Addition;
            }

            if (Error == true)
            {
                lblOut.Text = "Something's not right here...";
            }
            else
            {
                int BaseO = Decimal.ToInt32(updwnBaseO.Value); //base output
                int Remainder = 1;
                string NewNum = "";
                while (DenaryConv != 0)
                {
                    Remainder = DenaryConv % BaseO;//remainder is Number mod base
                    Digit = Characters[Remainder]; //new digit to be added
                    NewNum = NewNum.Insert(0, Digit.ToString()); //adds digit into number
                    DenaryConv = DenaryConv / BaseO; //integer div for new conv
                }

                lblOut.Text = NewNum; //outputs converted value
            }
        }
    }
}