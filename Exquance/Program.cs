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
            IFileService _fileService = new FileService();
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

                var fileLines = await File.ReadAllLinesAsync(filePath);
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

                List<FileLine> lines = _fileService.MapFileLines(fileLines);
                Parallel.ForEach(lines, l => l.CalculatedValue = _evaluator.EvaluateExpression(formula.ToLower().Replace("x", l.Value.ToString())));

                if (outParameter.ToLower().Equals("-f"))
                {
                    await _fileService.WriteLinesToFileAsync(filePath, lines);
                    Console.WriteLine("File with results written to source directory");
                }
                else
                {
                    foreach (var line in lines)
                    {
                        Console.WriteLine($"{line.LineNumber}: {line.Value}: {line.CalculatedValue}");
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
