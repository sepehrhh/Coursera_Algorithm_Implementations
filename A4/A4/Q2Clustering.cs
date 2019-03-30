using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A4
{
    public class Q2Clustering : Processor
    {
        public Q2Clustering(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, double>)Solve);


        public double Solve(long pointCount, long[][] points, long clusterCount)
        {
            var disjointSet = new DisjointSet(pointCount);
            var clusters = new Dictionary<long, List<long[]>>();
            for (int i = 0; i < pointCount; i++)
            {
                disjointSet.MakeSet(i);
                clusters.Add(i, new List<long[]> { points[i] });
            }

            var edges = Q1BuildingRoads.MakeEdges(pointCount, points);
            edges = edges.OrderBy(x => x.Item3).ToList();
            
            var unionNum = 0;

            foreach (var edge in edges)
            {
                if (pointCount - unionNum == clusterCount)
                    break;

                var startNode = edge.Item1;
                var sinkNode = edge.Item2;
                var startNodeRoot = disjointSet.Find(startNode);
                var sinkNodeRoot = disjointSet.Find(sinkNode);

                if (startNodeRoot != sinkNodeRoot)
                {
                    unionNum++;
                    disjointSet.Union(startNode, sinkNode);
                    clusters[sinkNodeRoot].AddRange(clusters[startNodeRoot]);
                    clusters.Remove(startNodeRoot);
                }
            }

            double distance = int.MaxValue;
            var dicKeys = clusters.Keys.ToList();
            for (int i = 0; i < dicKeys.Count; i++)
                for (int j = i + 1; j < dicKeys.Count; j++)
                {
                    foreach (var point1 in clusters[dicKeys[i]])
                        foreach (var point2 in clusters[dicKeys[j]])
                        {
                            var pointsDist = Q1BuildingRoads.ComputeDistance(point1, point2);
                            if (pointsDist < distance)
                                distance = pointsDist;
                        }
                }

            return Math.Round(distance, 6);
        }
    }
}
