using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Models
{
    public class UserInput
    {
        public UserInput(string path, bool console, bool file, string formula)
        {
            Path = path;
            WriteToConsole = console;
            WriteToFile = file;
            Formula = formula;
        }
        public string Path { get; }
        public bool WriteToConsole { get; }
        public bool WriteToFile { get; }
        public string Formula { get; }
    }
}
