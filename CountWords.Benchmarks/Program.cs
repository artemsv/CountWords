using System;
using BenchmarkDotNet.Running;

namespace CountWords.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ParsersBenchmarks>();
        }
    }
}
