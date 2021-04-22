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
            Console.ForegroundColor = ConsoleColor.Blue;

            IExpressionEvaluator _evaluator = new ExpressionEvaluator();
            IFileService _fileService = new FileService();
            IValidator _validator = new Validator();
            ICellProcessor _cellService = new CellProcessor();

            Interactions.GreetUser();
            Interactions.AskForFilesCount();
            string filesCount = Console.ReadLine();
            int filesCountNum;
            while (!_validator.FilesCountIsValid(filesCount, out filesCountNum))
            {
                Interactions.AskForFilesCount();
                filesCount = Console.ReadLine();
            }

            for (int i = 0; i < filesCountNum; i++)
            {
                string filePath, outParameter, formula;

                Interactions.AskForSrcFilePath();
                filePath = Console.ReadLine();
                while (!_validator.FilePathIsValid(filePath))
                {
                    Interactions.AskForSrcFilePath();
                    filePath = Console.ReadLine();
                }

                var fileLines = await File.ReadAllLinesAsync(filePath);
                if (!_validator.FileLinesAreValid(fileLines)) return;

                Interactions.AskForOutParameter();
                outParameter = Console.ReadLine();
                while (!_validator.OutParameterIsValid(outParameter))
                {
                    Interactions.AskForOutParameter();
                    outParameter = Console.ReadLine();
                }

                Interactions.AskForFormula();
                formula = Console.ReadLine();
                while (!_validator.FormulaIsValid(formula))
                {
                    Interactions.AskForFormula();
                    formula = Console.ReadLine();
                }

                List<FileLine> lines = _fileService.MapFileLines(fileLines);
                await Task.Run(() => // cpu-bound operation, better to perform in a separate thread
                {
                    Parallel.ForEach(lines, l => l.CalculatedValue = _evaluator.EvaluateExpression(formula.ToLower().Replace("x", l.Value.ToString())));
                });

                if (outParameter.ToLower().Equals("-f"))
                {
                    await _fileService.WriteLinesToFileAsync(filePath, lines); // I/O-bound operation, better to await without Task.Run()
                    Interactions.AnnounceSuccess();
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
