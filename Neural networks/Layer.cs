using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neural_networks
{
    internal class Layer
    {
        public Layer next;
        public Layer pref;
        LayerType layerType;
        double[,] matrix;
        double[,] newMatrix;
        static double l = 0.1;
        Random random = new Random();

        public static double[] _tmpResultVector = new double[10];
        public static (double[] res, double[] ogudanie) _accurucy;
  


        public Layer(LayerType layerType, int sizethis, int sizepref )
        {
            this.layerType = layerType;

            if (layerType != LayerType.Input)
                matrix = new double[sizethis, sizepref];
            else
                matrix = new double[sizethis, 1];
            

            for (int i = 0; i < matrix.GetUpperBound(0) + 1; i++) 
                for(int j = 0; j < matrix.GetUpperBound(1) + 1; j++)
                    matrix[i, j] = random.NextDouble() - 0.5;
         
        }


        public double[] SdelatPredskazanie(double[] pick)
        {
            
            if(layerType == LayerType.Input)
                return next.SdelatPredskazanie(pick);

            var result = matrix.VectorMatrixMultiplication( pick);
            var newResult = new double[result.Length];

            for (int i = 0; i < result.Length; i++)
                newResult[i] = Sigmoid (result[i]);
            

            if (next != null)
                return next.SdelatPredskazanie(newResult);

            else
                return newResult;


        }

       
        public void ChangeWeight()
        {
            matrix = newMatrix;
            if(layerType != LayerType.Output)
                next.ChangeWeight();    
        }

        public double[] NewLearning(double[] pick, double[] ogudanie)
        {
            if (layerType == LayerType.Input)
            {
                var res =  next.NewLearning(pick, ogudanie);
                next.ChangeWeight();
                return res;
            }

            var tmpVector = this.matrix.VectorMatrixMultiplication(pick);
            var vector = new double[tmpVector.Length];
            for (int i = 0; i < tmpVector.Length; i++)
                vector[i] = Sigmoid(tmpVector[i]);

            if (layerType == LayerType.Output)
            {
                var sigma = SigmaOUtput(vector, ogudanie, tmpVector);
                UpdateWeight(pick, sigma);
                return sigma;
            }
            else
            {
                var nextSigma = next.NewLearning(vector, ogudanie);
                var sigma = SigmaHidden(nextSigma, tmpVector);
                UpdateWeight(pick, sigma);
                return sigma; 
            }
        }



        public double[] SigmaOUtput(
            double[] res,
            double[] ogudanie, 
            double[] doFuncActivation)
        {
            double[] needVector = new double[res.Length] ;

            for (int i = 0; i < res.Length; i++)
            {
                needVector[i] = (res[i] - ogudanie[i]) * SigmoidDx(doFuncActivation[i]);

                if(_tmpResultVector == null)
                    _tmpResultVector = new double[res.Length];

                _tmpResultVector[i] += res[i] - ogudanie[i];

                if (_accurucy.ogudanie == null)
                    _accurucy.ogudanie = new double[res.Length];

                if(_accurucy.res == null)
                    _accurucy.res = new double[res.Length];

                _accurucy.res = res;
                _accurucy.ogudanie = ogudanie;
            }
            return needVector;
        }

        public double[] SigmaHidden(double[] nextSigma, double[] tmpVector)
        {
            double[] needVector = new double[matrix.GetUpperBound(0)+1];

            for (int i = 0; i < needVector.Length; i++)
            {
                for (int j = 0; j < nextSigma.Length; j++)
                    needVector[i] += nextSigma[j] * next.matrix[j, i];
                needVector[i] *= SigmoidDx(tmpVector[i]);
            }

            return needVector;
        }

        private void UpdateWeight(double[] inputVect, double[] sigma)
        {
            newMatrix = new double[sigma.Length, inputVect.Length];


            for (int i = 0; i < inputVect.Length; i++)
            {
                for(int j = 0 ; j < sigma.Length; j++)
                {
                    newMatrix[j,i] = matrix[j, i] - inputVect[i] * l * sigma[j];
                }
            }
        }

     
        private double Sigmoid(double x)
           => 1.0 / (1.0 + Math.Pow(Math.E, -x));
        private double SigmoidDx(double x)
            => this.Sigmoid(x) * (1 - this.Sigmoid(x));

    }

    public enum LayerType
    {
        Input,
        Output,
        Hidden
    }


}
