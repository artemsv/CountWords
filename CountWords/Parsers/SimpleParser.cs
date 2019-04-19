using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CountWords.LineHandlers;

namespace CountWords.Parsers
{
    internal class SimpleParser
    {
        private ApplicationParameters _parameters;

        public SimpleParser(ApplicationParameters parameters)
        {
            _parameters = parameters;
        }

        internal async Task RunAsync()
        {
            var httpClient = new HttpClient();

            var timeStamp = Stopwatch.StartNew();

            var queries = await httpClient.GetStringAsync(_parameters.QueryUrl);
            var content = await httpClient.GetStringAsync(_parameters.SourceUrl);
            //var content = await File.ReadAllTextAsync(@"c:\Users\a.sokolov\Downloads\records.txt");
            
            timeStamp.Stop();
            Console.WriteLine($"Download time: {timeStamp.ElapsedMilliseconds}ms");

            timeStamp.Restart();
            var queryLines = queries.Split('\n');
            var lines = content.Split('\n');
            timeStamp.Stop();
            Console.WriteLine($"Split time: {timeStamp.ElapsedMilliseconds}ms");

            timeStamp.Restart();

            foreach (var line in lines)
            {
                ExceptBasedLineHandler.Handle(line, queryLines);
            }

            timeStamp.Stop();

            Console.WriteLine($"Calculation time: {timeStamp.ElapsedMilliseconds}ms");
            //Console.ReadLine();
        }
    }
}
