using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using A2;

namespace A4
{
    public class Q1BuildingRoads : Processor
    {
        public Q1BuildingRoads(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], double>)Solve);


        public double Solve(long pointCount, long[][] points)
        {
            var disjointSet = new DisjointSet(pointCount);
            for (int i = 0; i < pointCount; i++)
                disjointSet.MakeSet(i);

            var edges = MakeEdges(pointCount, points);
            edges = edges.OrderBy(x => x.Item3).ToList();
            double minDistance = 0;

            foreach (var edge in edges)
            {
                var startNode = edge.Item1;
                var sinkNode = edge.Item2;
                if (disjointSet.Find(startNode) != disjointSet.Find(sinkNode))
                {
                    disjointSet.Union(startNode, sinkNode);
                    minDistance += edge.Item3;
                }
            }
                
            return Math.Round(minDistance, 6);
        }

        public static List<(long, long, double)> MakeEdges(long pointCount, long[][] points)
        {
            var edges = new List<(long, long, double)>();

            for (int i = 0; i < pointCount; i++)
                for (int j = i + 1; j < pointCount; j++)
                    edges.Add((i, j, ComputeDistance(points[i], points[j])));
            return edges;
        }

        public static double ComputeDistance(long[] startNode, long[] sinkNode)
            => Math.Sqrt(Math.Pow((sinkNode[0] - startNode[0]), 2) +
                Math.Pow((sinkNode[1] - startNode[1]), 2));
    }
}
