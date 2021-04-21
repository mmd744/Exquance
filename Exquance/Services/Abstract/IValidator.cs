using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Services.Abstract
{
    public interface IValidator
    {
        bool FilesCountIsValid(string filesCount, out int count);
        bool FilePathIsValid(string filePath);
        bool FileLinesAreValid(string[] fileLines);
        bool OutParameterIsValid(string outParameter);
        bool FormulaIsValid(string formula);
    }
}
