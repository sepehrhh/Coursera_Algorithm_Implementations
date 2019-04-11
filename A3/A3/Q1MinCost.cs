using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using A2;

namespace A3
{
    public class Q1MinCost : Processor
    {
        public Q1MinCost(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, long, long>)Solve);


        public long Solve(long nodeCount, long[][] edges, long startNode, long endNode)
        {
            var nodes = new Node[nodeCount];
            var nodeConnections = new Dictionary<long, List<(Node, long)>>();
            for (int i = 0; i < nodeCount; i++)
            {
                nodes[i] = new Node(i + 1, long.MaxValue);
                nodeConnections.Add(i + 1, new List<(Node, long)>());
            }

            nodes[startNode - 1].Distance = 0;
            var priorityQueue = new PriorityQueue((int)nodeCount, nodes);
            var graph = MakeGraph(nodeCount, nodes, edges, nodeConnections);

            if (graph[startNode].Count == 0)
                return -1;

            while (priorityQueue.Size > 0)
            {
                var currentNode = priorityQueue.ExtractMin();
                foreach (var node in graph[currentNode.Data])
                    if (currentNode.Distance < long.MaxValue)
                        if (node.Item1.Distance > currentNode.Distance + node.Item2)
                            if (priorityQueue.MinHeap.Contains(node.Item1)) 
                                priorityQueue.ChangePriority(Array.IndexOf(priorityQueue.MinHeap, node.Item1),
                                    (int)currentNode.Distance + (int)node.Item2);
            }


            return nodes[(int)endNode - 1].Distance != long.MaxValue ? nodes[(int)endNode - 1].Distance : -1;
        }

        public static Dictionary<long, List<(Node, long)>> MakeGraph(long nodeCount, Node[] nodes, long[][] edges,
            Dictionary<long, List<(Node, long)>> nodeConnections)
        {
            foreach (var edge in edges)
                nodeConnections[edge[0]].Add((nodes[edge[1] - 1], edge[2]));
            return nodeConnections;
        }
    }
}
