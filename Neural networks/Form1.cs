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
     
        int _sizeImage = 32;
        Layer layer;
        Layer layer1;
        Layer layer2;
        Layer layer3;
        int countRandomFile = 300;

        List<DirectoryInfo> directoryInfo;

        private string[] _alphobet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        private static int _epoch = 15;
        public Form1()
        {
             layer = new Layer(LayerType.Input, 1024, 1);

             layer1 = new Layer(LayerType.Hidden, 512, 1024);
             layer2 = new Layer(LayerType.Hidden, 256, 512);
             layer3 = new Layer(LayerType.Output, 10, 256);

            layer.next = layer1;

            layer1.next = layer2;
            layer1.pref = layer;

            layer2.next = layer3;
            layer2.pref = layer1;

            layer3.pref = layer2;

            InitializeComponent();
            graphics = Graphics.FromImage(big);

            chart1.Series[0].Points.Clear();

            chart2.Series[0].Points.Clear();
                

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
            => ClearPanel();

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
          
            var res = (layer.MakePrediction(ConevertDoubleToArray(small)));
            res.Print(true);

            Dictionary<string, double> map = new Dictionary<string, double>();
            for (int i = 0; i < res.Length; i++)
                map.Add(_alphobet[i], res[i]);

           var resultMap =  map.OrderBy(pair => pair.Value).ToList();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var i in resultMap)
            {
                stringBuilder.Append(i.ToString());
                stringBuilder.AppendLine();
            }
         
            label1.Text = stringBuilder.ToString();
        }

        private double[] ConevertDoubleToArray(Bitmap small)
        {
            double[,] matrix = new double[_sizeImage, _sizeImage];
            Color[,] colorPixel = new Color[_sizeImage, _sizeImage];

            Color colorWhile = Color.White;
            for (int i = 0; i < _sizeImage; i++)
            {
                for (int j = 0; j < _sizeImage; j++)
                {
                    colorPixel[i, j] = small.GetPixel(i, j);
                    if (colorPixel[i, j].R < Color.White.R && colorPixel[i, j].B < Color.White.B && colorPixel[i, j].G < Color.White.G)
                        matrix[i, j] = 1;
                    else matrix[i, j] = 0;
                }
            }

          List<double> result = new List<double>();

            for(int i = 0; i < 32; i++)
            {
                for(int j = 0; j < 32; j++)
                {
                    result.Add(matrix[i, j]);
                }
            }

            return result.ToArray();
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            var value = Array.IndexOf(_alphobet, textBox1.Text);
            var errorVector = new double[10];

                errorVector[value] = 1.0;
                var res = (layer.NewLearning(ConevertDoubleToArray(small), errorVector));

                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < res.Length; i++)
                    stringBuilder.Append($"{_alphobet[i]} - {res[i]} {'\n'}");
                button5_Click(sender, e);
   
        

        }


        public void Training(string path)
        {
            var allDirectory = Directory.GetDirectories(path).ToList();
        
            directoryInfo = new List<DirectoryInfo>();

            foreach (var i in allDirectory)
                directoryInfo.Add(new DirectoryInfo(i));

            var random = new Random();
            _epoch--;
            while (countRandomFile > 0 && _epoch != 0)
            {
               
                label2.Text =$" Epoch : { _epoch}";
                var needRandomDirectory = directoryInfo[random.Next(directoryInfo.Count)];
                var files = needRandomDirectory.GetFiles();

                var needRandomfile = files[random.Next(files.Length)];
                var imageArray = ReaderFile.GetInformationPic(needRandomfile.FullName);

                List<double> resultVector = new List<double>();

                for (int i = 0; i < 32; i++)
                    for (int j = 0; j < 32; j++)
                        resultVector.Add(imageArray[i, j]);



                var value = Array.IndexOf(_alphobet, needRandomDirectory.Name);
                var errorVector = new double[10];

                errorVector[value] = 1.0;
                var res = (layer.NewLearning(resultVector.ToArray(), errorVector));

                countRandomFile--;
            }

            countRandomFile = 300;

         
            AccuracyCheck();

        }

        private void AccuracyCheck()
        {

            var tmpResultVector = new double[10];
            var allDirectory = Directory.GetDirectories("C:/OtherDataset/").ToList();
            double allAccurucy = 0.0;
            directoryInfo = new List<DirectoryInfo>();

            foreach (var i in allDirectory)
                directoryInfo.Add(new DirectoryInfo(i));

            var random = new Random();
            var count = 20;
            while (count > 0 )
            {
                var needRandomDirectory = directoryInfo[random.Next(directoryInfo.Count)];
                var files = needRandomDirectory.GetFiles();

                var needRandomfile = files[random.Next(files.Length)];
                var imageArray = ReaderFile.GetInformationPic(needRandomfile.FullName);

                List<double> resultVector = new List<double>();

                for (int i = 0; i < 32; i++)
                    for (int j = 0; j < 32; j++)
                        resultVector.Add(imageArray[i, j]);

                var value = Array.IndexOf(_alphobet, needRandomDirectory.Name);
                var errorVector = new double[10];

                errorVector[value] = 1.0;

                var res = (layer.MakePrediction(resultVector.ToArray()));
                allAccurucy = CalculateAccuracy(res, errorVector);
                count--;

                for (int i = 0; i < 10; i++)
                {
                    tmpResultVector[i] += res[i] - errorVector[i];
                }
            }
            count = 20;


            UpdatePlot(tmpResultVector, count) ;
            chart2.Series[0].Points.AddY(allAccurucy / count);
        }

        private void UpdatePlot(double[] val, int count)
        {
            double result = 0.0;
            for (int i = 0; i < val.Length; i++)
                result += Math.Abs(val[i]);
            chart1.Series[0].Points.AddY(result / count);
        }
       

        private double CalculateAccuracy(double[] res, double[] ogudanie)
        {
            double TN = 0.0, FN = 0.0, FP = 0.0, TP = 0.0;
            for (int i = 0; i < res.Length; i++)
            {
                if (ogudanie[i] == 1.0 && res[i] >= 0.7)
                    TP++;
                else if (ogudanie[i] == 1.0 && res[i] < 0.7)
                    FP++;
                else if (ogudanie[i] == 0.0 && res[i] > 0.4)
                    FN++;
                else if (ogudanie[i] == 0.0 && res[i] <= 0.4)
                    TN++;
            }
      

            var precision = (TP) / (FP + TP);
            var recall = (TP) / (FN + TP);
            label3.Text = $"Precision = {precision}";
            label4.Text = $"Recall = {recall}";
            label5.Text = $"Score = { (2 * precision * recall) / (precision + recall)}";

            return (TP + TN) / (FP + FN + TP + TN);
        }
      
        private void button1_Click(object sender, EventArgs e)
            => Training(_path);
    }
}
