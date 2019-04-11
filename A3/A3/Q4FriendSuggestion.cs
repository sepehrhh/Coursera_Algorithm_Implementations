using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TestCommon;
using A2;

namespace A3
{
    public class Q4FriendSuggestion : Processor
    {
        public Q4FriendSuggestion(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long, long[][], long[]>)Solve);

        public long[] Solve(long NodeCount, long EdgeCount,
                              long[][] edges, long QueriesCount,
                              long[][] Queries)
        {
            var result = new long[(int)QueriesCount];
            var nodes = new Node[NodeCount];
            var reversedNodes = new Node[NodeCount];
            var directGraph = new Dictionary<long, List<(Node, long)>>();
            var reversedGraph = new Dictionary<long, List<(Node, long)>>();

            for (int i = 0; i < NodeCount; i++)
            {
                nodes[i] = new Node(i + 1, int.MaxValue);
                reversedNodes[i] = new Node(i + 1, int.MaxValue);
                directGraph.Add(i + 1, new List<(Node, long)>());
                reversedGraph.Add(i + 1, new List<(Node, long)>());
            }

            var graph = MakeBidirectionalGraph(NodeCount, nodes, reversedNodes, edges, directGraph, reversedGraph);

            for (int i = 0; i < QueriesCount; i++)
            {
                var startNode = Queries[i][0];
                var targetNode = Queries[i][1];

                for (int j = 0; j < NodeCount; j++)
                {
                    nodes[j].Distance = int.MaxValue;
                    reversedNodes[j].Distance = int.MaxValue;
                    nodes[j].QueueIndex = null;
                    nodes[j].ReversedQueueIndex = null;
                    reversedNodes[j].QueueIndex = null;
                    reversedNodes[j].ReversedQueueIndex = null;
                }

                result[i] = BidirectionalDijkstra(nodes, reversedNodes, graph.Item1, graph.Item2, startNode, targetNode,
                    edges, NodeCount, EdgeCount);
            }

            return result;
        }

        private long BidirectionalDijkstra(Node[] nodes, Node[] reversedNodes,
            Dictionary<long, List<(Node, long)>> graph,
            Dictionary<long, List<(Node, long)>> reversedGraph,
            long startNode, long targetNode,
            long[][] edges, long NodeCount, long EdgeCount)
        {
            if (startNode == targetNode)
                return 0;

            if (graph[startNode].Count == 0 || reversedGraph[targetNode].Count == 0)
                return -1;

            nodes[startNode - 1].Distance = 0;
            reversedNodes[targetNode - 1].Distance = 0;

            var nodesQ = new FastPriorityQueue((int)NodeCount);
            var reversedNodesQ = new FastPriorityQueue((int)NodeCount);

            nodesQ.Enqueue(nodes[startNode - 1], false);
            reversedNodesQ.Enqueue(reversedNodes[targetNode - 1], true);

            var processedNodes = new List<long>();
            var reversedNodesProcessed = new List<long>();

            do
            {
                var currentNode = nodesQ.ExtractPeek(false);
                ProcessNode(currentNode, graph, nodes, processedNodes, nodesQ, false);

                if (reversedNodesProcessed.Contains(currentNode.Data))
                    return currentNode.Distance != int.MaxValue ? ShortestPath(startNode, targetNode, nodes,
                        reversedNodes, processedNodes, reversedNodesProcessed) : -1;

                var currentReverseNode = reversedNodesQ.ExtractPeek(true);

                ProcessNode(currentReverseNode, reversedGraph, reversedNodes,
                    reversedNodesProcessed, reversedNodesQ, true);

                if (processedNodes.Contains(currentReverseNode.Data))
                    return currentNode.Distance != int.MaxValue ? ShortestPath(startNode, targetNode, nodes,
                        reversedNodes, processedNodes, reversedNodesProcessed) : -1;

            } while (nodesQ.Size > 0 && reversedNodesQ.Size > 0);

            return -1;
        }


        private void ProcessNode(Node currentNode,
            Dictionary<long, List<(Node, long)>> graph,
            Node[] nodes, List<long> processed, FastPriorityQueue nodesQ, bool mode)
        {
            foreach (var edge in graph[currentNode.Data])
                Relax(currentNode, edge.Item1, edge.Item2, nodesQ, mode);

            processed.Add(currentNode.Data);
        }


        private void Relax(Node startNode, Node sinkNode, long weight, FastPriorityQueue nodesQ, bool reverseMode)
        {
            if (sinkNode.Distance > startNode.Distance + weight)
            {
                sinkNode.Prev = startNode;
                var newSinkNodePriority = startNode.Distance + weight;
                if (!reverseMode)
                {
                    if (sinkNode.QueueIndex != null)
                    {
                        nodesQ.ChangePriority((int)sinkNode.QueueIndex,
                            newSinkNodePriority, reverseMode);
                    }
                    else
                    {
                        sinkNode.Distance = newSinkNodePriority;
                        nodesQ.Enqueue(sinkNode, reverseMode);
                    }
                }
                else
                {
                    if (sinkNode.ReversedQueueIndex != null)
                    {
                        nodesQ.ChangePriority((int)sinkNode.ReversedQueueIndex,
                            newSinkNodePriority, reverseMode);
                    }
                    else
                    {
                        sinkNode.Distance = newSinkNodePriority;
                        nodesQ.Enqueue(sinkNode, reverseMode);
                    }
                }
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


        public static (Dictionary<long, List<(Node, long)>>, Dictionary<long, List<(Node, long)>>)
            MakeBidirectionalGraph(long nodeCount, Node[] nodes, Node[] reversedNodes, long[][] edges,
            Dictionary<long, List<(Node, long)>> directGraph, Dictionary<long, List<(Node, long)>> reversedGraph)
        {
            foreach (var edge in edges)
            {
                directGraph[edge[0]].Add((nodes[edge[1] - 1], edge[2]));
                reversedGraph[edge[1]].Add((reversedNodes[edge[0] - 1], edge[2]));
            }

            return (directGraph, reversedGraph);
        }

    }
}
