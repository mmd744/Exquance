﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exquance
{
    public static class Interactions
    {
        public static void GreetUser() => Console.WriteLine("Hello, user! Please follow instructions below: \n");

        public static void AskForSrcFilePath() => Console.WriteLine("Paste source file path: (or type 'start' if you're done with files)");
        public static void AskForOutParameter() => Console.WriteLine("Choose out parameter: ('-f' for file / '-c' for console)");
        public static void AskForFormula() => Console.WriteLine("Type out formula (example: x + 1 - 20): ");

        public static void AnnounceSuccess() => Console.WriteLine("File(s) with results successfully written to source directory");
    }
}
