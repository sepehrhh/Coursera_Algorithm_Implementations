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
            this.ExcludeTestCaseRangeInclusive(3, 50);
			this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String, long[]>)Solve, "\n");

        public long[] Solve(string text, string pattern)
        {
            var suffixArray = BuildSuffixArray(text);
            return PatternMatching(text, pattern, suffixArray);
        }

        private long[] PatternMatching(string text, string pattern, int[] suffixArray)
        {
            var minIndex = 0;
            var maxIndex = text.Length;
            while (minIndex < maxIndex)
            {
                var midIndex = (minIndex + maxIndex) / 2;
                if (String.Compare(pattern, text.Substring(suffixArray[midIndex])) < 0)
                    minIndex++;
                else
                    maxIndex = midIndex;
            }
            return null;
            //maxIndex = text.Length;

            //while (minIndex < maxIndex)
            //{
            //    var midIndex = (minIndex + maxIndex) / 2;
            //}
        }

        private int[] BuildSuffixArray(string text)
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

        private int[] UpdateClasses(int[] newOrder, int[] _class, int l)
        {
            var n = newOrder.Length;
            var newClass = new int[n];
            newClass[newOrder[0]] = 0;

            for (int i = 1; i < n; i++)
            {
                var cur = newOrder[i];
                var prev = newOrder[i - 1];
                var mid = cur + l;
                var midPrev = (prev + l) % n;
                if (_class[cur] != _class[prev] || _class[mid] != _class[midPrev])
                    newClass[cur] = newClass[prev] + 1;
                else
                    newClass[cur] = newClass[prev];
            }

            return newClass;
        }

        private int[] SortDoubled(string text, int l, int[] order, int[] _class)
        {
            var count = new int[text.Length];
            var newOrder = new int[text.Length];

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

        private int[] ComputeCharClasses(string text, int[] order)
        {
            var _class = new int[text.Length];
            _class[order[0]] = 0;
            for (int i = 1; i < text.Length; i++)
            {
                if (text[order[i]] != text[order[i - 1]])
                    _class[order[i]] = _class[order[i - 1]] + 1;
                else
                    _class[order[i]] = _class[order[i - 1]];
            }
            return _class;
        }

        private int[] SortCharacters(string text)
        {
            var order = new int[text.Length];
            var count = new int[26];

            for (int i = 0; i < text.Length; i++)
                count[text[i] - 'A']++;

            for (int j = 1; j < count.Length - 1; j++)
                count[j] = count[j] + count[j + 1];

            for (int i = text.Length - 1; i >= 0; i--)
            {
                var c = text[i] - 'A';
                count[c]--;
                order[count[c]] = i;
            }

            return order;
        }
    }
}
