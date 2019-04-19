using CountWords.LineHandlers;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CountWords.Parsers
{
    public class Block
    {
        public const int BlockSize = 10008;
        public string[] Buffer { get; set; }
        public int Counter { get; set; }

        public Block()
        {
            Buffer = new string[BlockSize];
        }

        internal void Add(string v)
        {
            Buffer[Counter++] = v;
        }
    }

    public class StreamedMultiThreadedParser
    {
        private readonly ApplicationParameters _parameters;
        private readonly BlockingCollection<Block> _queue;

        public StreamedMultiThreadedParser(ApplicationParameters parameters)
        {
            _parameters = parameters;
            _queue = new BlockingCollection<Block>();
        }

        public async Task RunAsync()
        {
            var httpClient = new HttpClient();
            var timeStamp = Stopwatch.StartNew();

            var queries = await httpClient.GetStringAsync(_parameters.QueryUrl);
            var stream = await httpClient.GetStreamAsync(_parameters.SourceUrl);
            //var stream = File.OpenRead(@"c:\Users\a.sokolov\Downloads\records.txt");

            var queryLines = queries.Split('\n');

            var consumerTasks = PrepareConsumers(queryLines);
            var block = new Block();

            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    block.Add(reader.ReadLine());

                    if (block.Counter == Block.BlockSize)
                    {
                        _queue.Add(block);

                        block = new Block();
                    }
                }
            }

            if (block.Counter > 0)
            {
                _queue.Add(block);
            }

            _queue.CompleteAdding();

            Task.WaitAll(consumerTasks);

            timeStamp.Stop();

            Console.WriteLine($"Working time: {timeStamp.ElapsedMilliseconds}ms");
        }

        private Task[] PrepareConsumers(string[] queryLines)
        {
            return Enumerable.Range(1, Environment.ProcessorCount).Select(w => Task.Factory.StartNew(x =>
            {
                foreach (var block in _queue.GetConsumingEnumerable())
                {
                    for (var k = 0; k < block.Counter; k++)
                    {
                        ExceptBasedLineHandler.Handle(block.Buffer[k], queryLines);
                    }
                }

            }, CancellationToken.None, TaskCreationOptions.LongRunning)).ToArray();
        }
    }
}