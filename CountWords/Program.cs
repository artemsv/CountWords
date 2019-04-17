﻿using System;
using System.Threading.Tasks;

namespace CountWords
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var parameters = ParseCommandLine(args);

            if (parameters != null)
            {
                await new Parser(parameters).RunAsync();
            }
            else
            {
                Console.WriteLine(@"usage: CountWords <sourceurl> <queriesurl>");
            }
        }

        private static ApplicationParameters ParseCommandLine(string[] args)
        {
            ApplicationParameters res = null;

            if (args.Length == 2)
            {
                res = new ApplicationParameters
                {
                    SourceUrl = args[0],
                    QueryUrl = args[1]
                };
            }

            return res;
        }
    }
}
