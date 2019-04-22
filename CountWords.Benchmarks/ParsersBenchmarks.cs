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
        private readonly string[] _lines;

        public ParsersBenchmarks()
        {
            //  the files should be downloaded at first
            var lines = File.ReadLines(@"c:\Users\a.sokolov\Downloads\records.txt").ToArray();
            _queries = File.ReadLines(@"c:\Users\a.sokolov\Downloads\queries.txt").ToArray();

            _lines = lines.Take(10000).ToArray();
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
