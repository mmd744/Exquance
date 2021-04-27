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
        private static readonly object locker = new();
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;

            IExpressionEvaluator _evaluator = new ExpressionEvaluator();
            IValidator _validator = new Validator();
            ICellProcessor _cellService = new CellProcessor();

            Console.WriteLine("Hello, user! Please follow instructions below: \n");

            List<UserInput> inputs = new();

            bool userFinished = false;

            string path = null, formula = null;
            bool consoleOutput = false, fileOutput = false, pathIsValid = false, formulaIsValid = false;
            while (!userFinished)
            {
                while (!pathIsValid || !formulaIsValid)
                {
                    Console.WriteLine("Enter source file path, output parameter (-c for console, -f for file) and formula");
                    Console.WriteLine("Example: D:\\Exquance\\1.txt -c -formula x+25-3");
                    string userInput = Console.ReadLine();
                    if (userInput.Equals("start"))
                    {
                        userFinished = true;
                        break;
                    }
                    try
                    {
                        path = userInput.Substring(0, userInput.IndexOf(".txt") + 4);
                        consoleOutput = userInput.Contains(" -c ");
                        fileOutput = userInput.Contains(" -f ");
                        formula = userInput.Substring(userInput.IndexOf("-formula") + 8).ToLower().RemoveAllWhiteSpaces();

                        pathIsValid = _validator.FilePathIsValid(path, inputs.Select(i => i.Path));
                        formulaIsValid = _validator.FormulaIsValid(formula);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Wrong input");
                        continue;
                    }
                }
                if (userFinished) break;
                UserInput model = new(path, consoleOutput, fileOutput, formula);
                inputs.Add(model);
                Console.WriteLine("\nFile accepted\nType 'start' to finish entering files and start calculation");
                pathIsValid = false; formulaIsValid = false;
            }

            string variable = formula.First(ch => char.IsLetter(ch)).ToString();

            Parallel.ForEach(inputs, async input =>
            {
                List<FileLine> fileLines = new();
                using (var file = new StreamReader(input.Path))
                {
                    int counter = 0;
                    string ln;

                    while ((ln = file.ReadLine()) != null)
                    {
                        counter++;
                        fileLines.Add(new FileLine(counter, ln));
                    }
                }

                Task consoleTask = null, fileTask = null;
                List<Task> actualTasks = new();
                if (input.WriteToConsole)
                {
                    consoleTask = Task.Run(() =>
                    {
                        Parallel.ForEach(fileLines, fl =>
                        {
                            string outputVal = string.Empty;
                            if (!fl.LineVal.Equals("Not a number"))
                            {
                                var calculatedVal = _evaluator.EvaluateExpression(formula.Replace(variable, fl.LineVal));
                                outputVal = calculatedVal.ToString();
                            }
                            Console.WriteLine($"{fl.LineNum}: {fl.LineVal}: {outputVal}");
                        });
                    });
                    actualTasks.Add(consoleTask);
                }
                if (input.WriteToFile)
                {
                    var folder = Path.GetDirectoryName(input.Path);
                    var fileName = Path.GetFileName(input.Path);
                    var outputFilePath = $"{folder}\\Thread{Thread.CurrentThread.ManagedThreadId}-{fileName}";
                    if (File.Exists(outputFilePath))
                        File.Delete(outputFilePath);

                    fileTask = Task.Run(() =>
                    {
                        Parallel.ForEach(fileLines, fl =>
                        {
                            string outputVal = string.Empty;
                            if (!fl.LineVal.Equals("Not a number"))
                            {
                                var calculatedVal = _evaluator.EvaluateExpression(input.Formula.ToLower().Replace(variable, fl.LineVal));
                                outputVal = calculatedVal.ToString();
                            }
                                
                            lock (locker)
                            {
                                using (StreamWriter writer = new(outputFilePath, true)) // true to append data to the file
                                {
                                    writer.WriteLine($"{fl.LineNum}: {fl.LineVal}: {outputVal}");
                                }
                            }
                        });
                    });
                    actualTasks.Add(fileTask);
                }
                await Task.WhenAll(actualTasks);
            });

            Console.ReadKey();
        }
    }
}
