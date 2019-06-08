using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A9
{
    public class Q2OptimalDiet : Processor
    {
        public Q2OptimalDiet(string testDataName) : base(testDataName)
        {
            this.ExcludeTestCaseRangeInclusive(2, 100);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int,int, double[,], String>)Solve);

        private double[,] SimplexMatrix;
        private int MatrixSize;

        public string Solve(int N,int M, double[,] matrix1)
        {
            MatrixSize = N + 1;
            SimplexMatrix = new double[N + 1, M + N + 1];

            for (int i = 0; i < N + 1; i++)
            {
                for (int j = 0; j < N + M + 1; j++)
                {
                    if (j < M)
                        SimplexMatrix[i, j] = matrix1[i, j];
                    else if (j == M + N)
                        SimplexMatrix[i, j] = matrix1[i, N - 1];
                    else
                    {
                        if (i != N)
                        {
                            if (j == i + M)
                                SimplexMatrix[i, j] = 1;
                            else
                                SimplexMatrix[i, j] = 0;
                        }
                        else
                            SimplexMatrix[i, j] = 0;
                    }
                }
            }

            PerformOperation();





            return null;
        }



        private void SwapRows(int i, int j)
        {
            for (int k = 0; k < MatrixSize + 1; k++)
                (SimplexMatrix[i, k], SimplexMatrix[j, k]) = (SimplexMatrix[j, k], SimplexMatrix[i, k]);
        }

        private void MultiplyRow(int i, double multiplier)
        {
            for (int k = 0; k < MatrixSize + 1; k++)
                SimplexMatrix[i, k] *= multiplier;
        }

        private void AddRows(int i, int j, double multiplier)
        {
            for (int k = 0; k < MatrixSize + 1; k++)
                SimplexMatrix[i, k] += SimplexMatrix[j, k] * multiplier;
        }

        private void PerformOperation()
        {
            for (int i = 0; i < MatrixSize; i++)
            {
                if (SimplexMatrix[i, i] == 0)
                {
                    var c = 1;
                    while ((i + c) < MatrixSize && SimplexMatrix[i + c, i] == 0)
                        c++;
                    if ((i + c) == MatrixSize)
                        break;
                    SwapRows(i, i + c);
                }

                for (int j = 0; j < MatrixSize; j++)
                {
                    if (i != j)
                    {
                        double pro = SimplexMatrix[j, i] / SimplexMatrix[i, i];
                        AddRows(j, i, -pro);
                    }
                }
            }

            return;
        }

    }


}
