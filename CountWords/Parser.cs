using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CountWords
{
    internal class Parser
    {
        private ApplicationParameters _parameters;

        public Parser(ApplicationParameters parameters)
        {
            _parameters = parameters;
        }

        internal async Task RunAsync()
        {
            Console.WriteLine();
            Console.WriteLine("#######################");

            var httpClient = new HttpClient();

            var timeStamp = Stopwatch.StartNew();

            var queries = await httpClient.GetStringAsync(_parameters.QueryUrl);
            var stream = await httpClient.GetStreamAsync(_parameters.SourceUrl);
            //var content = await File.ReadAllTextAsync(@"c:\Users\a.sokolov\Downloads\records.txt");

            var queryLines = queries.Split('\n');
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {

                    var line = reader.ReadLine();
                    HandleLine(line, queryLines);
                }
            }

            //timeStamp.Stop();
            //Console.WriteLine($"Download time: {timeStamp.ElapsedMilliseconds}ms");

            //timeStamp.Restart();
            //var queryLines = queries.Split('\n');
            //var lines = content.Split('\n');
            //timeStamp.Stop();
            //Console.WriteLine($"Split time: {timeStamp.ElapsedMilliseconds}ms");

            //timeStamp.Restart();

            //foreach (var line in lines)
            //{

            //}

            timeStamp.Stop();

            Console.WriteLine($"Calculation time: {timeStamp.ElapsedMilliseconds}ms");
            //Console.ReadLine();
        }

        private static void HandleLine(string line, string[] queryLines)
        {
            var words = line.Split(',');

            foreach (var queryLine in queryLines)
            {
                var queryWords = queryLine.Split(',');

                var recordSet = new HashSet<string>(words);

                if (recordSet.IsSupersetOf(queryWords))
                {
                    recordSet.ExceptWith(queryWords);
                    var g = recordSet.GroupBy(x => x);
                    var wordsCountStr = string.Join(", ", g.Select(x => $"{x.Key}: {x.Count()}"));

                    Console.WriteLine($"{{{wordsCountStr}}}");
                }
            }
        }
    }
}