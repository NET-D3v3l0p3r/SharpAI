using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GANeuroSharp.Converters;
using GANeuroSharp.Data;
using GANeuroSharp.NeuralNetwork;
using GANeuroSharp.NeuralNetwork.Activation;

namespace GANeuroSharp
{
    public class MainClass
    {
        public static void Main(string[] param)
        {

            //TravellingSalesmanProblem tsp = new TravellingSalesmanProblem();
            //Application.Run(tsp);

            Brain br = new Brain();

            br.AddLayer(new Layer(2, new Linear()));

            br.AddLayer(new Layer(5, new Tanh()));
       

            br.AddLayer(new Layer(1, new Tanh()));

            br.Mesh();


            br.TrainingSet.Add(new double[] { 0, 0 }, new double[] { 1 });
            br.TrainingSet.Add(new double[] { 0, 1 }, new double[] { 0 });
            br.TrainingSet.Add(new double[] { 1, 1 }, new double[] { 1 });
            br.TrainingSet.Add(new double[] { 1, 0 }, new double[] { 0 });


            br.Backpropagate(0.1, 150000);




            Console.WriteLine("{0, 0} " + Math.Round(br.Push(new double[] { 0, 0 })[0], 2));
            br.Clear();

            Console.WriteLine("{1, 1} " + Math.Round(br.Push(new double[] { 1, 1 })[0], 2));
            br.Clear();

            Console.WriteLine("{0, 1} " + Math.Round(br.Push(new double[] { 0, 1 })[0], 2));
            br.Clear();

            Console.WriteLine("{1, 0} " + Math.Round(br.Push(new double[] { 1, 0 })[0], 2));
            br.Clear();


            Console.Read();


        }
    }
}
