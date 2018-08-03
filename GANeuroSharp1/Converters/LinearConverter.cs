using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GANeuroSharp.Converters
{
    public class LinearConverter : IConverter<string>
    {
        public string Name { get { return "LinearConverter"; } }
        public string Decode(string input)
        {
            return input;
        }

        public string Encode(string input)
        {
            return input;
        }
    }
}
