using Exquance.Extensions;
using Exquance.Services.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Services.Implementation
{
    public class Validator : BaseValidator, IValidator
    {
        public bool FileLinesAreValid(string[] fileLines)
        {
            bool result = true;
            try
            {
                foreach (var fl in fileLines)
                {
                    int.Parse(fl.RemoveAllWhiteSpaces());
                }
                //Parallel.ForEach(fileLines, fl => int.Parse(fl.RemoveAllWhiteSpaces()));
            }
            catch (FormatException)
            {
                Console.WriteLine("Not a number");
                result = false;
            }
            return result;
        }

        public bool FilePathIsValid(string filePath)
        {
            bool result = true;
            if (!base.ParamIsValid(filePath, "File path", out string errMsg))
            {
                Console.WriteLine(errMsg);
                result = false;
            }
            else if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found");
                result = false;
            }
            return result;
        }

        public bool FilesCountIsValid(string filesCount, out int count)
        {
            bool result = true;
            if (!base.ParamIsValid(filesCount, "Files count", out string errMsg))
            {
                Console.WriteLine(errMsg);
                result = false;
                count = 0;
            }
            else if (!int.TryParse(filesCount, out count))
            {
                Console.WriteLine("Not a number");
                result = false;
            }
            return result;
        }

        public bool FormulaIsValid(string formula)
        {
            bool result = true;
            if (!base.ParamIsValid(formula, "Formula", out string errMsg))
            {
                Console.WriteLine(errMsg);
                result = false;
            }
            else if (!formula.ToLower().Contains("x"))
            {
                Console.WriteLine("Wrong formula");
                result = false;
            }
            return result;
        }

        public bool OutParameterIsValid(string outParameter)
        {
            bool result = true;
            if (!base.ParamIsValid(outParameter, "Out parameter", out string errMsg))
            {
                Console.WriteLine(errMsg);
                result = false;
            }
            else if (!outParameter.Equals("-c") && !outParameter.Equals("-f"))
            {
                Console.WriteLine("Wrong our parameter");
                result = false;
            }
            return result;
        }
    }
}
