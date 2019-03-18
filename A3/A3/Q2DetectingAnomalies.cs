using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using A2;

namespace A3
{
    public class Q2DetectingAnomalies : Processor
    {
        public Q2DetectingAnomalies(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);


        public long Solve(long nodeCount, long[][] edges)
        {
            var nodes = new Node[nodeCount];
            for (int i = 0; i < nodeCount; i++)
                nodes[i] = new Node(i + 1, int.MaxValue);

            var graph = Q1MinCost.MakeGraph(nodeCount, nodes, edges);
            nodes[0].Distance = 0;

            for (int i = 0; i < nodeCount; i++)
            {
                foreach (var node in graph)
                    foreach (var edge in node.Value)
                    {
                        var source = nodes[node.Key - 1];
                        var sink = edge.Item1;
                        var weight = edge.Item2;
                        if (sink.Distance > source.Distance + weight)
                        {
                            if (i == nodeCount - 1)
                                return 1;
                            sink.Distance = source.Distance + weight;
                        }
                    }
            }

            return 0;
        }

    }
}
