using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using A2;

namespace A3
{
    public class Q4FriendSuggestion : Processor
    {
        public Q4FriendSuggestion(string testDataName) : base(testDataName)
        {
            //this.ExcludeTestCaseRangeInclusive(1, 36);
            this.ExcludeTestCaseRangeInclusive(36, 50);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long, long[][], long[]>)Solve);

        public long[] Solve(long NodeCount, long EdgeCount,
                              long[][] edges, long QueriesCount,
                              long[][] Queries)
        {
            var result = new List<long>((int)QueriesCount);
            var nodes = new Node[NodeCount];
            var reversedNodes = new Node[NodeCount];

            var nodesClone = new Node[NodeCount];
            var reversedNodesClone = new Node[NodeCount];

            for (int i = 0; i < NodeCount; i++)
            {
                nodes[i] = new Node(i + 1, int.MaxValue, null);
                reversedNodes[i] = new Node(i + 1, int.MaxValue, null);
                nodesClone[i] = new Node(i + 1, int.MaxValue, null);
                reversedNodesClone[i] = new Node(i + 1, int.MaxValue, null);
            }


            var graph = MakeGraph(NodeCount, nodes, edges, false);
            var reversedGraph = MakeGraph(NodeCount,
                reversedNodes, edges, true);

            foreach (var query in Queries)
            {
                var startNode = query[0];
                var targetNode = query[1];

                for (int i = 0; i < NodeCount; i++)
                {
                    nodes[i].Distance = int.MaxValue;
                    reversedNodes[i].Distance = int.MaxValue;
                }

                result.Add(BidirectionalDijkstra(nodes, reversedNodes, graph, reversedGraph, startNode, targetNode,
                    edges, NodeCount, EdgeCount));
            }


            return result.ToArray();
        }

        private long BidirectionalDijkstra(Node[] nodes, Node[] reversedNodes,
            Dictionary<long, List<(Node, long)>> graph,
            Dictionary<long, List<(Node, long)>> reversedGraph,
            long startNode, long targetNode,
            long[][] edges, long NodeCount, long EdgeCount)
        {
            if (startNode == targetNode)
                return 0;

            nodes[startNode - 1].Distance = 0;
            reversedNodes[targetNode - 1].Distance = 0;

            var nodesQ = new PriorityQueue((int)NodeCount, nodes);
            var reversedNodesQ = new PriorityQueue((int)NodeCount, reversedNodes);

            var processedNodes = new List<long>();
            var reversedNodesProcessed = new List<long>();

            if (reversedGraph[targetNode].Count == 0)
                return -1;

            do
            {
                var currentNode = nodesQ.ExtractMin();
                ProcessNode(currentNode, graph, nodes, processedNodes, nodesQ);

                if (reversedNodesProcessed.Contains(currentNode.Data))
                    return currentNode.Distance != int.MaxValue ? ShortestPath(startNode, targetNode, nodes,
                        reversedNodes, processedNodes, reversedNodesProcessed) : -1;

                var currentReverseNode = reversedNodesQ.ExtractMin();

                ProcessNode(currentReverseNode, reversedGraph, reversedNodes,
                    reversedNodesProcessed, reversedNodesQ);

                if (processedNodes.Contains(currentReverseNode.Data))
                    return currentNode.Distance != int.MaxValue ? ShortestPath(startNode, targetNode, nodes,
                        reversedNodes, processedNodes, reversedNodesProcessed) : -1;

            } while (true);
        }


        private void ProcessNode(Node currentNode,
            Dictionary<long, List<(Node, long)>> graph,
            Node[] nodes, List<long> processed, PriorityQueue nodesQ)
        {
            foreach (var edge in graph[currentNode.Data])
                Relax(currentNode, edge.Item1, edge.Item2, nodesQ);
            processed.Add(currentNode.Data);
        }


        private void Relax(Node startNode, Node sinkNode, long weight, PriorityQueue nodesQ)
        {
            if (sinkNode.Distance > startNode.Distance + weight)
            {
                sinkNode.Prev = startNode;
                nodesQ.ChangePriority(Array.IndexOf(nodesQ.MinHeap, sinkNode),
                                (int)startNode.Distance + (int)weight);
            }
        }


        private long ShortestPath(long startNode, long targetNode,
            Node[] nodes, Node[] reversedNodes, List<long> processedNodes,
            List<long> reverseProcessedNodes)
        {
            long distance = int.MaxValue;
            long best = -1;
            foreach (var nodeData in processedNodes.Concat(reverseProcessedNodes))
            {
                var directNode = nodes[nodeData - 1];
                var reverseNode = reversedNodes[nodeData - 1];
                if (directNode.Distance + reverseNode.Distance < distance)
                {
                    best = nodeData;
                    distance = directNode.Distance + reverseNode.Distance;
                }
            }

            return distance;
        }

        public static Dictionary<long, List<(Node, long)>> MakeGraph(long nodeCount, Node[] nodes, long[][] edges, bool reverse)
        {
            var nodeConnections = new Dictionary<long, List<(Node, long)>>();

            if (!reverse)
            {
                for (int i = 1; i <= nodeCount; i++)
                {
                    nodeConnections.Add(i, new List<(Node, long)>());
                    foreach (var edge in edges)
                        if (edge[0] == i)
                            nodeConnections[i].Add((nodes[edge[1] - 1], edge[2]));
                }
            }

            else
            {
                for (int i = 1; i <= nodeCount; i++)
                {
                    nodeConnections.Add(i, new List<(Node, long)>());
                    foreach (var edge in edges)
                        if (edge[1] == i)
                            nodeConnections[i].Add((nodes[edge[0] - 1], edge[2]));
                }
            }

            return nodeConnections;
        }

    }
}
