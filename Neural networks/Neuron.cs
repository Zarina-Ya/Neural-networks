﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Neural_networks
{
    internal class Neuron
    {
        double[] Weights { get; }// vector
        double SigmoidWeight;
 
        public Neuron(double[] neruron, double[,] matrix, int countNeuron)
        {
            Weights = matrix.VectorMatrixMultiplication(neruron);
            var tmp = 0.0;
            foreach (var i in Weights)
                tmp += i;
            //SigmoidWeight = Sigmoid(tmp);
        }


       

    }

    static class Ext
    {
        public static int RowsCount(this double[,] matrix)
            => matrix.GetUpperBound(0) + 1;
        public static int ColumnsCount(this double[,] matrix)
            => matrix.GetUpperBound(1) + 1;
        public static double[] VectorMatrixMultiplication(
            this double[,] inputMartix, 
            double[] vector)
        {
            var resultMatrix = new double[inputMartix.RowsCount()];
            for (var i = 0; i < inputMartix.RowsCount(); i++)
            {
                resultMatrix[i] = 0;

                for (var k = 0; k < inputMartix.ColumnsCount(); k++)
                {
                    resultMatrix[i] += inputMartix[i, k] * vector[k];
                }
            }
            return resultMatrix;
        }


        // метод для печати матрицы в консоль
        public static void Print(this double[,] matrix)
        {
            for (var i = 0; i < matrix.RowsCount(); i++)
            {
                for (var j = 0; j < matrix.ColumnsCount(); j++)
                {
                    Console.Write(matrix[i, j].ToString().PadLeft(4));
                }

                Console.WriteLine();
            }
        }

        // метод для печати вектора в консоль
        public static void Print(this double[] vector, bool vectorIsRow)
        {
            for (var i = 0; i < vector.Length; i++)
            {
                Console.Write(vector[i].ToString().PadLeft(4) + (vectorIsRow ? string.Empty : "\r\n"));
            }
            Console.WriteLine();
        }
    }
}
