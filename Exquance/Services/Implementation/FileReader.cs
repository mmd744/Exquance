using Exquance.Extensions;
using Exquance.Models;
using Exquance.Services.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance.Services.Implementation
{
    public class FileReader : IFileReader
    {
        public async Task<string[]> GetFileLinesAsync(string filePath)
        {
            return await File.ReadAllLinesAsync(filePath);
        }

        public List<FileLine> MapFileLines(string[] fileLines)
        {
            int index = 0;
            List<FileLine> lines = new();
            foreach (var line in fileLines)
            {
                index++;
                var newFileLine = new FileLine(line, index);
                lines.Add(newFileLine);
            }
            return lines;
        }
    }
}
