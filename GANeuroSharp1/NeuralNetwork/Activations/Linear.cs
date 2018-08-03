using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GANeuroSharp.NeuralNetwork.Activation
{
    [Serializable]
    public class Linear : IActivation
    {
        public Func<double, double> Activation
        {
            get
            {
                return new Func<double, double>((x) => { return x; });
            }
            set { }
        }

        public Func<double, double> Derivative
        {
            get
            {
                return new Func<double, double>((x) =>
                {
                    return 1.0d;
                });
            }
            set { }
        }
    }
}
