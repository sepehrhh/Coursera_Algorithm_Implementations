using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A7
{
    public class Q1FindAllOccur : Processor
    {
        public Q1FindAllOccur(string testDataName) : base(testDataName)
        {
            this.ExcludeTestCaseRangeInclusive(1, 2);
            this.ExcludeTestCaseRangeInclusive(4, 50);
			this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String, long[]>)Solve, "\n");

        public long[] Solve(string text, string pattern)
        {
            return KMP(text, pattern);
        }

        private long[] KMP(string text, string pattern)
        {
            var str = pattern + '$' + text;
            var s = ComputePrefix(str);
            var result = new List<long>();

            for (int i = pattern.Length + 1; i < str.Length; i++)
                if (s[i] == pattern.Length)
                    result.Add(i - 2 * pattern.Length);

            return result.ToArray();
        }

        private long[] ComputePrefix(string pattern)
        {
            var s = new long[pattern.Length];
            s[0] = 0;
            var border = 0;

            for (int i = 1; i < pattern.Length; i++)
            {
                while (border > 0 && pattern[i] != pattern[border])
                    border = (int)s[border - 1];

                if (pattern[i] == pattern[border])
                    border++;
                else
                    border = 0;
                s[i] = border;
            }

            return s;
        }

    }
}
