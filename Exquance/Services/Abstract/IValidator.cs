using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Services.Abstract
{
    public interface IValidator
    {
        bool FilePathIsValid(string filePath, IEnumerable<string> inputs);
        bool FormulaIsValid(string formula);
    }
}
