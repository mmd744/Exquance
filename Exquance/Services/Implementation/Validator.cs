using Exquance.Extensions;
using Exquance.Services.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Security;
using System.Security.AccessControl;

namespace Exquance.Services.Implementation
{
    public class Validator : BaseValidator, IValidator
    {

        public bool FileLinesAreValid(string[] fileLines)
        {
            bool result = true;
            Parallel.ForEach(fileLines, fl =>
            {
                if (!int.TryParse(fl.RemoveAllWhiteSpaces(), out int num))
                {
                    result = false;
                }
            });

            return result;
        }

        public bool FilePathIsValid(string filePath, IEnumerable<string> alreadyStoredPaths)
        {
            if (!base.ParamIsValid(filePath, "File path", out string errMsg))
            {
                Console.WriteLine(errMsg);
                return false;
            }
            else if (!IsAccessible(filePath))
            {
                Console.WriteLine("Not enough permissions for accessing this file");
                return false;
            }
            else if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found");
                return false;
            }
            else if (alreadyStoredPaths.Contains(filePath))
            {
                Console.WriteLine("You already added this file");
                return false;
            }
            return true;
        }

        public bool FormulaIsValid(string formula)
        {
            if (!base.ParamIsValid(formula, "Formula", out string errMsg))
            {
                Console.WriteLine(errMsg);
                return false;
            }
            else if (formula.Length < 3)
            {
                Console.WriteLine("Wrong formula (too short)");
                return false;
            }
            else if (formula.ToLower().Contains("/0"))
            {
                Console.WriteLine("Wrong formula (divzero)");
                return false;
            }
            else if (!formula.Any(ch => char.IsLetter(ch)))
            {
                Console.WriteLine("Wrong formula (no variables)");
                return false;
            }

            try
            {
                int digitsCount = 0;
                string finalExpression = formula.Replace(formula.First(c => char.IsLetter(c)), '1') + '|'; // -3-44-25+2*3/3|
                for (int i = 0; i < finalExpression.Length; i++)
                {
                    if (finalExpression[i].IsValidAction())
                    {
                        int value = int.Parse(finalExpression.Substring(i - digitsCount, digitsCount));
                        digitsCount = 0;
                    }
                    else
                    {
                        digitsCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Value was either too large or too small for an Int32."))
                    Console.WriteLine("Too big numbers found in formula");
                else
                    Console.WriteLine("Wrong formula");

                return false;
            }

            return true;
        }

        private bool IsAccessible(string path)
        {
            DirectoryInfo dirInfo = new(path);
            try
            {
#pragma warning disable CA1416 // Validate platform compatibility
                DirectorySecurity dirAC = dirInfo.GetAccessControl(AccessControlSections.Access);
#pragma warning restore CA1416 // Validate platform compatibility
                return true;
            }
            catch (PrivilegeNotHeldException)
            {
                return false;
            }
        }

        public bool OutParameterIsValid(string outParameter)
        {
            if (!base.ParamIsValid(outParameter, "Out parameter", out string errMsg))
            {
                Console.WriteLine(errMsg);
                return false;
            }
            else if (!outParameter.Equals("-c") && !outParameter.Equals("-f"))
            {
                Console.WriteLine("Wrong our parameter");
                return false;
            }
            return true;
        }
    }
}
