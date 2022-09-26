using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neural_networks
{
    class ReaderFile
    {

        private static int _sizeMatrix = 32;
        private int[,] _picMatrix;
 
      
        public static int[,] GetInformationPic(string file)
        {
           var image = Image.FromFile(file);
           Bitmap b1 = new Bitmap(image);
            int hight = b1.Height;
            int width = b1.Width;

            if (hight != _sizeMatrix || width != _sizeMatrix) throw new Exception("this file is incorrect");

            int[,] result = new int[_sizeMatrix, _sizeMatrix];
            Color[,] colorPixel = new Color[_sizeMatrix, _sizeMatrix];

            Color colorWhile = Color.White;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < hight; j++)
                {
                    colorPixel[i,j] = b1.GetPixel(i, j);
                    if (colorPixel[i, j].R < Color.White.R && colorPixel[i, j].B < Color.White.B && colorPixel[i, j].G < Color.White.G)
                        result[i, j] = 1;
                    else result[i, j] = 0;
                }
            }
            return result;
        }
    }
}
