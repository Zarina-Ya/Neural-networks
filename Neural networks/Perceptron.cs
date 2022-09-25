﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neural_networks
{
    class Perceptron
    {
        int _sizeImage = 32;
        double[,] _weightPixel;
        string _symbol;
        double _step = 0.001d;
        public Perceptron(string value)
        {
            _symbol = value;
            _weightPixel = new double[_sizeImage, _sizeImage];
        }

        public string Symbol { get => _symbol; set => _symbol = value; }

        public void AddArrayPixels(int[,] array, double value)
        {
            for(int i = 0; i < _sizeImage; i++)
            {
                for(int j = 0; j < _sizeImage; j++)
                {
                    if (array[i, j] == 1)
                    {
                        _weightPixel[i, j] += value;
                        if (_weightPixel[i, j] > 1)
                            _weightPixel[i, j] = 1;
                    }
                }
            }
        }

        public void SubtractStep(int[,] array, double value)
        {
            for (int i = 0; i < _sizeImage; i++)
            {
                for (int j = 0; j < _sizeImage; j++)
                {
                    if (array[i, j] == 1)
                    {

                        _weightPixel[i, j] -= value;
                        if (_weightPixel[i, j] < 0)
                            _weightPixel[i, j] = 0;
                    }
                    
                }
            }
        }

        public double SumWeight(int[,] pixelsImage)
        {
            double sum = 0.0;
            for(int i = 0; i < _sizeImage; i++)
            {
                for(int j = 0; j < _sizeImage; j++)
                {
                    if (pixelsImage[i, j] == 1)
                        sum += _weightPixel[i, j];
                }
            }
            return sum;
        }
    }
}