using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GANeuroSharp.NeuralNetwork.Activation
{
    public class Tanh : IActivation
    {
        public Func<double, double> Activation
        {
            get
            {
                return new Func<double, double>((x) => { return 1.7159d * Math.Tanh((2.0d / 3.0d) * x); });
            }
            set { }
        }

        public Func<double, double> Derivative
        {
            get
            {
                return new Func<double, double>((x) =>
                {
                    return (17159 * _sech2((2.0d / 3.0d) * x)) / 15000d;
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
