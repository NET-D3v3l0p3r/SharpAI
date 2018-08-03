using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GANeuroSharp.Converters;
using GANeuroSharp.Data;
using GANeuroSharp.NeuralNetwork.Activation;


namespace GANeuroSharp.NeuralNetwork
{
    [Serializable]
    public class Brain
    {
        public List<Layer> Layers { get; private set; }
        public DNA<double> DNA { get; private set; }
        public bool IsMeshed { get; private set; }

        public Dictionary<double[], double[]> TrainingSet = new Dictionary<double[], double[]>();

        public Brain()
        {
            Layers = new List<Layer>();
            DNA = new DNA<double>(new BinaryConverter());
        }

        public void AddLayer(Layer layer)
        {
            Layers.Add(layer);
        }

        public static Brain FromDNA(Brain parent)
        {
            Brain child = new Brain();

            for (int i = 0; i < parent.Layers.Count; i++)
                child.AddLayer(new Layer(parent.Layers[i].NeuroneCount, parent.Layers[i].ActivationFunction, parent?.DNA.Compontents[i]));

            child.Mesh();

            return child;
        }

        public void Mesh()
        {
            for (int i = 0; i < Layers.Count - 1; i++)
            {
                Layer CURRENT = Layers[i];
                Layer NEXT = Layers[i + 1];

                NEXT.PreviousLayer = CURRENT;
                CURRENT.NextLayer = NEXT;

                CURRENT.MeshWithNextLayer();
                DNA.AddDNA(CURRENT.DNA);
            }

            DNA.AddDNA(Layers[Layers.Count - 1].DNA);

            IsMeshed = true;
        }

        public double[] Push(double[] values)
        {
            for (int i = 0; i < Layers[0].Neurones.Length; i++)
            {
                Layers[0].Neurones[i].Set(values[i]);
                Layers[0].Neurones[i].Fire();
            }

            for (int i = 1; i < Layers.Count; i++)
            {
                for (int j = 0; j < Layers[i].Neurones.Length; j++)
                {
                    Layers[i].Neurones[j].Fire();
                }
            }

            List<double> results = new List<double>();
            for (int i = 0; i < Layers[Layers.Count - 1].Neurones.Length; i++)
                results.Add(Layers[Layers.Count - 1].Neurones[i].Get());



            return results.ToArray();

        }

       

        public void Clear()
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                for (int j = 0; j < Layers[i].Neurones.Length; j++)
                {
                    Layers[i].Neurones[j].Clear();
                }
            }
        }



        public void Backpropagate(double epsilon, int iterations)
        {
            int epoch = 0;

            while (true)
            {

                foreach (var data in TrainingSet)
                {
                    Push(data.Key);
                    Layer.RequestedValues = data.Value;

                    for (int i = Layers.Count - 1; i >= 1; i--)
                    {
                        Layer L1 = Layers[i];
                        Layer L0 = Layers[i - 1];

                        L1.CalculateDelta();

                        double[] deltaWeightsMatrix = new double[L0.Neurones.Length * L1.Neurones.Length];

                        int deltaCounter = 0;
                        for (int j = 0; j < L1.Neurones.Length; j++)
                        {
                            for (int k = 0; k < L0.Neurones.Length; k++)
                            {
                                deltaWeightsMatrix[deltaCounter++] = epsilon * L1.Neurones[j].Delta * L0.Neurones[k].Get();
                            }
                        }

                        deltaCounter = 0;
                        for (int j = 0; j < L0.Neurones.Length; j++)
                        {
                            for (int k = 0; k < L0.Neurones[j].Connections.Count; k++)
                            {
                                L0.Neurones[j].Connections[k].Adjust(deltaWeightsMatrix[deltaCounter++], .8);
                            }
                        }

                        for (int j = 0; j < L1.Neurones.Length; j++)
                            L1.Neurones[j].Adjust(L1.Neurones[j].Delta * epsilon);

                        
                    }

                    Clear();

                }

                if (epoch++ >= iterations)
                    return;


            }
        }





    }
}
