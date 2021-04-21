using Exquance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Services.Abstract
{
    public interface IFileReader
    {
        Task<string[]> GetFileLinesAsync(string filePath);
        List<FileLine> MapFileLines(string[] fileLines);
    }
}
