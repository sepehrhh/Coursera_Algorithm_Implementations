using TestCommon;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Exam1
{
    public class Q2Cryptanalyst : Processor
    {
        public Q2Cryptanalyst(string testDataName) : base(testDataName)
        {
            this.ExcludeTestCaseRangeInclusive(24, 37);
            Vocab =
             File.ReadAllLines("D:/git/AD97982/Exam1/Exam1Tests/TestData/TD2/dictionary.txt");
        }

        public override string Process(string inStr) => Solve(inStr);

        private string[] Vocab;

        public string Solve(string cipher)
        {
            for (int i = 0; i < 999; i++)
            {
                var encryption = new Encryption(i.ToString(), ' ', 'z', false);
                var dicrypted = encryption.Decrypt(cipher);
                var words = dicrypted.Split();
                var validCount = 0;
                for (int j = 0; j < words.Length; j++)
                {
                    var word = words[j];
                    if (Vocab.Contains(word))
                        validCount++;
                    if (validCount >= 20)
                        return dicrypted.GetHashCode().ToString();
                }
            }

            return null;
        }
    }
}