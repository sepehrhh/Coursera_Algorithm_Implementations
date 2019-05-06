using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A7
{
    public class Q2CunstructSuffixArray : Processor
    {
        public Q2CunstructSuffixArray(string testDataName) : base(testDataName)
        {
            this.ExcludeTestCaseRangeInclusive(1, 3);
            this.ExcludeTestCaseRangeInclusive(5, 50);
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long[]>)Solve);

        private long[] Solve(string text)
        {
            var suffixArray = new SuffixArray();
            return suffixArray.BuildSuffixArray(text);
        }
    }
}
