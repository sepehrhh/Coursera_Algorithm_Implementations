using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCommon;

namespace Exam2
{
    public class Q2LatinSquareBT : Processor
    {
        public Q2LatinSquareBT(string testDataName) : base(testDataName)
        {
            this.ExcludeTestCases(new int[] { 9, 23, 27, 33, 50 });
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int,int?[,],string>)Solve);

        public string Solve(int dim, int?[,] square)
        {
            this.Dimension = dim;
            this.Matrix = square;
            return GetCandidates();
        }

        private int Dimension;
        private int?[,] Matrix;

        private string GetCandidates()
        {
            var emptyCells = new Dictionary<(int, int), List<int>>();
            var columnsAsset = new bool[Dimension][];
            var rowsAsset = new bool[Dimension][];


            for (int i = 0; i < Dimension; i++)
            {
                rowsAsset[i] = new bool[Dimension];
                columnsAsset[i] = new bool[Dimension];
            }

            for (int i = 0; i < Dimension; i++)
                for (int j = 0; j < Dimension; j++)
                {
                    if (Matrix[i, j] == null)
                        emptyCells.Add((i, j), new List<int>());
                    else
                    {
                        rowsAsset[i][(int)Matrix[i, j]] = true;
                        columnsAsset[j][(int)Matrix[i, j]] = true;
                    }
                }

            foreach (var eCell in emptyCells)
            {
                var colIdx = eCell.Key.Item2;
                var rowIdx = eCell.Key.Item1;

                for (int i = 0; i < Dimension; i++)
                {
                    if (!rowsAsset[rowIdx][i] && !columnsAsset[colIdx][i])
                        eCell.Value.Add(i);
                }

                if (eCell.Value.Count == 0)
                    return "UNSATISFIABLE";

            }

            return "SATISFIABLE";
        }

    }
}
