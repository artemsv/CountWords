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
        private readonly IEnumerable<string> _lines;

        public ParsersBenchmarks()
        {
            //  the file should be downloaded at first
            var lines = File.ReadLines(@"c:\Users\a.sokolov\Downloads\records.txt");

            var httpClient = new HttpClient();
            _queries = httpClient.GetStringAsync("https://s3.amazonaws.com/idt-code-challenge/queries.txt")
                .GetAwaiter().GetResult().Split('\n');

            _lines = lines.Take(10000).ToList();
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

        [Benchmark]
        public void DictionaryBased2LineHandlerTest()
        {
            foreach (var line in _lines)
            {
                DictionaryBased2LineHandler.Handle(line, _queries);
            }
        }
    }
}
