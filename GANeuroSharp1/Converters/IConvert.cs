using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GANeuroSharp.Converters
{
    public interface IConverter<T>
    {
        string Name { get;}

        string Encode(T input);
        T Decode(string input);
    }
}
