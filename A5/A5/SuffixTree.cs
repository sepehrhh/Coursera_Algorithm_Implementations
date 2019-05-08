using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5
{
    public class SuffixTree
    {
        public class SuffixTreeNode
        {
            public SuffixTreeNode Parent;
            public Dictionary<char, SuffixTreeNode> Children;
            public int StringDepth;
            public int EdgeStart;
            public int EdgeEnd;

            public SuffixTreeNode(Dictionary<char, SuffixTreeNode> children,
                SuffixTreeNode parent, int stringDepth, int edgeStart, int edgeEnd)
            {
                Children = children;
                Parent = parent;
                StringDepth = stringDepth;
                EdgeStart = edgeStart;
                EdgeEnd = edgeEnd;
            }
        }

        public SuffixTreeNode BuildSuffixTree(string text, long[] SuffixArray,
            long[] LCPArray/*, ref List<string> result*/)
        {
            var root = new SuffixTreeNode(new Dictionary<char, SuffixTreeNode>(), null, 0,
                -1, -1);
            var lcpPrev = 0;
            var curNode = root;

            for (int i = 0; i < text.Length; i++)
            {
                var suffix = SuffixArray[i];
                while (curNode.StringDepth > lcpPrev)
                    curNode = curNode.Parent;
                if (curNode.StringDepth == lcpPrev)
                    curNode = CreateNewLeaf(curNode, text, suffix);
                else
                {
                    var edgeStart = SuffixArray[i - 1] + curNode.StringDepth;
                    var offset = lcpPrev - curNode.StringDepth;
                    var midNode = BreakEdge(curNode, text, edgeStart, offset);
                    curNode = CreateNewLeaf(midNode, text, suffix);
                }
                if (i < text.Length - 1)
                    lcpPrev = (int)LCPArray[i];
                //result.Add(text.Substring(curNode.EdgeStart, curNode.EdgeEnd - curNode.EdgeStart + 1));
            }

            return root;
        }

        private SuffixTreeNode BreakEdge(SuffixTreeNode node, string text, long start,
            int offset)
        {
            var startChar = text[(int)start];
            var midChar = text[(int)start + offset];
            var midNode = new SuffixTreeNode(new Dictionary<char, SuffixTreeNode>(),
                node, node.StringDepth + offset, (int)start,
                (int)start + offset - 1);
            midNode.Children[midChar] = node.Children[startChar];
            node.Children[startChar].Parent = midNode;
            node.Children[startChar].EdgeStart += offset;
            node.Children[startChar] = midNode;

            return midNode;
        }

        private SuffixTreeNode CreateNewLeaf(SuffixTreeNode curNode, string text,
            long suffix)
        {
            var leaf = new SuffixTreeNode(new Dictionary<char, SuffixTreeNode>(), curNode,
                text.Length - (int)suffix,
                (int)suffix + curNode.StringDepth, text.Length - 1);
            curNode.Children[text[leaf.EdgeStart]] = leaf;
            return leaf;
        }

    }
}
