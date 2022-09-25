using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neural_networks
{
    class ANNs
    {
        List<Perceptron> _perceptrons = new List<Perceptron>();
        List<string> _symbols = new List<string>() {"0" ,"1", "2" , "3", "4" /*,"5", "6", "7", "8", "9"*/};
        int countRandomFile = 1000;
        int _sizeImage = 32;
        public ANNs()
        {
            foreach (var i in _symbols)
                _perceptrons.Add(new Perceptron(i));
        }

        public void TrainingPerceptron(string path)
        {
            var allDirectory = Directory.GetDirectories(path).ToList();

            List<DirectoryInfo> directoryInfo = new List<DirectoryInfo>();

            foreach (var i in allDirectory)
                directoryInfo.Add(new DirectoryInfo(i));
            var count = 500;
            while (count > 0)
            {
                foreach (var i in _perceptrons)
                {
                    var needDirectory = directoryInfo.First(s => s.Name == i.Symbol);

                    var files = needDirectory.GetFiles();

                    foreach (var file in files)
                        i.AddArrayPixels(ReaderFile.GetInformationPic(file.FullName));

             
                }

                count--;
            }

            foreach (var i in _perceptrons)
            {
                var random = new Random();

                while (countRandomFile > 0)
                {
                    var needRandomDirectory = directoryInfo[random.Next(directoryInfo.Count)];
                    var files = needRandomDirectory.GetFiles();

                    var needRandomfile = files[random.Next(files.Length)];
                    if (needRandomDirectory.Name != i.Symbol)
                    {

                        i.SubtractStep(ReaderFile.GetInformationPic(needRandomfile.FullName));

                    }
                    else
                    {
                        i.AddArrayPixels(ReaderFile.GetInformationPic(needRandomfile.FullName));
                    }
                    countRandomFile--;
                }
                countRandomFile = 1000;
            }


        }
        public Dictionary<string, double> AnalysisImage(int[,] image)
        {
            var sumUnitImage = GetSumUnitPixels(image);
            Dictionary<string, double> resultWeight = new Dictionary<string, double>();

            foreach (var i in _perceptrons)
                resultWeight.Add(i.Symbol, ( i.SumWeight(image) / (double)sumUnitImage ));

            return resultWeight;
        }

        private int GetSumUnitPixels(int[,] image)
        {
            int result = 0;

            for (int i = 0; i < _sizeImage; i++)
            {
                for (int j = 0; j < _sizeImage; j++)
                {
                    if (image[i, j] == 1)
                        result++;
                }
            }

            return result;
        }


    }
}
