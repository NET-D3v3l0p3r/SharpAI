using GANeuroSharp.Converters;
using GANeuroSharp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GANeuroSharp
{
    public partial class TravellingSalesmanProblem : Form
    {
        public TravellingSalesmanProblem()
        {
            InitializeComponent();
        }

   
        private Dictionary<string, PointF> Cities = new Dictionary<string, PointF>();
        private string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        Random r = new Random();
        char[] array;

        DNA<string>[] Generations = new DNA<string>[150];
        static object sync = new object();
        private void TravellingSalesmanProblem_Load(object sender, EventArgs e)
        {

            array = Letters.ToCharArray();

            var pts = GetCircle(300, 300, 150, 15);
          
            for (int i = 0; i < pts.Length; i++)
                Cities.Add(GetName(), pts[i]);


            for (int i = 0; i < Generations.Length; i++)
            {
                Generations[i] = new DNA<string>(new LinearConverter());

                for (int j = 0; j < Cities.Count; j++)
                {


                    Generations[i].AddSegment(Cities.ElementAt(j).Key);

                }

                Generations[i].Mutate(1, 5, DNA<string>.MutationStrategy.SegmentSwap, 2);

            }

            DoubleBuffered = true;

        }

        private string GetName()
        {
            while (true)
            {
                string name = array[r.Next(0, Letters.Length)] + "" + array[r.Next(0, Letters.Length)];
                if (!Cities.ContainsKey(name))
                    return name;
            }

        }
 

        private PointF[] GetCircle(int x, int y, int r, float delta)
        {
            List<PointF> pts = new List<PointF>();
            for (float i = -r; i < r; i += delta)
            {
                float X = i;
                float Y = (float)Math.Sqrt(1.0 - (X * X) / (r * r)) * r ;

                pts.Add(new PointF(X + x, Y + y));
                pts.Add(new PointF(X + x, -Y + y));
            }

            return pts.ToArray();
        }


        DNA<string> BestCreature;
        DNA<string> SecondBestCreature;

        private void OnUpdate()
        {
            // CALC FITNESS
 

            for (int i = 0; i < Generations.Length; i++)
            {
                for (int j = 0; j < Generations[i].IterationLength - 1; j++)
                {
                    string initials = Generations[i].GetSegmentValue(j);
                    string initials1 = Generations[i].GetSegmentValue(j + 1);

                    PointF a = Cities[initials];
                    PointF b = Cities[initials1];

                    Generations[i].Fitness += Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
                }
 
            }
            
            Generations = Generations.OrderBy(p => p.Fitness).ToArray();

            BestCreature = Generations[0];
            SecondBestCreature = Generations[1];



            for (int i = 0; i < Generations.Length; i++)
            {
                DNA<String> child = DNA<string>.Combine(BestCreature, SecondBestCreature, DNA<string>.CrossoverStrategy.SimplePlusWhatIsLeft, 2);

                double rate = (1.0 / Math.Pow(Generations[Generations.Length - 1].Fitness, 2)) * Math.Pow(BestCreature.Fitness, 2);
                rate *= 0.0045;
                child.Mutate(rate, 10, DNA<string>.MutationStrategy.SegmentSwap, 2);
                Generations[i] = new DNA<string>(new LinearConverter())
                {
                    FullSequence = child.FullSequence,
                    FullLength = BestCreature.FullLength,
                    SegmentLength = BestCreature.SegmentLength,
                    IterationLength = BestCreature.IterationLength,
                };
            }

        }
        ManualResetEvent re = new ManualResetEvent(false);
        private void TravellingSalesmanProblem_Paint(object sender, PaintEventArgs e)
        {
            OnUpdate();
            foreach (var item in Cities)
            {
                e.Graphics.FillRectangle(Brushes.Blue, new RectangleF(item.Value.X, item.Value.Y, 5, 5));
            }

            for (int i = 0; i < BestCreature.IterationLength - 1; i++)
            {
                string initials = Generations[i].GetSegmentValue(i);
                string initials1 = Generations[i].GetSegmentValue(i + 1);

                PointF a = Cities[initials];
                PointF b = Cities[initials1];

                e.Graphics.DrawLine(Pens.Green, a, b);
            }



            this.Invalidate();


        }
    }
}
