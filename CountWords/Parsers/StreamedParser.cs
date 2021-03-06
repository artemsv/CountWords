﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CountWords.LineHandlers;

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
            var httpClient = new HttpClient();

            var timeStamp = Stopwatch.StartNew();

            var queries = await httpClient.GetStringAsync(_parameters.QueryUrl);
            var stream = await httpClient.GetStreamAsync(_parameters.SourceUrl);

            var queryLines = queries.Split('\n');
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    DictionaryBased2LineHandler.Handle(line, queryLines);
                }
            }

            timeStamp.Stop();

            Console.WriteLine($"Calculation time: {timeStamp.ElapsedMilliseconds}ms");
        }
    }
}