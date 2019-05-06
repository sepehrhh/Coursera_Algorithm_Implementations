using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A7
{
    public class Q3PatternMatchingSuffixArray : Processor
    {
        public Q3PatternMatchingSuffixArray(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, string[], long[]>)Solve, "\n");

        private long[] Solve(string text, long n, string[] patterns)
        {
            text += '$';
            var suffixArray = new SuffixArray().BuildSuffixArray(text);
            var result = new List<long>();
            foreach (var pattern in patterns)
                PatternMatching(text, pattern, suffixArray, ref result);

            if (result.Count == 0)
                result.Add(-1);

            return result.Distinct().ToArray();
        }
        

        private void PatternMatching(string text, string pattern, long[] suffixArray, ref List<long> result)
        {
            var minIndex = 0;
            var maxIndex = text.Length;

            while (minIndex < maxIndex)
            {
                var midIndex = (minIndex + maxIndex) / 2;
                var suffix = text.Substring((int)suffixArray[midIndex], 
                    Math.Min((int)suffixArray[midIndex] + pattern.Length, 
                    text.Length) - (int)suffixArray[midIndex]);
                int comparison = pattern.CompareTo(suffix);

                if (comparison > 0)
                    minIndex = midIndex + 1;
                else
                    maxIndex = midIndex;
            }

            var start = minIndex;
            maxIndex = text.Length;

            while (minIndex < maxIndex)
            {
                var midIndex = (minIndex + maxIndex) / 2;
                var suffix = text.Substring((int)suffixArray[midIndex],
                    Math.Min((int)suffixArray[midIndex] + pattern.Length, text.Length)
                    - (int)suffixArray[midIndex]);
                int comparison = pattern.CompareTo(suffix);

                if (comparison < 0)
                    maxIndex = midIndex;
                else
                    minIndex = midIndex + 1;
            }

            var end = maxIndex;
            if (start <= end)
                for (int i = start; i < end; i++)
                    result.Add(suffixArray[i]);
        }

    }
}
