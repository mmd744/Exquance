using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Services
{
    public abstract class BaseValidator
    {
        internal bool ParamIsValid(string input, string paramName, out string errorMessage)
        {
            if (string.IsNullOrEmpty(input))
            {
                errorMessage = $"{paramName} is empty";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
    }
}
