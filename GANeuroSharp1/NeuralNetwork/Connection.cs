using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GANeuroSharp.NeuralNetwork
{
    [Serializable]
    public class Connection
    {
        public Neurone Dendrite { get; set; }
        public Neurone Axon { get; set; }

        public double Weight { get; set; }

        public Connection(Neurone a, Neurone b, double weight)
        {
            Dendrite = a;
            Axon = b;

            Weight = weight;
        }

        public void Fire()
        {
            Axon.Set(Dendrite.Get() * Weight);
        }

        double lastWeight = 1;
        public void Adjust(double delta, double momentum)
        {
            Weight += delta + (lastWeight * momentum);
            lastWeight = delta;
        }

    }
}
