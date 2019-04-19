using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A6
{
    public class Q2ReconstructStringFromBWT : Processor
    {
        public Q2ReconstructStringFromBWT(string testDataName) : base(testDataName)
        {
            this.ExcludeTestCaseRangeInclusive(31, 50);
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String>)Solve);

        public string Solve(string bwt)
        {
            return BWTRevert(bwt);
        }

        private string BWTRevert(string bwt)
        {
            var BWTList = new List<(char, int)>();

            for (int i = 0; i < bwt.Length; i++)
                BWTList.Add((bwt[i], i));

            var sortedBWTList = BWTList.OrderBy(x => x.Item1).ToList();
            var sortedDic = new Dictionary<int, int>();

            for (int i = 0; i < sortedBWTList.Count; i++)
                sortedDic.Add(sortedBWTList[i].Item2, i);

            var result = new List<char> { '$' };
            var index = 0;

            while (result.Count != bwt.Length)
            {
                result.Add(BWTList[index].Item1);
                index = sortedDic[index];
            }

            result.Reverse();
            return new string (result.ToArray());
        }
    }
}
