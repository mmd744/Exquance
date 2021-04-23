using Exquance.Extensions;
using Exquance.Models;
using Exquance.Services.Abstract;
using Exquance.Services.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            IValidator _validator = new Validator();
            ICellProcessor _cellService = new CellProcessor();

            Interactions.GreetUser();

            List<UserInput> inputs = new();

            bool userFinished = false;
            while (!userFinished)
            {
                string path = null, outputParam = null, formula = null;
                bool pathIsValid = false;
                while (!pathIsValid)
                {
                    Interactions.AskForSrcFilePath();
                    string input = Console.ReadLine();

                    if (input.Equals("start"))
                    {
                        if (!inputs.Any())
                        {
                            Console.WriteLine("At least one file is required for proceeding");
                        }
                        else
                        {
                            userFinished = true;
                            break;
                        }
                    }
                    else
                    {
                        if (_validator.FilePathIsValid(input, inputs.Select(i => i.Path)))
                        {
                            path = input;
                            pathIsValid = true;
                        }
                    }
                }
                if (userFinished) break;

                bool outputParamIsValid = false;
                while (!outputParamIsValid)
                {
                    Interactions.AskForOutParameter();
                    string input = Console.ReadLine();
                    if (_validator.OutParameterIsValid(input))
                    {
                        outputParam = input;
                        outputParamIsValid = true;
                    }
                }
                bool formulaIsValid = false;
                while (!formulaIsValid)
                {
                    Interactions.AskForFormula();
                    string input = Console.ReadLine().RemoveAllWhiteSpaces();
                    if (_validator.FormulaIsValid(input))
                    {
                        formula = input;
                        formulaIsValid = true;
                    }
                }
                if (pathIsValid && outputParamIsValid && formulaIsValid)
                {
                    inputs.Add(new UserInput
                    {
                        Path = path,
                        Formula = formula,
                        OutputParam = outputParam
                    });
                    Console.WriteLine("\nFile accepted\n");
                }
            }

            IEnumerable<UserInput> consoleParamInputs = inputs.Where(i => i.OutputParam.RemoveAllWhiteSpaces().ToLower().Equals("-c"));
            IEnumerable<UserInput> fileParamInputs = inputs.Except(consoleParamInputs);

            if (fileParamInputs.Any())
            {
                await Task.Run(() =>
                {
                    Parallel.ForEach(fileParamInputs, async input =>
                    {
                        var folder = Path.GetDirectoryName(input.Path);
                        var fileName = Path.GetFileName(input.Path);
                        var fileLines = await File.ReadAllLinesAsync(input.Path);
                        if (!_validator.FileLinesAreValid(fileLines))
                        {
                            Console.WriteLine($"{fileName} is not a valid file");
                            return;
                        }

                        var outputFilePath = $"{folder}\\Thread{Thread.CurrentThread.ManagedThreadId}-{fileName}";
                        if (File.Exists(outputFilePath))
                            File.Delete(outputFilePath);

                        using (StreamWriter writer = new(outputFilePath, true)) // true to append data to the file
                        {
                            int lineNum = 0;
                            foreach (var line in fileLines)
                            {
                                lineNum++;
                                var lineVal = int.Parse(line.RemoveAllWhiteSpaces());
                                var variable = input.Formula.ToLower().First(ch => char.IsLetter(ch)).ToString();
                                var calculatedVal = _evaluator.EvaluateExpression(input.Formula.ToLower().Replace(variable, lineVal.ToString()));
                                await writer.WriteLineAsync($"{lineNum}: {lineVal}: {calculatedVal}");
                            }
                        }
                    });
                    Interactions.AnnounceSuccess();
                });
            }
            
            if (consoleParamInputs.Any())
            {
                await Task.Run(async () =>
                {
                    foreach (var input in consoleParamInputs)
                    {
                        var fileName = Path.GetFileName(input.Path);
                        var fileLines = await File.ReadAllLinesAsync(input.Path);
                        if (!_validator.FileLinesAreValid(fileLines))
                        {
                            Console.WriteLine($"{fileName} is not a valid file");
                            continue;
                        }

                        int lineNum = 0;
                        Console.WriteLine($"\n{fileName}:\n");
                        foreach (var line in fileLines)
                        {
                            lineNum++;
                            var lineVal = int.Parse(line.RemoveAllWhiteSpaces());
                            var variable = input.Formula.ToLower().First(ch => char.IsLetter(ch)).ToString();
                            var calculatedVal = _evaluator.EvaluateExpression(input.Formula.ToLower().Replace(variable, lineVal.ToString()));
                            Console.WriteLine($"{lineNum}: {lineVal}: {calculatedVal}");
                        }
                    }
                });
            }

            Console.ReadKey();
        }
    }
}
