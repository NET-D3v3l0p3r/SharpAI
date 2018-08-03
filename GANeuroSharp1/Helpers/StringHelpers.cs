using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GANeuroSharp.Helpers
{
    public static class StringHelpers
    {
        public static string[] GetParts(this string input, int length)
        {
            List<string> _Parts = new List<string>();
            int _OffSet = 0;

            for (int i = 0; i < input.Length; i += length)
            {
                if (i + length >= input.Length)
                    _Parts.Add(input.Substring(_OffSet, input.Length - _OffSet));
                else _Parts.Add(input.Substring(_OffSet, length));

                _OffSet += length;
            }

            return _Parts.ToArray();

        }
    }
}
