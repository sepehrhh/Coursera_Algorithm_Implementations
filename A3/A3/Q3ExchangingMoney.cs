using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using A2;

namespace A3
{
    public class Q3ExchangingMoney:Processor
    {
        public Q3ExchangingMoney(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, string[]>)Solve);


        public string[] Solve(long nodeCount, long[][] edges,long startNode)
        {
            var nodes = new Node[nodeCount];
            for (int i = 0; i < nodeCount; i++)
                nodes[i] = new Node(i + 1, int.MaxValue);

            nodes[startNode - 1].Distance = 0;
            var graph = Q1MinCost.MakeGraph(nodeCount, nodes, edges);
            var infinityCostNodes = new List<Node>();
            var result = new string[nodeCount];

            for (int i = 0; i < 2 * nodeCount; i++)
            {
                foreach (var node in graph)
                    foreach (var edge in node.Value)
                    {
                        var source = nodes[node.Key - 1];
                        var sink = edge.Item1;
                        var weight = edge.Item2;
                        if (sink.Distance > source.Distance + weight)
                        {
                            sink.Distance = source.Distance + weight;
                            sink.Prev = source;
                            if (i >= nodeCount - 1)
                            {
                                result[source.Data - 1] = "-";
                                result[sink.Data - 1] = "-";
                            }
                        }
                    }
            }

            var reachableNodes = new List<Node>();
            var queue = new Queue<Node>();
            queue.Enqueue(nodes[startNode - 1]);
            var visited = new bool[nodeCount];

            while (queue.Count > 0)
            {
                var peekNode = queue.Dequeue();
                reachableNodes.Add(peekNode);
                foreach (var node in graph[peekNode.Data])
                    if (!visited[node.Item1.Data - 1])
                    {
                        visited[node.Item1.Data - 1] = true;
                        queue.Enqueue(node.Item1);
                    }
            }

            for (int i = 0; i < nodeCount; i++)
                if (!reachableNodes.Contains(nodes[i]))
                    result[i] = "*";
                else if (result[i] != "-")
                        result[i] = nodes[i].Distance.ToString();

            return result;
        }
    }
}
