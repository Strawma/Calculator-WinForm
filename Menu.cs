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
    public partial class frmMode : Form
    {
        Color[] FormColours = RandomColour(); //form colour array, 0 = text colour, 1 = bg colour
        int[] CustomIndexes = RandomNumbers(6, 4); //Music + Image Indexes, 0 = music, 1 = bg image
        int PrevSize = 1; //for sizing form correctly
        int Multiplier = 1; //for setting correct form sizes
        int Divisor = 1;

        System.Media.SoundPlayer MusicPlayer = new System.Media.SoundPlayer(); //music player for form

        public frmMode()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Music(CustomIndexes[0]); //background music played at loading
            ChangeColour(this); //sets form colour
        }

        private void Music(int Sound) //plays background music for awesome calculator Goodness
        {
            System.IO.Stream MusicFile = Properties.Resources.MusicA; //uses system media player //MEDIA PLAYER IS GARBAGE
            switch (Sound) //music depends on char given, saved in system resources 
            {
                case 0:
                    MusicFile = Properties.Resources.MusicA;
                    break;
                case 1:
                    MusicFile = Properties.Resources.MusicB;
                    break;
                case 2:
                    MusicFile = Properties.Resources.MusicC;
                    break;
                case 3:
                    MusicFile = Properties.Resources.MusicD;
                    break;
                case 4:
                    MusicFile = Properties.Resources.MusicE;
                    break;
                case 5:
                    MusicFile = Properties.Resources.MusicF;
                    break;
                case 6:
                    MusicFile = Properties.Resources.MusicG;
                    break;
            }
            MusicPlayer.Dispose();
            MusicPlayer = new System.Media.SoundPlayer(MusicFile);

            MusicPlayer.PlayLooping();

            if (Sound == 7) //stops music with input of 0
            {
                MusicPlayer.Stop();
            }
        }

        private void rbtn_CheckedChanged(object sender, EventArgs e) //radiobox for switching forms
        {
            this.Hide(); //menu form hidden
            if (rbtnCalc.Checked == true) //when a box is checked...
            {
                Form Calcform = new frmBaseCalculator();
                rbtnCalc.Checked = false; //box unchecked
                ChangeColour(Calcform);
                FormResize(Calcform, Multiplier, Divisor);
            }
            else if (rbtnGraphing.Checked == true)
            {
                Form Graphingform = new frmGraphingCalc(); //new form changed according to button press
                rbtnGraphing.Checked = false;
                ChangeColour(Graphingform);
                FormResize(Graphingform, Multiplier, Divisor);
            }
            else if (rbtnProgrammer.Checked == true)
            {
                Form Programmerform = new frmProgrammerCalc(); //new form changed according to button press
                rbtnProgrammer.Checked = false;
                ChangeColour(Programmerform);
                FormResize(Programmerform, Multiplier, Divisor);
            }
        }

        private void ChangeColour(Form form) //for changing form colour
        {
            foreach (Control c in form.Controls)
            {
                c.ForeColor = FormColours[0]; //changes text + background colours
                c.BackColor = FormColours[1];
            }
            switch (CustomIndexes[1]) //changes form image
            {
                case 0:
                    form.BackgroundImage = Properties.Resources.gradient;
                    break;
                case 1:
                    form.BackgroundImage = Properties.Resources.skeleton;
                    break;
                case 2:
                    form.BackgroundImage = Properties.Resources.cat;
                    break;
                case 3:
                    form.BackgroundImage = Properties.Resources.HappyMan;
                    break;
                case 4:
                    form.BackgroundImage = Properties.Resources.Crab_Knife;
                    break;

            }
        }

        private void FormResize(Form form, int Multiplier, int Divisor) //for changing form size //WARNING: CODE SUCKS
        {
            form.Width = form.Width * Multiplier / Divisor; //uses multiplier to scale form up, DIvisor (when needed) to scale down
            form.Height = form.Height * Multiplier / Divisor;
            foreach (Control c in form.Controls) //uses multiplier + divisor to scale form as well
            {
                float FontSize = c.Font.Size * Multiplier / Divisor;
                c.Font = new Font("Comic Sans MS", FontSize, FontStyle.Regular); //changes font as well, OH YEAH

                c.Top = c.Top* Multiplier / Divisor; 
                c.Left = c.Left* Multiplier / Divisor;
                c.Width = c.Width * Multiplier / Divisor;
                c.Height = c.Height * Multiplier / Divisor;
            }
            form.Show(); //form show at end
        }

        private static Color[] RandomColour() //for getting random number
        {
            Random r = new Random(); //creates randomiser
            Color[] Colours = new Color[2];
            Colours[0] = (Color.FromArgb(r.Next(256), r.Next(256), r.Next(256))); //generates random colour for text
            Colours[1] = (Color.FromArgb(255 - Colours[0].R, 255 - Colours[0].G, 255 - Colours[0].B)); //gets complimentary colour for background
            return Colours;
        }

        private static int[] RandomNumbers(int LimitA, int LimitB) //for getting random number
        {
            Random r = new Random(); //makes randomiser //why why why why why?????????????
            int[] Numbers = new int[2];
            Numbers[0] = r.Next(LimitA);
            Numbers[1] = r.Next(LimitB);
            return (Numbers);
        }

        //----------------------------------------------------------------Buttons-------------------------------------------------------------

        private void btnColour_Click(object sender, EventArgs e) //buttons for changing calc colour
        {

            Button button = sender as Button;
            this.Hide(); //hides form while dialogue open
            DialogResult Colour = colourDialog.ShowDialog(); //dialogue used for colour selection
            if (Colour == DialogResult.OK)
            {
                switch (button.Text)
                {
                    case "Text Colour": //changes text/background depending on button pressed
                        FormColours[0] = colourDialog.Color;
                        break;
                    case "Background Colour":
                        FormColours[1] = colourDialog.Color;
                        break;
                }
                ChangeColour(this);
            }
            this.Show();
        }

        private void cboxImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox Combo = sender as ComboBox;
            switch (Combo.Text) //gets turns input into character
            {
                case "Rainbow Gradient": //Background Inputs change custom index
                    CustomIndexes[1] = 0;
                    break;
                case "Cool Skeleton":
                    CustomIndexes[1] = 1;
                    break;
                case "Cat":
                    CustomIndexes[1] = 2;
                    break;
                case "Happy Man":
                    CustomIndexes[1] = 3;
                    break;
                case "Crab":
                    CustomIndexes[1] = 4;
                    break;
            }
            ChangeColour(this); //change bg colour
            cboxBackImage.Text = "Background Image"; //resets text on combo box
        }

        private void cboxMusic_SelectedIndexChanged(object sender, EventArgs e) //if music combobox changes
        {
            ComboBox Combo = sender as ComboBox;
            Char Input = Combo.Text.Last();
            switch (Input) //turns final char of input into character
            {
                case 'e': //if none 'e', use 7 (no song)
                    CustomIndexes[0] = 7;
                    break;
                default: //else use int of input
                    int IntInput = Input;
                    CustomIndexes[0] = Input - 65; //-65 when converting char of letter to int
                    break;
            }
            Music(CustomIndexes[0]); //changes music
            cboxMusic.Text = "Music";
        }

        private void btnRandomise_Click(object sender, EventArgs e) //Randomise Button!
        {
            FormColours = RandomColour(); //form colour array, 0 = text colour, 1 = bg colour
            CustomIndexes = RandomNumbers(7, 5); //Music + Image Indexes, 0 = music, 1 = bg image
            ChangeColour(this);
            Music(CustomIndexes[0]);
        }

        private void trckFormSize_Scroll(object sender, EventArgs e) //WARNING: CODE GARBAGE, PLEASE AVERT EYES
        {
            switch(trckFormSize.Value) //form sizes x1, x1.5, x2, x2.5
            {
                case 1: 
                    Multiplier = 1;
                    Divisor = 1;
                    break;
                case 2:
                    Multiplier = 3;
                    Divisor = 2;
                    break;
                case 3:
                    Multiplier = 2;
                    Divisor = 1;
                    break;
                case 4:
                    Multiplier = 5;
                    Divisor = 2;
                    break;
            }
            int SMultiplier = 1; //special multiplier just for this form since i cant close this!!!
            int SDivisor = 1; //it even gets a divisor??
            switch (trckFormSize.Value)
            {
                case 1:
                    SMultiplier = 2;
                    SDivisor = 3; //going back 2-1, means multiplying by 2/3
                    break;
                case 2:
                    switch (PrevSize)
                    {
                        case 1:
                            SMultiplier = 3; //going by 1-2, multiply by 3/2
                            SDivisor = 2;
                            break;
                        case 3:
                            SMultiplier = 3; //going from 3-2, you gotta multiply by 3/4
                            SDivisor = 4;
                            break;
                    }
                    break;
                case 3:
                    switch (PrevSize)
                    {
                        case 2:
                            SMultiplier = 4; //going by 1-2, multiply by 4/3
                            SDivisor = 3;
                            break;
                        case 4:
                            SMultiplier = 4; //going from 3-2, you gotta multiply by 4/5
                            SDivisor = 5;
                            break;
                    }
                    break;
                case 4:
                    SMultiplier = 5; //going back 2-3, means multiplying by 5/4
                    SDivisor = 4;
                    break;
            }
            PrevSize = trckFormSize.Value; //sets prevsize for next run
            this.Hide();
            FormResize(this, SMultiplier, SDivisor); //resizes this form
        }
    }
}