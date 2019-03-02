using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A2
{
    public class Q1ShortestPath : Processor
    {
        public Q1ShortestPath(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long,long[][], long, long, long>)Solve);
        
        public long Solve(long NodeCount, long[][] edges, long StartNode,  long EndNode)
        {
            var nodes = new Node[NodeCount];
            for (int i = 0; i < nodes.Length; i++)
                nodes[i] = new Node(i + 1, long.MaxValue);

            nodes[StartNode - 1].Distance = 0;
            var priorityQueue = new PriorityQueue((int)NodeCount, nodes);
            var Graph = MakeGraph(NodeCount, nodes, edges);

            if (Graph[StartNode].Count == 0)
                return -1;

            while (priorityQueue.Size > 0)
            {
                var currentNode = priorityQueue.ExtractMin();
                foreach (var node in Graph[currentNode.Data])
                    if (currentNode.Distance < long.MaxValue)
                        if (node.Distance > currentNode.Distance + 1)
                            priorityQueue.ChangePriority(Array.IndexOf(priorityQueue.MinHeap, node), (int)currentNode.Distance + 1);
            }

            return nodes[(int)EndNode - 1].Distance != long.MaxValue ? nodes[(int)EndNode - 1].Distance : -1;
        }

        private Dictionary<long, List<Node>> MakeGraph(long nodeCount, Node[] nodes, long[][] edges)
        {
            var nodeConnections = new Dictionary<long, List<Node>>();
            for (int i = 1; i <= nodeCount; i++)
            {
                nodeConnections.Add(i, new List<Node>());
                foreach (var edge in edges)
                    if (edge.Contains(i))
                        nodeConnections[i].Add(nodes[edge.Where(x => x != i).First() - 1]);
            }
            return nodeConnections;
        }

    }
}
