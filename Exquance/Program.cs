using Exquance.Models;
using Exquance.Services.Abstract;
using Exquance.Services.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exquance
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IFormulaEvaluator _evaluator = new FormulaEvaluator();
            IFileReader _fileReader = new FileReader();
            IValidator _validator = new Validator();

            Console.WriteLine("Hello, user! Please follow instructions below: \n");
            Console.WriteLine("How many files do you want to process?");
            string filesCount = Console.ReadLine();
            int filesCountNum;
            while (!_validator.FilesCountIsValid(filesCount, out filesCountNum))
            {
                Console.WriteLine("How many files do you want to process?");
                filesCount = Console.ReadLine();
            }

            for (int i = 0; i < filesCountNum; i++)
            {
                string filePath, outParameter, formula;

                Console.WriteLine("Paste source file path: ");
                filePath = Console.ReadLine();
                while (!_validator.FilePathIsValid(filePath))
                {
                    Console.WriteLine("Paste source file path: ");
                    filePath = Console.ReadLine();
                }

                var fileLines = await _fileReader.GetFileLinesAsync(filePath);
                if (!_validator.FileLinesAreValid(fileLines)) return;

                Console.WriteLine("Choose out parameter: ('-f' for file / '-c' for console)");
                outParameter = Console.ReadLine();
                while (!_validator.OutParameterIsValid(outParameter))
                {
                    Console.WriteLine("Choose out parameter: ('-f' for file / '-c' for console)");
                    outParameter = Console.ReadLine();
                }

                Console.WriteLine("Type out formula (example: x + 1 - 20): ");
                formula = Console.ReadLine();
                while (!_validator.FormulaIsValid(formula))
                {
                    Console.WriteLine("Type out formula (example: x + 1 - 20): ");
                    formula = Console.ReadLine();
                }

                List<FileLine> lines = _fileReader.MapFileLines(fileLines);

                if (outParameter.ToLower().Equals("-f"))
                {
                    List<string> outputFileLines = new();
                    foreach (var line in lines)
                    {
                        outputFileLines.Add($"{line.LineNumber}: {line.Value}: " +
                            $"{_evaluator.EvaluateExpression(formula.ToLower().Replace("x", line.Value.ToString()))}");
                    }
                    await File.WriteAllLinesAsync(
                        path: $"{Path.GetDirectoryName(filePath)}\\Thread{Thread.CurrentThread.ManagedThreadId}-{Path.GetFileName(filePath)}",
                        contents: outputFileLines);
                }
                else
                {
                    foreach (var line in lines)
                    {
                        Console.WriteLine($"{line.LineNumber}: {line.Value}: " +
                            $"{_evaluator.EvaluateExpression(formula.ToLower().Replace("x", line.Value.ToString()))}");
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
