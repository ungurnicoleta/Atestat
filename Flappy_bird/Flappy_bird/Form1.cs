using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Flappy_bird.Properties;
using System.Threading;
using System.Media;
 
namespace Flappy_bird
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
        List<int> Tub1 = new List<int>();
        List<int> Tub2 = new List<int>();
        int TubLatime = 55;
        int PipeDifferentY = 140;
        int PipeDifferentX = 180;
        bool start = true;
        bool running;
        int pas = 5;
        int OriginalX, OriginalY;
        bool ReseteazaTub = false;
        int puncte;
        bool inTub = false;
        int scor;
        int diferenta_scor;
 
        private void Moare()
        {
            running = false;  
            timer2.Enabled = false;
            timer3.Enabled = false;
            button1.Visible = true;
            button1.Enabled = true;
            ReadAndShowScore();          
            puncte = 0;
            pictureBox1.Location = new Point(OriginalX, OriginalY);
            ReseteazaTub = true;
            Tub1.Clear();
        }
 
        private void ReadAndShowScore()
        {
            using (StreamReader reader = new StreamReader("Score.ini"))
            {
                scor = int.Parse(reader.ReadToEnd());
                reader.Close();
                if (int.Parse(label1.Text) == 0 | int.Parse(label1.Text) > 0)
                {
                    diferenta_scor = scor - int.Parse(label1.Text) + 1;
                }
                if (scor < int.Parse(label1.Text))
                {
                    MessageBox.Show(string.Format("Felicitari, ai facut un scor mai mare decat {0}. Noul scor e {1}", scor, label1.Text), "Flappy Bird", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    using (StreamWriter writer = new StreamWriter("Score.ini"))
                    {
                        writer.Write(label1.Text);
                        writer.Close();
                    }
                }
                if (scor > int.Parse(label1.Text))
                {
                    MessageBox.Show(string.Format("Iti mai trebuia {0} ca sa depasesti scorul max de {1}", diferenta_scor, scor), "Flappy Bird", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
 
                if (scor == int.Parse(label1.Text))
                {
                    MessageBox.Show(string.Format("Ai facut exact {0} (scorul max). Incearca sa-l depasesti de data asta.", scor), "Flappy Bird", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
 
        private void IncepeJoc()
        {
            ReseteazaTub = false;
            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Enabled = true;
            Random random = new Random();//randomiram tubi
            int num = random.Next(40, this.Height - this.PipeDifferentY);
            int num1 = num + this.PipeDifferentY;
            Tub1.Clear();
            Tub1.Add(this.Width); //adaugam lungimea
            Tub1.Add(num);
            Tub1.Add(this.Width);
            Tub1.Add(num1);
 
            num = random.Next(40, (this.Height - PipeDifferentY));
            num1 = num + PipeDifferentY;
            Tub2.Clear();
            Tub2.Add(this.Width + PipeDifferentX);
            Tub2.Add(num);
            Tub2.Add(this.Width + PipeDifferentX);
            Tub2.Add(num1);
           
            button1.Visible = false;
            button1.Enabled = false;
            running = true;
            Focus();//atunci cand se concentreaza actiunea pe elementul respectiv(pe buton)
        }
 
        private void Form1_Load(object sender, EventArgs e)
        {
            OriginalX = pictureBox1.Location.X;//stocheaza locatia pasarii in spatiu xOy
            OriginalY = pictureBox1.Location.Y;
            if (!File.Exists("Score.ini"))//daca nu exista fisierul sa il creeze
            {
                File.Create("Score.ini").Dispose();
            }
        }
 
        private void button1_Click(object sender, EventArgs e)
        {
            IncepeJoc();
        }
 
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
 
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Tub1[0] + TubLatime <= 0 | start == true)
            {
                Random aleator = new Random(); 
                int px = this.Width;
                int py = aleator.Next(40, (this.Height - PipeDifferentY));//y=inaltimea(diferenta de inaltime)
                var p2x = px;
                var p2y = py + PipeDifferentY;
                Tub1.Clear();
                Tub1.Add(px);
                Tub1.Add(py);
                Tub1.Add(p2x);
                Tub1.Add(p2y);//adaugam ce am creat randomizat
            }
            else
            {
                Tub1[0] = Tub1[0] - 2;
                Tub1[2] = Tub1[2] - 2;
            }
            if (Tub2[0] + TubLatime <= 0)
            {
                Random aleator = new Random();
                int px = this.Width;
                int py = aleator.Next(40, (this.Height - PipeDifferentY));
                var p2x = px;
                var p2y = py + PipeDifferentY;
                int[] p1 = { px, py, p2x, p2y };
                Tub2.Clear();
                Tub2.Add(px);
                Tub2.Add(py);
                Tub2.Add(p2x);
                Tub2.Add(p2y);
            }
            else
            {
                Tub2[0] = Tub2[0] - 2;
                Tub2[2] = Tub2[2] - 2;
            }
            if (start == true)
            {
                start = false;
            }
        }
 
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!ReseteazaTub && Tub1.Any() && Tub2.Any())//deseneaza conducte
            {
                //prima de sus
                e.Graphics.FillRectangle(Brushes.PeachPuff, new Rectangle(Tub1[0], 0, TubLatime, Tub1[1]));
                e.Graphics.FillRectangle(Brushes.LightSalmon, new Rectangle(Tub1[0] - 10, Tub1[3] - PipeDifferentY, 75, 15));//partea mai groasa din tub
                //prima de jos
                e.Graphics.FillRectangle(Brushes.DarkCyan, new Rectangle(Tub1[2], Tub1[3], TubLatime, this.Height - Tub1[3]));
                e.Graphics.FillRectangle(Brushes.Turquoise, new Rectangle(Tub1[2] - 10, Tub1[3], 75, 15));
                // a doua de sus
                e.Graphics.FillRectangle(Brushes.SlateGray, new Rectangle(Tub2[0], 0, TubLatime, Tub2[1]));
                e.Graphics.FillRectangle(Brushes.LightSkyBlue, new Rectangle(Tub2[0] - 10, Tub2[3] - PipeDifferentY, 75, 15));
                //a doua de jos
                e.Graphics.FillRectangle(Brushes.NavajoWhite, new Rectangle(Tub2[2], Tub2[3], TubLatime, this.Height - Tub2[3]));
                e.Graphics.FillRectangle(Brushes.Peru, new Rectangle(Tub2[2] - 10, Tub2[3], 75, 15));
 
            }
        }
 
        private void CheckForPoint()
        {
            Rectangle rec = pictureBox1.Bounds;
            Rectangle rec1 = new Rectangle(Tub1[2] + 20, Tub1[3] - PipeDifferentY, 15, PipeDifferentY);
            Rectangle rec2 = new Rectangle(Tub2[2] + 20, Tub2[3] - PipeDifferentY, 15, PipeDifferentY);
            Rectangle intersectia1 = Rectangle.Intersect(rec, rec1);//daca se intersecteaza pasarea cu tubii
            Rectangle intersectia2 = Rectangle.Intersect(rec, rec2);
            if (!ReseteazaTub | start)
            {
                if (intersectia1 != Rectangle.Empty | intersectia2 != Rectangle.Empty)
                {
                    if (!inTub)
                    {
                        puncte++;
                        SoundPlayer sp = new SoundPlayer(Flappy_bird.Properties.Resources.hit3);
                        sp.Play();
                        inTub = true;
                    }
                }
                else
                {
                    inTub = false;                  
                }
            }
        }
 
        private void CheckForCollision()
        {
            Rectangle rec = pictureBox1.Bounds;
            Rectangle rec1 = new Rectangle(Tub1[0], 0, TubLatime, Tub1[1]);
            Rectangle rec2 = new Rectangle(Tub1[2], Tub1[3], TubLatime, this.Height - Tub1[3]);
            Rectangle rec3 = new Rectangle(Tub2[0], 0, TubLatime, Tub2[1]);
            Rectangle rec4 = new Rectangle(Tub2[2], Tub2[3], TubLatime, this.Height - Tub2[3]);
            Rectangle intersectia1 = Rectangle.Intersect(rec, rec1);
            Rectangle intersectia2 = Rectangle.Intersect(rec, rec2);
            Rectangle intersectia3 = Rectangle.Intersect(rec, rec3);
            Rectangle intersectia4 = Rectangle.Intersect(rec, rec4);
            if (!ReseteazaTub | start)
            {
                if (intersectia1 != Rectangle.Empty | intersectia2 != Rectangle.Empty | intersectia3 != Rectangle.Empty | intersectia4 != Rectangle.Empty)
                {
                   SoundPlayer sp = new SoundPlayer(Flappy_bird.Properties.Resources.hit1);
                   sp.Play();
                    Moare();
                }
            }
        }
 
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    pas = -5;
                    pictureBox1.Image = Resources.Flappy_Bird1;
                    break;
            }
        }
 
        private void timer3_Tick(object sender, EventArgs e)
        {
            pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + pas);
            if (pictureBox1.Location.Y < 0)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, 0);
            }
            if (pictureBox1.Location.Y + pictureBox1.Height > this.ClientSize.Height)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, this.ClientSize.Height - pictureBox1.Height);
            }
            CheckForCollision();
            if (running)
            {
                CheckForPoint();
            }
            label1.Text = Convert.ToString(puncte);
        }
 
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    pas = 5;
                    pictureBox1.Image = Resources.Flappy_Bird_down;
                    break;
            }
        }

       

       
    }
}