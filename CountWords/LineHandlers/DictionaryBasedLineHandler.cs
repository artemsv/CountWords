using System;
using System.Collections.Generic;
using System.Linq;

namespace CountWords.LineHandlers
{
    public static class DictionaryBasedLineHandler
    {
        public static void Handle(string line, string[] queryLines)
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
