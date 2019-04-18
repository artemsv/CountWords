using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CountWords.Parsers
{
    public class StreamedParser
    {
        private readonly ApplicationParameters _parameters;

        public StreamedParser(ApplicationParameters parameters)
        {
            _parameters = parameters;
        }

        public async Task RunAsync()
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
                    HandleLine3(line, queryLines);
                }
            }

            timeStamp.Stop();

            Console.WriteLine($"Calculation time: {timeStamp.ElapsedMilliseconds}ms");
        }

        public static void HandleLine(string line, string[] queryLines)
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

        public static void HandleLine2(string line, string[] queryLines)
        {
            var words = line.Split(',');

            foreach (var queryLine in queryLines)
            {
                var queryWords = queryLine.Split(',');

                if (!queryWords.Except(words).Any())
                {
                    var rest = words.Except(queryWords);
                    var g = rest.GroupBy(x => x);
                    var wordsCountStr = string.Join(", ", g.Select(x => $"{x.Key}: {x.Count()}"));

                    Console.WriteLine($"{{{wordsCountStr}}}");
                }
            }
        }

        public static void HandleLine3(string line, string[] queryLines)
        {
            var words = line.Split(',');
            var dic = new Dictionary<string, int>();

            foreach (var queryLine in queryLines)
            {
                var queryWords = queryLine.Split(',');

                if (!queryWords.Except(words).Any())
                {
                    dic.Clear();

                    foreach (var word in words)
                    {
                        if (!queryWords.Contains(word))
                        {
                            if (dic.ContainsKey(word))
                                dic[word]++;
                            else
                                dic[word] = 1;
                        }
                    }

                    var wordsCountStr = string.Join(", ", dic.Select(x => $"{x.Key}: {x.Value}"));

                    Console.WriteLine($"{{{wordsCountStr}}}");
                }
            }
        }

        public static void HandleLine4(string line, string[] queryLines)
        {
            var words = line.Split(',');
            var dic = new Dictionary<string, int>();

            foreach (var queryLine in queryLines)
            {
                var queryWords = queryLine.Split(',');

                if (!queryWords.Except(words).Any())
                {
                    dic.Clear();

                    foreach (var word in words)
                    {
                        if (!queryWords.Contains(word))
                        {
                            if (dic.ContainsKey(word))
                                dic[word]++;
                            else
                                dic[word] = 1;
                        }
                    }

                    var wordsCountStr = string.Join(", ", dic.Select(x => $"{x.Key}: {x.Value}"));

                    Console.WriteLine($"{{{wordsCountStr}}}");
                }
            }
        }
    }
}