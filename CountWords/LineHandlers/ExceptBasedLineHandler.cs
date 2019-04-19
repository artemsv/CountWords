using System;
using System.Linq;

namespace CountWords.LineHandlers
{
    public static class ExceptBasedLineHandler
    {
        public static void Handle(string line, string[] queryLines)
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
    }
}
