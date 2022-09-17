using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neural_networks
{
    public partial class Form1 : Form
    {
        bool IsDrawing = false;
        Pen pen = new Pen(Color.Black, 4);
        Bitmap big = new Bitmap(100, 100);
        Bitmap small;
        Graphics graphics;
        public Form1()
        {
            InitializeComponent();
            graphics = Graphics.FromImage(big);
        }
       

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (IsDrawing)
                IsDrawing = false;
            else IsDrawing = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (small != null && !String.IsNullOrEmpty(textBox1.Text)) 
            {
                CheckDirectory();
                ClearPanel();
            }
            else textBox1.Text = "Fill in the field";
        }
        private void CheckDirectory()
        {
            DirectoryInfo dir = Directory.CreateDirectory($"C://Users//zarin//source//repos//Neural networks//Neural networks//DataSet/{textBox1.Text}");
            FileInfo[] files = dir.GetFiles();
            small.Save(dir.FullName + $"/{files.Length}.bmp", ImageFormat.Bmp);
        }

        private void ClearPanel()
        {
            graphics.Clear(Color.White);
            textBox1.Text = String.Empty;
            UpdateBitmaps();
        }
        private void UpdateBitmaps()
        {
            small = new Bitmap(Image.FromHbitmap(big.GetHbitmap()), 32, 32);
            pictureBox1.Image = big;
            pictureBox2.Image = small;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearPanel();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDrawing)
            {
                graphics.DrawRectangle(pen, e.X, e.Y, 3 ,1);
                UpdateBitmaps();
            }
        }

        

    }
}
