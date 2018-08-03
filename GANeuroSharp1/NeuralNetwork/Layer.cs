using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GANeuroSharp.NeuralNetwork;
using GANeuroSharp.Data;
using GANeuroSharp.Helpers;
using GANeuroSharp.Converters;

namespace GANeuroSharp.NeuralNetwork
{
    [Serializable]
    public class Layer
    {
        public IActivation ActivationFunction { get; set; }

        public Layer PreviousLayer { get; set; }
        public Layer NextLayer { get; set; }

        public int NeuroneCount { get; private set; }
        public Neurone[] Neurones { get; private set; }

        public double Bias { get; private set; }

        public DNA<double> Parent { get; private set; }
        public DNA<double> DNA { get; private set; }

        private double[] _ParentWeights;

        public static double[] RequestedValues { get; set; }


        public Layer(int n, IActivation f, DNA<double> parent = null)
        {
            NeuroneCount = n;
            ActivationFunction = f;

            Neurones = new Neurone[n];

            Parent = parent;
            Bias = Globals.GlobalRandom.Next(-1000, 1000) * 0.001;
            if (parent != null)
            {
                Bias = parent.GetSegmentValue(0);
                _ParentWeights = parent.GetFullSegments(1);
            }
            DNA = new DNA<double>(new BinaryConverter());
            DNA.AddSegment(Bias);

            for (int i = 0; i < Neurones.Length; i++)
                Neurones[i] = new Neurone(f, Bias);
        }

        public void MeshWithNextLayer()
        {
            int index = 0;
            for (int i = 0; i < Neurones.Length; i++)
            {
                Neurone current = Neurones[i];
                for (int j = 0; j < NextLayer.Neurones.Length; j++)
                {
                    var weight = Globals.GlobalRandom.Next(-1000, 1000) * 0.001;
                    if (Parent != null)
                        weight = _ParentWeights[index++];

                    current.Connections.Add(new Connection(current, NextLayer.Neurones[j], weight));
                    DNA.AddSegment(weight);
                }
            }
        }

        public void CalculateDelta()
        {
            for (int i = 0; i < Neurones.Length; i++)
                if (NextLayer == null)
                    Neurones[i].CalculateDelta(true, RequestedValues[i], null);
                else Neurones[i].CalculateDelta(false, 0.0, NextLayer);
        }





    }
}
