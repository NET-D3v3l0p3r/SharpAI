using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GANeuroSharp.Converters;
using GANeuroSharp.Helpers;

namespace GANeuroSharp.Data
{
    public class DNA<T>
    {
        public List<DNA<T>> Compontents { get; set; }
        public string FullSequence { get; set; }

        public IConverter<T> Converter { get; set; }

        public int FullLength { get; set; }
        public int SegmentLength { get; set; }

        public int IterationLength { get; set; }


        public double Fitness;

        public DNA(IConverter<T> converter)
        {
            Converter = converter;
            Compontents = new List<DNA<T>>();
        }

        public void AddSegment(T value)
        {
            string _temp = Converter.Encode(value);
            SegmentLength = _temp.Length;
            FullSequence += _temp;
            FullLength += _temp.Length;

            IterationLength = FullSequence.Length / SegmentLength;
        }

        public void AddDNA(DNA<T> value)
        {
            Compontents.Add(value);
            FullSequence += value.FullSequence;
            FullLength += value.FullLength;
            SegmentLength = value.SegmentLength;

            IterationLength = FullSequence.Length / SegmentLength;
        }

        public T[] GetFullSegments(int offSet)
        {
            
            List<T> _Values = new List<T>();
            for (int i = offSet; i < FullLength / SegmentLength; i++)
                _Values.Add(GetSegmentValue(i));

            return _Values.ToArray();
        }

        public T GetSegmentValue(int segmentIndex)
        {
         

            string[] parts = FullSequence.GetParts(SegmentLength);
            return Converter.Decode(parts[segmentIndex]);
        }

        public static DNA<T> Combine(DNA<T> a, DNA<T> b, CrossoverStrategy strategy, int partition)
        {
            string[] seqA = a.FullSequence.GetParts(partition);
            string[] seqB = b.FullSequence.GetParts(partition);

            if (seqA.Length != seqB.Length)
                return null;
            if (!a.Converter.Name.Equals(b.Converter.Name)) return null;

            string[] newDNA = new string[seqA.Length];
            var _currentDNA = seqA;

            switch (strategy)
            {
                case CrossoverStrategy.Duplex:
                    int _CIindex0 = Globals.GlobalRandom.Next(0, seqA.Length);
                    int _CIindex1 = Globals.GlobalRandom.Next(_CIindex0, seqA.Length);


                    for (int i = 0; i < _currentDNA.Length; i++)
                    {
                        if (i >= _CIindex0 && i < _CIindex1)
                            _currentDNA = seqB;
                        else _currentDNA = seqA;

                        newDNA[i] = (_currentDNA[i]);
                    }

                    break;

                case CrossoverStrategy.Simple:
                    int _CIindex = Globals.GlobalRandom.Next(0, seqA.Length);

                    for (int i = 0; i < _currentDNA.Length; i++)
                    {
                        if (i >= _CIindex)
                            _currentDNA = seqB;

                        newDNA[i] = (_currentDNA[i]);
                    }

                    break;

                case CrossoverStrategy.SimplePlusWhatIsLeft:
                    int _CIindex2 = Globals.GlobalRandom.Next(0, seqA.Length);
                    int _CILength = Globals.GlobalRandom.Next(0, seqA.Length - _CIindex2);

                    List<string> _used = new List<string>();

                    for (int i = _CIindex2; i < _CIindex2 + _CILength; i++)
                    {
                        newDNA[i] = seqA[i];
                        _used.Add(newDNA[i]);
                    }

                    int lastIndex = 0;
                    for (int i = 0; i < newDNA.Length; i++)
                    {
                        string _cur = newDNA[i];
                        if (_cur != null) continue;

                        for (int j = lastIndex; j < newDNA.Length; j++)
                        {
                            if (_used.Contains(seqB[j]))
                                continue;

                            newDNA[i] = seqB[j];
                            lastIndex++;
                            _used.Add(seqB[j]);
                            break;
                        }
                        
                    }
                    


                    break;
            }

            string combinedDNA = "";

            for (int i = 0; i < newDNA.Length; i++)
                combinedDNA += newDNA[i];

            DNA<T> _Combined = new DNA<T>(a.Converter)
            {
                FullSequence = combinedDNA,
                FullLength = newDNA.Length * partition,
                SegmentLength = a.SegmentLength
            };

            return _Combined;
        }
        public static DNA<T> CrossOver(DNA<T> a, DNA<T> b, CrossoverStrategy strategy, int partition)
        {
            if (!a.Converter.Equals(b.Converter))
                return null;

            DNA<T> parent = new DNA<T>(a.Converter);
            for (int i = 0; i < a.Compontents.Count; i++)
            {
                DNA<T> _combined = DNA<T>.Combine(a.Compontents[i], b.Compontents[i], strategy, partition);
                parent.AddDNA(_combined);
            }


            return parent;
        }

        public void Mutate(double rate, int amount, MutationStrategy strategy, int partition = 1)
        {
            if (amount == 0)
                return;

            int _Treshold = (int)(FullSequence.Length - FullSequence.Length * rate);
            int _Count = 0;


            switch (strategy)
            {
                case MutationStrategy.BinarySwap:

                    for (int i = 0; i < Compontents.Count; i++)
                    {
                        if (Globals.GlobalRandom.Next(0, FullSequence.Length) < _Treshold)
                            continue;

                        char[] _seq = Compontents[i].FullSequence.ToCharArray();
                        for (int j = 0; j < _seq.Length; j++)
                        {
                            if (Globals.GlobalRandom.Next(0, FullSequence.Length) > _Treshold)
                                continue;

                            _seq[j] = _seq[j] == '0' ? '1' : '0';
                            _Count++;
                        }

                        Compontents[i].FullSequence = new string(_seq);
                    }

                    this.FullSequence = "";

                    for (int i = 0; i < Compontents.Count; i++)
                        FullSequence += Compontents[i].FullSequence;
                    

                    break;


                case MutationStrategy.SegmentSwap:

                    int length = FullSequence.Length;

                    if (Compontents.Count == 0)
                        Compontents.Add(this);

                    for (int i = 0; i < Compontents.Count; i++)
                    {
                        if (Globals.GlobalRandom.Next(0, length) < _Treshold)
                            continue;

                        string[] _seq = Compontents[i].FullSequence.GetParts(partition);
                        for (int j = 0; j < _seq.Length; j++)
                        {
                            if (Globals.GlobalRandom.Next(0, length) < _Treshold)
                                continue;

                            int randomIndex = Globals.GlobalRandom.Next(0, _seq.Length);
                            string oldVal = _seq[randomIndex];
                            _seq[randomIndex] = _seq[j];
                            _seq[j] = oldVal;
                        }

                        Compontents[i].FullSequence = "";
                        for (int k = 0; k < _seq.Length; k++)
                            Compontents[i].FullSequence += _seq[k];

                         
                    }
 


                    break;
            }


            Mutate(rate, amount - 1, strategy, partition);
        }


        public enum CrossoverStrategy
        {
            Simple,
            Duplex,
            SimplePlusWhatIsLeft
        }
        public enum MutationStrategy
        {
            BinarySwap,
            SegmentSwap
        }


    }
}
