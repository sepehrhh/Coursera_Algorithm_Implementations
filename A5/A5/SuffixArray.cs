using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5
{
    public class SuffixArray
    {
        private Dictionary<char, int> Alphabet = new Dictionary<char, int>()
        {
            { '$', 0 },
            { 'A', 1 },
            { 'C', 2 },
            { 'G', 3 },
            { 'T', 4 }
        };

        public long[] BuildSuffixArray(string text)
        {
            var order = SortCharacters(text);
            var _class = ComputeCharClasses(text, order);
            var l = 1;
            while (l < text.Length)
            {
                order = SortDoubled(text, l, order, _class);
                _class = UpdateClasses(order, _class, l);
                l *= 2;
            }
            return order;
        }

        private long[] UpdateClasses(long[] newOrder, long[] _class, int l)
        {
            var n = newOrder.Length;
            var newClass = new long[n];
            newClass[newOrder[0]] = 0;
            for (int i = 1; i < n; i++)
            {
                var cur = newOrder[i];
                var prev = newOrder[i - 1];
                var mid = (cur + l) % n;
                var midPrev = (prev + l) % n;
                if (_class[cur] != _class[prev] || _class[mid] != _class[midPrev])
                    newClass[cur] = newClass[prev] + 1;
                else
                    newClass[cur] = newClass[prev];
            }

            return newClass;
        }

        private long[] SortDoubled(string text, int l, long[] order, long[] _class)
        {
            var count = new long[text.Length];
            var newOrder = new long[text.Length];

            for (int i = 0; i < text.Length; i++)
                count[_class[i]]++;

            for (int j = 1; j < text.Length; j++)
                count[j] = count[j] + count[j - 1];

            for (int i = text.Length - 1; i >= 0; i--)
            {
                var start = (order[i] - l + text.Length) % (text.Length);
                var cl = _class[start];
                count[cl]--;
                newOrder[count[cl]] = start;
            }

            return newOrder;
        }

        private long[] ComputeCharClasses(string text, long[] order)
        {
            var _class = new long[text.Length];
            _class[order[0]] = 0;
            for (int i = 1; i < text.Length; i++)
            {
                if (text[(int)order[i]] != text[(int)order[i - 1]])
                    _class[(int)order[i]] = _class[(int)order[i - 1]] + 1;
                else
                    _class[(int)order[i]] = _class[(int)order[i - 1]];
            }
            return _class;
        }

        private long[] SortCharacters(string text)
        {
            var order = new long[text.Length];
            var count = new long[Alphabet.Count];

            for (int i = 0; i < text.Length; i++)
                count[Alphabet[text[i]]]++;

            for (int j = 1; j < count.Length; j++)
                count[j] = count[j] + count[j - 1];

            for (int i = text.Length - 1; i >= 0; i--)
            {
                var c = text[i];
                count[Alphabet[c]]--;
                order[count[Alphabet[c]]] = i;
            }

            return order;
        }

        public long[] ComputeLCPArray(string text, long[] suffixArray)
        {
            var lcpArray = new long[text.Length - 1];
            var lcp = 0;
            var posInOrder = InvertSuffixArray(suffixArray);
            var suffix = suffixArray[0];

            for (int i = 0; i < text.Length; i++)
            {
                var orderIndex = posInOrder[suffix];
                if (orderIndex == text.Length - 1)
                {
                    lcp = 0;
                    suffix = (suffix + 1) % text.Length;
                    continue;
                }
                var nextSuffix = suffixArray[orderIndex + 1];
                lcp = LCPOfSuffixes(text, suffix, nextSuffix, lcp - 1);
                lcpArray[orderIndex] = lcp;
                suffix = (suffix + 1) % text.Length;
            }

            return lcpArray;
        }

        private int LCPOfSuffixes(string text, long i, long j, int equal)
        {
            var lcp = Math.Max(0, equal);
            while (i + lcp < text.Length && j + lcp < text.Length)
                if (text[(int)i + lcp] == text[(int)j + lcp])
                    lcp++;
                else
                    break;

            return lcp;
        }

        private long[] InvertSuffixArray(long[] suffixArray)
        {
            var pos = new long[suffixArray.Length];
            for (int i = 0; i < pos.Length; i++)
                pos[suffixArray[i]] = i;
            return pos;
        }
    }
}
