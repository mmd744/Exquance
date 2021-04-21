using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Models
{
    public class FileLine
    {
        public FileLine(string value, int index)
        {
            Value = int.Parse(value);
            LineNumber = index;
        }
        public int LineNumber { get; set; }
        public int Value { get; set; }
    }
}
