using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using CountWords.Parsers;

namespace CountWords.Benchmarks
{
    [MemoryDiagnoser]
    public class ParsersBenchmarks
    {
        private readonly string[] _queries;
        private IEnumerable<string> _lines;

        public ParsersBenchmarks()
        {
            var lines = File.ReadLines(@"c:\Users\a.sokolov\Downloads\records.txt");

            var httpClient = new HttpClient();
            _queries = httpClient.GetStringAsync("https://s3.amazonaws.com/idt-code-challenge/queries.txt")
                .GetAwaiter().GetResult().Split('\n');

            _lines = lines.Take(10000).ToList();
        }

        [Benchmark]
        public void HandleLineTest()
        {
            foreach (var line in _lines)
            {
                StreamedParser.HandleLine(line, _queries);
            }
        }

        [Benchmark]
        public void HandleLine2Test()
        {
            foreach (var line in _lines)
            {
                StreamedParser.HandleLine2(line, _queries);
            }
        }

        [Benchmark]
        public void HandleLine3Test()
        {
            foreach (var line in _lines)
            {
                StreamedParser.HandleLine3(line, _queries);
            }
        }

        [Benchmark]
        public void HandleLine4Test()
        {
            foreach (var line in _lines)
            {
                StreamedParser.HandleLine4(line, _queries);
            }
        }
    }
}
