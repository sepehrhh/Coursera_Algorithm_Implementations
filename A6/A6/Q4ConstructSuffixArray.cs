using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A6
{
    public class Q4ConstructSuffixArray : Processor
    {
        public Q4ConstructSuffixArray(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) => 
            TestTools.Process(inStr, (Func<String, long[]>)Solve);

        public long[] Solve(string text)
        {
            return ConstructSuffixArray(text);
        }

        private long[] ConstructSuffixArray(string text)
        {
            var suffixArray = new string[text.Length];
            var suffixIndexArray = new long[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                var suffix = text.Substring(i);
                suffixArray[i] = suffix;
                suffixIndexArray[i] = i;
            }
            Array.Sort(suffixArray, suffixIndexArray);
            return suffixIndexArray;
        }
    }
}
