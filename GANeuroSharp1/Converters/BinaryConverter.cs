using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GANeuroSharp.Converters
{
    public class BinaryConverter : IConverter<double>
    {
        public string Name { get { return "BinaryConverter"; } }

        public double Decode(string input)
        {
            char[] reversedArray = input.ToCharArray();
            bool signed = reversedArray[0] == '0';

            Array.Reverse(reversedArray);

            double result = .0d;
            for (int i = 0; i < reversedArray.Length - 1; i++)
            {
                int num = int.Parse(reversedArray[i] + "");
                result += num * Math.Pow(2, i);
            }

            result = signed ? result * -1.0 : result;

            return result * 0.001;
        }

        public string Encode(double input)
        {
            input /= 0.001;
            string fullBin = "";

            int num = (int)input;
            for (int i = 0; i < 10; i++)
            {
                if (num % 2 == 0)
                    fullBin += "0";
                else fullBin += '1';

                num /= 2;
            }

            if (input < 0)
                fullBin += "0";
            else fullBin += "1";

            char[] reversedArray = fullBin.ToCharArray();
            Array.Reverse(reversedArray);

            return new string(reversedArray);
        }
    }
}
