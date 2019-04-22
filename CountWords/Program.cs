using CountWords.Parsers;
using System;
using System.Threading.Tasks;

namespace CountWords
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("CountWords utility version 1.0");

            var parameters = ParseCommandLine(args);

            if (parameters != null)
            {
                // 1. The simplest way 
                //await new SimpleParser(parameters).RunAsync();
                
                // 2. Some optimizations
                //await new StreamedParser(parameters).RunAsync();

                // 3. the fastest way
                await new StreamedMultiThreadedParser(parameters).RunAsync();
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
