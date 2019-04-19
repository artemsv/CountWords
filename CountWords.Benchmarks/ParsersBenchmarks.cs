using BenchmarkDotNet.Attributes;
using CountWords.LineHandlers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

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
        public void HashSetBasedLineHandlerTest()
        {
            foreach (var line in _lines)
            {
                HashSetBasedLineHandler.Handle(line, _queries);
            }
        }

        [Benchmark]
        public void ExceptBasedLineHandlerTest()
        {
            foreach (var line in _lines)
            {
                ExceptBasedLineHandler.Handle(line, _queries);
            }
        }

        [Benchmark]
        public void DictionaryBasedLineHandlerTest()
        {
            foreach (var line in _lines)
            {
                DictionaryBasedLineHandler.Handle(line, _queries);
            }
        }
    }
}
