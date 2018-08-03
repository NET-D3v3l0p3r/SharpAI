using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GANeuroSharp.NeuralNetwork
{
    public interface IActivation
    {
        Func<double, double> Activation { get; set; }
        Func<double, double> Derivative { get; set; }
    }
}
