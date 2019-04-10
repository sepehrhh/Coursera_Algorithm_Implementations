using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5
{
    public class Trie
    {
        public class Node
        {
            public long Data;
            public Dictionary<char, Node>
                Children = new Dictionary<char, Node>();
            public bool IsLeaf = false;

            public Node(long data)
            {
                Data = data;
            }
        }

        public Node TrieRoot = new Node(0);
        public List<string> VisualizedGraph = new List<string>();

        public Trie(string[] patterns)
        {
            var root = TrieRoot;
            long index = 0;
            foreach (var pattern in patterns)
            {
                root = TrieRoot;
                foreach (var ch in pattern)
                {
                    if (!root.Children.ContainsKey(ch))
                    {
                        VisualizedGraph.Add($"{root.Data}->{index + 1}:{ch}");
                        index++;
                        root.Children.Add(ch, new Node(index));
                    }
                    root = root.Children[ch];
                }
                root.IsLeaf = true;
            }
        }

        public List<long> PatternMatching(string text)
        {
            var result = new List<long>();
            for (int i = 0; i < text.Length; i++)
            {
                var root = TrieRoot;
                var currentIndex = i;
                while (currentIndex < text.Length && root.Children.ContainsKey(text[currentIndex]))
                {
                    root = root.Children[text[currentIndex]];
                    currentIndex++;
                    if (root.IsLeaf)
                    {
                        result.Add(i);
                        break;
                    }
                }
            }

            if (result.Count == 0)
                result.Add(-1);

            return result;
        }

    }
}
