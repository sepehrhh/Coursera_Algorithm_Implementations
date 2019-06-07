using System;
using TestCommon;

namespace A9
{
    public class Q1InferEnergyValues : Processor
    {
        public Q1InferEnergyValues(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, double[,], double[]>)Solve);

        private long MatrixSize;
        private double[,] Matrix;

        public double[] Solve(long MATRIX_SIZE, double[,] matrix)
        {
            MatrixSize = MATRIX_SIZE;
            Matrix = matrix;
            PerformOperation();
            return GetResult();
        }

        private double[] GetResult()
        {
            var result = new double[MatrixSize];
            for (int i = 0; i < MatrixSize; i++)
            {
                result[i] = Matrix[i, MatrixSize] / Matrix[i, i];
                var decimals = Math.Abs(result[i] - Math.Truncate(result[i]));
                if (decimals < 0.25)
                    if (result[i] > 0)
                        result[i] = Math.Floor(result[i]);
                    else
                        result[i] = Math.Ceiling(result[i]);
                else if (decimals >= 0.75)
                    if (result[i] > 0)
                        result[i] = Math.Ceiling(result[i]);
                    else
                        result[i] = Math.Floor(result[i]);
                else
                    result[i] = Math.Floor(result[i]) + 0.5;
            }

            return result;
        }

        private void SwapRows(int i, int j)
        {
            for (int k = 0; k < MatrixSize + 1; k++)
                (Matrix[i, k], Matrix[j, k]) = (Matrix[j, k], Matrix[i, k]);
        }

        private void MultiplyRow(int i, double multiplier)
        {
            for (int k = 0; k < MatrixSize + 1; k++)
                Matrix[i, k] *= multiplier;
        }

        private void AddRows(int i, int j, double multiplier)
        {
            for (int k = 0; k < MatrixSize + 1; k++)
                Matrix[i, k] += Matrix[j, k] * multiplier;
        }

        private void PerformOperation()
        {
            for (int i = 0; i < MatrixSize; i++)
            {
                if (Matrix[i, i] == 0)
                {
                    var c = 1;
                    while ((i + c) < MatrixSize && Matrix[i + c, i] == 0)
                        c++;
                    if ((i + c) == MatrixSize)
                        break;
                    SwapRows(i, i + c);
                }

                for (int j = 0; j < MatrixSize; j++)
                {
                    if (i != j)
                    {
                        double pro = Matrix[j, i] / Matrix[i, i];
                        AddRows(j, i, -pro);
                    }
                }
            }

            return;
        }

    }
}
