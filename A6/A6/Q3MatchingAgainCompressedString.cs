using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A6
{
    public class Q3MatchingAgainCompressedString : Processor
    {
        public Q3MatchingAgainCompressedString(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) => 
        TestTools.Process(inStr, (Func<String, long, String[], long[]>)Solve);

        public long[] Solve(string text, long n, String[] patterns)
        {
            var result = new long[n];
            var firstColumn = new List<(char, int)>(text.Length);
            var lastColumn = new List<(char, int)>(text.Length);
            var countArray = new List<Dictionary<char, int>>();
            countArray.Add(new Dictionary<char, int>(text.Length + 1) { { 'A', 0 }, { 'C', 0 },
                { 'G', 0 }, { 'T', 0 }, { '$', 0 }});

            for (int i = 0; i < text.Length; i++)
            {
                firstColumn.Add((text[i], i));
                lastColumn.Add((text[i], i));
                countArray.Add(new Dictionary<char, int>(countArray[i]));
                countArray[i + 1][text[i]]++;
            }

            firstColumn = firstColumn.OrderBy(x => x.Item1).ToList();

            for (int i = 0; i < n; i++)
                result[i] = BWMatching(countArray, firstColumn, lastColumn, patterns[i]);

            return result;
        }

        private long BWMatching(List<Dictionary<char, int>> count,
            List<(char, int)> firstColumn, List<(char, int)> lastColumn, string pattern)
        {
            var top = 0;
            var bottom = lastColumn.Count - 1;
            while (top <= bottom)
            {
                if (pattern.Length > 0)
                {
                    var symbol = pattern.Last();
                    pattern = pattern.Remove(pattern.Length - 1);
                    var symbolFirstOccurence = firstColumn.FindIndex(x => x.Item1 == symbol);
                    top = symbolFirstOccurence + count[top][symbol];
                    bottom = symbolFirstOccurence + count[bottom + 1][symbol] - 1;
                }
                else
                    return bottom - top + 1;
            }

            return 0;
        }
    }
}
