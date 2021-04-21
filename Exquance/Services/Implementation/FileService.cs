using Exquance.Models;
using Exquance.Services.Abstract;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Exquance.Services.Implementation
{
    public class FileService : IFileService
    {
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

        public async Task WriteLinesToFileAsync(string filePath, List<FileLine> lines)
        {
            List<string> outputFileLines = new();
            foreach (var line in lines)
            {
                outputFileLines.Add($"{line.LineNumber}: {line.Value}: {line.CalculatedValue}");
            }
            await File.WriteAllLinesAsync(
                path: $"{Path.GetDirectoryName(filePath)}\\Thread{Thread.CurrentThread.ManagedThreadId}-{Path.GetFileName(filePath)}",
                contents: outputFileLines);
        }
    }
}
