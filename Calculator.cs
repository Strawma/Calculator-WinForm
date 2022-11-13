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
    public partial class frmBaseCalculator : Form
    {
        public frmBaseCalculator()
        {
            InitializeComponent();
        }

        private void frmCalculator_Load(object sender, EventArgs e)
        {

        }

        //initialises variables that will be used throughout code
        List<string> NumList = new List<string>(); //NumArrayStores Numbers + operators in array
        bool ClearCalc = false; // determines whether calculator needs to be cleard
        bool CalcError = false; //error state of calculator

        string AnsNum = "0"; // saved number for "Ans" Button
        string DisplayText = ""; //text to be displayed (w/o | marker)

        int InsertionOffset = 0; //offset for flashing insertion bar (from string length)
        bool InsertionFlash = true; //bool for flashing insetion marker (true/false oscillation)

        private void DisplayAdd(string NewText) //Displays number - with selection 
        {
            DisplayText = DisplayText.Insert(DisplayText.Length - InsertionOffset, NewText); //inserts new number at selection point
            lblDisplay.Text = DisplayText; //copies text to label 
        }

        private void DisplayReset() //Clears Display Values
        {
            if (ClearCalc == true)
            {
                ClearCalc = false;
                InsertionOffset = 0; //resets insertion, mathstext and label
                DisplayText = "";
                lblDisplay.Text = "";
            }
        }

        private static int InsertionCheck(int ShiftSize, int InsertionOffset, string DisplayText) //finds length of whatever's left/right of insertion marker  
        {
            string Character = "";
            switch (ShiftSize)
            {
                case 1: //checks letter to right if moving right
                    Character = DisplayText.Substring(DisplayText.Length - InsertionOffset - 1, 1);
                    break;
                case -1: //checks letter to left if moving left
                    Character = DisplayText.Substring(DisplayText.Length - InsertionOffset, 1);
                    break;
            }

            switch (Character)
            {
                case "A":
                    ShiftSize *= 3;
                    break;
                case "s":
                    ShiftSize *= 3;
                    break;
                case " ":
                    ShiftSize *= 3;
                    break;
            }
            return ShiftSize;
        }

        private void Operation(string[] SymbolList) //code for doing calculations with basic operators
        {
            if (CalcError == false) //only operates if no error has occured
            {
                try //tries operation
                {
                    for (int IndexA = 0; IndexA < NumList.Count; IndexA++) //Looks through entire array
                    {
                        if (SymbolList.Contains(NumList[IndexA])) //checks to see if operation is being used via array
                        {
                            float OperandA = float.Parse(NumList[IndexA - 1]);
                            string TempB = NumList[IndexA + 1];
                            float OperandB = 0;

                            bool MinusCheck = true; //gets - + numbers outta there
                            int Minus = 1;
                            while (MinusCheck == true)
                            {
                                switch (TempB) 
                                {
                                    case "+":
                                        TempB = NumList[IndexA + 2];
                                        NumList.RemoveAt(IndexA + 2);
                                        break;
                                    case "-":
                                        TempB = NumList[IndexA + 2];
                                        NumList.RemoveAt(IndexA + 2);
                                        Minus /= -1; //minus multiplier alternates
                                        break;
                                    default:
                                        OperandB = float.Parse(TempB) * Minus; //multiplies by - multiplier
                                        MinusCheck = false;
                                        break;
                                }
                            }

                            double NewNum = 0; //initialises NewNum float

                            switch (NumList[IndexA]) //different operation based on symbol
                            {
                                case "^":
                                    NewNum = Math.Pow(OperandA, OperandB);
                                    break;
                                case "÷":
                                    NewNum = OperandA / OperandB;
                                    break;

                                case "x":
                                    NewNum = OperandA * OperandB;
                                    break;

                                case "+":
                                    NewNum = OperandA + OperandB;
                                    break;

                                case "-":
                                    NewNum = OperandA - OperandB;
                                    break;
                            }

                            NumList[IndexA] = NewNum.ToString(); //replaces operator with new answer
                            NumList.RemoveAt(IndexA + 1); //removes operand B
                            NumList.RemoveAt(IndexA - 1); //then operand A if needed
                            IndexA = 0; //resets loop to ensure no values missed
                        }
                    }
                }
                catch //sets CalcError to true if error
                {
                    CalcError = true;
                }
            }
        }

        //----------------------------------------------------------Buttons--------------------------------------------------------------------------
        private void btnNumber_Click(object sender, EventArgs e) //NumberButton called for each number,
        {
            DisplayReset(); //clears calculator with button press after answer

            Button button = sender as Button; //sets button to be the Button Pressed
            switch (button.Text)
            {
                case "AC": //wipes screen if AC pressed
                    ClearCalc = true;
                    DisplayReset();
                    AnsNum = "0";
                    break;
                default:
                    DisplayAdd(button.Text); //adds number to display else
                    break;
            }
        }

        private void btnBasicOperator_Click(object sender, EventArgs e) //OperatorButton called for each operator
        {
            DisplayReset();

            if (DisplayText == "") //Displays Ans as First Number if no initial operand
            {
                DisplayAdd("Ans");
            }

            Button button = sender as Button;
            DisplayAdd(" " + button.Text + " "); //adds operator,separated by spaces on either side, to display, spaces allow for splitting later
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            ClearCalc = true; //tells calc to clear when new button pressed
            string TempText = DisplayText.Replace("Ans", AnsNum); //makes temp text to replace all "Ans" with actual number
            NumList = TempText.Replace("  ", " ").Split(' ').ToList(); //splits display on comma text to make NumList //Replaces any double spaces from repeat operators
            string[] OpArray = {"^"," "}; //stores operators in array to be passed into operation //Powers
            Operation(OpArray);
            OpArray[0] = "x"; OpArray[1] = "÷"; //Multiplication + division
            Operation(OpArray); 
            OpArray[0] = "+"; OpArray[1] = "-";  //addition + subtraction
            Operation(OpArray); 
            DisplayReset(); //resets Display Values

            switch (CalcError) // displays "error" if error has occurred
            {
                case false:
                    AnsNum = string.Join(" ", NumList);
                    DisplayAdd(AnsNum); // puts answer on display if no error
                    break;
                case true:
                    DisplayAdd("Error"); //displays error
                    CalcError = false; //resets error
                    break;
            }

            ClearCalc = true;
        }

        private void btnDirectionClick(object sender, EventArgs e) //used to move insertion marker
        {
            Button button = sender as Button;
            if (button.Text == "<-" & InsertionOffset != DisplayText.Length) //only moves right when not at start of text
            {
                InsertionOffset += InsertionCheck(1, InsertionOffset, DisplayText); //increases offset to move left
            }
            else if (button.Text == "->" & InsertionOffset != 0) //only moves right when not at end of text
            {
                InsertionOffset += InsertionCheck(-1, InsertionOffset, DisplayText); //decreases offset to move right
            }
            else if (button.Text == "Del" & InsertionOffset != DisplayText.Length) //deletes text at insertion marker
            {
                int RemoveLength = InsertionCheck(1, InsertionOffset, DisplayText); //length also depends on whether operator is single or multi-digit
                DisplayText = DisplayText.Remove(DisplayText.Length - InsertionOffset - RemoveLength, RemoveLength);
                lblDisplay.Text = DisplayText;
            }
        }


        private void btnMode_Click(object sender, EventArgs e) //buttons sends user to mode form
        {
            this.Close();
        }

        private void frmCalculator_Close(object sender, FormClosedEventArgs e) //reopens Menu when form closes
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

        //--------------------------------------------------------------------------extra subroutines----------------------------------------------
        private void tmrDisplayFlash_Tick(object sender, EventArgs e) //constant display with flashing insertion marker
        {
            switch (InsertionFlash) // constantly adding and removing | when timer ticks
            {
                case true:
                    lblDisplay.Text = DisplayText; //clears |s by setting to display text
                    InsertionFlash = false;
                    break;

                case false:
                    switch (InsertionOffset)
                    {
                        case int n when n > DisplayText.Length: //sets insertion marker to displaylength if it goes beyond it
                            InsertionOffset = DisplayText.Length;
                            break;

                        case int n when n < 0: //sets insertion marker to 0 if its in -
                            InsertionOffset = 0;
                            break;
                    }
                    lblDisplay.Text = lblDisplay.Text.Insert(DisplayText.Length - InsertionOffset, "‎‏‏‎|‎"); // places | in offset position on label
                    InsertionFlash = true;
                    break;
            }
        }
    }
}