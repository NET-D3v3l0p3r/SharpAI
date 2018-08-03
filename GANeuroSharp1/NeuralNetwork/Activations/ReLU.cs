﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GANeuroSharp.NeuralNetwork.Activation
{
    public class ReLU : IActivation
    {
        public Func<double, double> Activation
        {
            get
            {
                return new Func<double, double>((x) => { return Math.Log(1.0 + Math.Exp(x)); });
            }
            set { }
        }

        public Func<double, double> Derivative
        {
            get
            {
                return new Func<double, double>((x) =>
                {
                    return 1.0 / (1.0 + Math.Exp(-x));
                });
            }
            set { }
        }
    }
}
