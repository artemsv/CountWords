using System;
using System.Collections.Generic;
using System.Linq;

namespace CountWords.LineHandlers
{
    public static class DictionaryBased2LineHandler
    {
        public static void Handle(string line, string[] queryLines)
        {
            var words = line.Split(',');
            var dic = new Dictionary<string, (int, int)>();

            for (var k = 0; k < words.Length; k++)
            {
                if (dic.ContainsKey(words[k]))
                {
                    dic[words[k]] = (dic[words[k]].Item1 + 1, dic.Count);
                }
                else
                    dic[words[k]] = (1, dic.Count);
            }

            foreach (var queryLine in queryLines)
            {
                var indexes = new bool[dic.Count];
                var queryWords = queryLine.Split(',');
                var exists = 0;
                for (int k = 0; k < queryWords.Length; k++)
                {
                    dic.TryGetValue(queryWords[k], out var item);
                    
                    if (!item.Equals(default))
                    {
                        indexes[item.Item2] = true;
                        exists++;
                    }
                }

                if (exists == queryWords.Length)
                {
                    var wordsCountStr = string.Join(", ", dic.Where(x => !indexes[x.Value.Item2]).Select(x => $"{x.Key}: {x.Value.Item1}"));

                    Console.WriteLine($"{{{wordsCountStr}}}");
                }
            }
        }
    }
}
