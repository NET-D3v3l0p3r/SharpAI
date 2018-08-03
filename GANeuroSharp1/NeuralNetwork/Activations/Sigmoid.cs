using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GANeuroSharp.NeuralNetwork.Activation
{
    [Serializable]
    public class Sigmoid: IActivation
    {
        public Func<double, double> Activation
        {
            get
            {
                return new Func<double, double>((x) => { return 1.0 / (1.0 + Math.Exp(-x)); });
            }
            set { }
        }

        public Func<double, double> Derivative
        {
            get
            {
               
                return new Func<double, double>((x) =>
                {

                    return Activation(x) * (1.0 - Activation(x));
                });
            }
            set { }
        }

        private static Func<double, double> _sech = new Func<double, double>((x) =>
        {
            return 1.0d / Math.Cosh(x);
        });

        private static Func<double, double> _sech2 = new Func<double, double>((x) =>
        {
            return _sech(x) * _sech(x);
        });
    }
}
