using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A6
{
    public class Q1ConstructBWT : Processor
    {
        public Q1ConstructBWT(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, String>)Solve);

        public string Solve(string text)
        {
            return BWT(text);
        }

        private string BWT(string text)
        {
            var strList = new List<string> { text };
            for (int i = 0; i < text.Length - 1; i++)
            {
                string newStr = text.Last() + text.Substring(0, text.Length - 1);
                strList.Add(newStr);
                text = newStr;
            }
            strList.Sort();
            var resultStr = String.Empty;
            foreach (var str in strList)
                resultStr += str.Last();

            return resultStr;
        }
    }
}
