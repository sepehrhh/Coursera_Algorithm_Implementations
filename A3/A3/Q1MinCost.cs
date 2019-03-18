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
            for (int i = 0; i < nodeCount; i++)
                nodes[i] = new Node(i + 1, long.MaxValue);

            nodes[startNode - 1].Distance = 0;
            var priorityQueue = new PriorityQueue((int)nodeCount, nodes);
            var graph = MakeGraph(nodeCount, nodes, edges);

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

        public static Dictionary<long, List<(Node, long)>> MakeGraph(long nodeCount, Node[] nodes, long[][] edges)
        {
            var nodeConnections = new Dictionary<long, List<(Node, long)>>();
            for (int i = 1; i <= nodeCount; i++)
            {
                nodeConnections.Add(i, new List<(Node, long)>());
                foreach (var edge in edges)
                    if (edge[0] == i)
                        nodeConnections[i].Add((nodes[edge[1] - 1], edge[2]));
            }
            return nodeConnections;
        }
    }
}
