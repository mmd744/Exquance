using Exquance.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Models
{
    public class FileLine
    {
        public FileLine(int lineNum, string lineVal)
        {
            LineNum = lineNum;
            if (int.TryParse(lineVal.RemoveAllWhiteSpaces(), out int actualValue))
                LineVal = lineVal;
            else
                LineVal = "Not a number";
        }
        public int LineNum { get; }
        public string LineVal { get; }
    }
}
