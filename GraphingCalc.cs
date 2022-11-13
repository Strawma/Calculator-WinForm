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
    public partial class frmGraphingCalc : Form
    {
        public frmGraphingCalc()
        {
            InitializeComponent();
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

        private void UpdateGraph(int GraphMode, int Scale)
        {
            int Multiplier = this.Width / 225;
            const int Divisor = 2;

            System.Drawing.Graphics graphics = pnlGraph.CreateGraphics(); //grid will be +-220 += 110
            pnlGraph.Refresh(); //clears graphics
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; //adds anti aliasing to line
            Pen DrawPen = new Pen(Color.Red, 2 * Multiplier / Divisor); //sets pen size + colour, uses multiplier to scale pen
            try
            {
                string[] Inputs = new string[] { txtFirstTerm.Text.Replace(" ", ""), txtSecondTerm.Text.Replace(" ", ""), txtThirdTerm.Text.Replace(" ", "") }; //removes spaces from input
                for (int i = 0; i < Inputs.Length; i++)
                {
                    if (Inputs[i] == "")
                    {
                        Inputs[i] = "0";
                    }
                }
                float a = float.Parse(Inputs[0]); //sets gradient + constant of line
                float b = float.Parse(Inputs[1]);
                float c = float.Parse(Inputs[2]);
                switch (GraphMode)
                {

                    case 1: //linear MODE
                        lblSolution.Text = "X intercept: (" + Math.Round(-b / a, 2) + ", 0)"; //displays x intercept
                        switch (b / a) //DO IT NOW
                        {
                            case float.NaN:
                                lblSolution.Text = "Line is on X-axis"; //if line has no x coefficient
                                break;
                            case float.PositiveInfinity:
                                lblSolution.Text = "Line above X-axis";
                                break;
                            case float.NegativeInfinity:
                                lblSolution.Text = "Line below X-axis";
                                break;
                        }

                        float[] MinP = new float[] { -Scale, -Scale * a + b }; //coordinates for x min (-Scale)
                        MinP = Conv(MinP, Multiplier, Divisor, Scale);

                        float[] MaxP = new float[] { Scale, Scale * a + b };//x max coordinates (200)
                        MaxP = Conv(MaxP, Multiplier, Divisor, Scale);

                        graphics.DrawLine(DrawPen, MinP[0], MinP[1], MaxP[0], MaxP[1]); //drawLine
                        break;

                    case 2: //QUADRATIC MODE
                        if (a == 0)
                        {
                            lblSolution2.Visible = false;
                            lblSolution.Text = "that's... not a quadratic graph";
                        }
                        else
                        {
                            lblSolution.Text = "Turning point: (" + Math.Round(-(b / a / 2), 2) + ", " + Math.Round(c - b * b / a / 4, 2) + ")"; //turning point
                            //b * b / 4 / a / a - c / a
                            if (Math.Round(b * b - 4 * a * c, 2) > 0) //checks if roots real using discriminant
                            {
                                lblSolution2.Text = "Roots: (" + Math.Round(-b / a / 2 + Math.Sqrt(b * b / 4 / a / a - c / a), 2) + ", 0) and (" + Math.Round(-b / a / 2 - Math.Sqrt(b * b / 4 / a / a - c / a), 2) + ", 0)"; //finding roots... i know it looks bad okay
                            }
                            else if (Math.Round(b * b - 4 * a * c, 2) == 0) //repeated root
                            {
                                lblSolution2.Text = "Repeated Root: (" + Math.Round(-b / a / 2, 2) + ", 0)";
                            }
                            else //Imaginary solutions pretty dope
                            {
                                lblSolution2.Text = "Roots: (" + Math.Round(-b / a / 2, 2) + " + " + Math.Round(Math.Sqrt(-b * b / 4 / a / a + c / a), 2) + "i, 0) and (" + Math.Round(-b / a / 2, 2) + " - " + Math.Round(Math.Sqrt(-b * b / 4 / a / a + c / a), 2) + "i, 0)"; //yeah... just yeah... it works though!
                            }
                            lblSolution2.Visible = true;
                        }

                        PointF[] Points = new PointF[45]; //array for final use, 43 points
                        int TempX;
                        float[] TempC = new float[2];
                        PointF TempP; //Temporary values since i have no idea what I'm doing
                        for (int i = 0; i < 45; i++) //adds 43 points across graph 
                        {
                            TempX = Scale - i * (Scale / 22); //graph step = scale/22 (22 points each side + origin)
                            TempC = Conv(new float[] { TempX, a * TempX * TempX + b * TempX + c }, Multiplier, Divisor, Scale);
                            TempP = new PointF(TempC[0], TempC[1]);
                            Points[i] = TempP;
                        }
                        graphics.DrawCurve(DrawPen, Points); //draws graph with points
                        break;
                }
            }
            catch //error handling like a boss
            {
                lblSolution2.Visible = false;
                lblSolution.Text = "Uh Oh...";
            }
        }

        private float[] Conv(float[] CheckCoords, int Multiplier, int Divisor, int Scale) //converts graph points to c# panel points
        {
            float[] Coords = new float[2];
            Coords[0] = (CheckCoords[0] + Scale) *220 / Scale * Multiplier / Divisor; //scales for form size //account for scale using *210/scale
            Coords[1] = -(CheckCoords[1] - Scale/2) *220 / Scale * Multiplier / Divisor; //110*Scale/Multiplier gives Vertical Scale (i think)
            return Coords;
        }

        private void cboxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int GraphMode; //decides whether linear or quadratic
            int Scale = ScaleCheck();
            switch (cboxMode.Text)
            {
                case "Linear Line":
                    GraphMode = 1; //sets graphmode
                    lblYequal.Text = "Y ="; //sets relevant text and labels
                    lblFirstTerm.Text = "X +";
                    lblFirstTerm.Visible = true;
                    lblSecondTerm.Visible = false;
                    lblSolution.Visible = true;
                    lblSolution2.Visible = false;
                    txtFirstTerm.Text = "0.5";
                    txtFirstTerm.Visible = true;
                    txtSecondTerm.Text = "0";
                    txtSecondTerm.Visible = true;
                    txtThirdTerm.Visible = false;
                    UpdateGraph(GraphMode,Scale);
                    break;

                case "Quadratic Curve":
                    GraphMode = 2; //sets graphmode
                    lblYequal.Text = "Y ="; //sets relevant text and labels
                    lblFirstTerm.Text = "X^2 +";
                    lblFirstTerm.Visible = true;
                    lblSecondTerm.Text = "X +";
                    lblSecondTerm.Visible = true;
                    lblSolution.Visible = true;
                    txtFirstTerm.Text = "0.01";
                    txtFirstTerm.Visible = true;
                    txtSecondTerm.Text = "0";
                    txtSecondTerm.Visible = true;
                    txtThirdTerm.Text = "-75";
                    txtThirdTerm.Visible = true;
                    UpdateGraph(GraphMode,Scale);
                    break;
            }
        }

        private void NewGraph(object sender, EventArgs e)
        {
            int Scale = ScaleCheck();
            switch (cboxMode.Text) //uses cbox text to decide linear vs quadratic
            {
                case "Linear Line":
                    UpdateGraph(1,Scale);
                    break;
                case "Quadratic Curve":
                    UpdateGraph(2,Scale);
                    break;
            }
        }

        private int ScaleCheck()
        {
            int Scale = 220;
            switch (cboxGraphScale.Text) //changes background when needed
            {
                case "Scale: Regular":
                    pnlGraph.BackgroundImage = Properties.Resources._220x110;
                    break;
                case "Scale: Small":
                    pnlGraph.BackgroundImage = Properties.Resources._110x55;
                    Scale = 110;
                    break;
                case "Scale: Very Small":
                    pnlGraph.BackgroundImage = Properties.Resources._22x11;
                    Scale = 22;
                    break;
                case "Scale: Large":
                    pnlGraph.BackgroundImage = Properties.Resources._880x440;
                    Scale = 880;
                    break;
            }
            return Scale;
        }
    }
}