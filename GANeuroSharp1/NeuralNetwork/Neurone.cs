using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GANeuroSharp.NeuralNetwork
{
    [Serializable]
    public class Neurone
    {
        private static int NEURONE_ID;
        
        public int ID { get; private set; }

        public IActivation Activation { get; set; }
        public List<Connection> Connections { get; set; }

        public double WeigtedInput { get; set; }
        public double Bias { get; set; }

        public double Delta { get; set; }

        public Neurone(IActivation activation, double bias)
        {
            NEURONE_ID++;
            ID = NEURONE_ID;

            Activation = activation;
            Connections = new List<Connection>();

            Bias = bias;
        }
        public void Fire() { Connections.ForEach(p => p.Fire()); }
        public void Clear()
        {
            WeigtedInput = 0;
        }

        public void Set(double value) { WeigtedInput += value; }
        public double Get() { return Activation.Activation(Bias + WeigtedInput); }

        public void Adjust(double delta)
        {
            Bias += delta;
        }

        public void CalculateDelta(bool isOutput, double requestedValue, Layer nextL)
        {
            switch (isOutput)
            {
                case true:
                    Delta = Activation.Derivative(WeigtedInput + Bias) * (requestedValue - Get());
                    break;

                case false:
                    double sum = .0d;

                    for (int i = 0; i < nextL.Neurones.Length - 1; i++)
                        sum += nextL.Neurones[i].Delta * Connections.Find(p => p.Axon.Equals(nextL.Neurones[i])).Weight;


                    Delta = Activation.Derivative(WeigtedInput + Bias) * sum;
                    break;
            }
        }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }
        public override int GetHashCode()
        {
            return ID;
        }
    }
}
