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
        Pen pen = new Pen(Color.Black, 9);
        Bitmap big = new Bitmap(100, 100);
        Bitmap small;
        Graphics graphics;
        string _path = "C:/DataSet/";
        ANNs aNNs;
        int _sizeImage = 32;
        public Form1()
        {
            aNNs = new ANNs();
            aNNs.TrainingPerceptron(_path);
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
            DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(_path,textBox1.Text));
            FileInfo[] files = dir.GetFiles();
            CheckColorImage();
            small.Save(dir.FullName + $"/{files.Length}.bmp", ImageFormat.Bmp);
        }

        private void CheckColorImage()
        {
            for(int i = 0; i < small.Height; i++)
            {
                for(int j = 0; j < small.Width; j++)
                {
                    Color pixelColor = small.GetPixel(i,j);
                    if (pixelColor.R < Color.White.R && pixelColor.B <  Color.White.B && pixelColor.G < Color.White.G)
                        small.SetPixel(i, j, Color.Black);
                }
            }
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

        private void button5_Click(object sender, EventArgs e)
        {
           
            var result = aNNs.AnalysisImage(ConevertToArray(small));

           

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var i in result.OrderBy(pair => pair.Value))
                stringBuilder.Append($"{i.Key} - {i.Value} {'\n'}");

            label1.Text = stringBuilder.ToString();
        }


        private int[,] ConevertToArray(Bitmap image)
        {
            int[,] result = new int[_sizeImage, _sizeImage];
            Color[,] colorPixel = new Color[_sizeImage, _sizeImage];

            Color colorWhile = Color.White;
            for (int i = 0; i < _sizeImage; i++)
            {
                for (int j = 0; j < _sizeImage; j++)
                {
                    colorPixel[i, j] = image.GetPixel(i, j);
                    if (colorPixel[i, j].R < Color.White.R && colorPixel[i, j].B < Color.White.B && colorPixel[i, j].G < Color.White.G)
                        result[i, j] = 1;
                    else result[i, j] = 0;
                }
            }
            return result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            aNNs.PunishSystem(ConevertToArray(small), textBox1.Text);

            var result = aNNs.AnalysisImage(ConevertToArray(small));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var i in result.OrderBy(pair => pair.Value))
                stringBuilder.Append($"{i.Key} - {i.Value} {'\n'}");

            label1.Text = stringBuilder.ToString();

        }
    }
}
