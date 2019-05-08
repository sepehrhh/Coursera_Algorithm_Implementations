using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q4SuffixTree : Processor
    {
        public Q4SuffixTree(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String[]>)Solve);

        public string[] Solve(string text)
        {
            var result = new List<string>();
            var SA = new SuffixArray();
            var suffixArray = SA.BuildSuffixArray(text);
            var lcpArray = SA.ComputeLCPArray(text, suffixArray);
            var ST = new SuffixTree();
            var suffixTree = ST.BuildSuffixTree(text, suffixArray, lcpArray);
            var treeNode = suffixTree.Children;
            TraverseTree(text, treeNode, ref result);

            return result.ToArray();
        }

        private void TraverseTree(string text, Dictionary<char, SuffixTree.SuffixTreeNode> treeNode,
            ref List<string> result)
        {
            foreach (var child in treeNode)
            {
                var childString = text.Substring(child.Value.EdgeStart,
                    child.Value.EdgeEnd - child.Value.EdgeStart + 1);
                result.Add(childString);
                if (child.Value.Children.Count != 0)
                    TraverseTree(text, child.Value.Children, ref result);
            }
        }

    }
}
