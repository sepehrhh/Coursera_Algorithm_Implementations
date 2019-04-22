using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using A2;
using A3;

namespace Exam1
{
    public class Q1Betweenness : Processor
    {
        public Q1Betweenness(string testDataName) : base(testDataName)
        {
            this.ExcludeTestCaseRangeInclusive(15, 50);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long[]>)Solve);


        public long[] Solve(long NodeCount, long[][] edges)
        {
            var result = new long[(int)NodeCount];
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
                result[i] = 0;
            }

            var graph = MakeBidirectionalGraph(NodeCount, nodes, reversedNodes, edges, directGraph, reversedGraph);

            for (int i = 0; i < NodeCount; i++)
            {
                for (int k = 0; k < NodeCount; k++)
                {
                    var startNode = nodes[i].Data;
                    var targetNode = nodes[k].Data;

                    if (startNode == targetNode)
                        continue;

                    if (graph.Item1[startNode].Count == 0 || graph.Item2[targetNode].Count == 0)
                        continue;

                    for (int j = 0; j < NodeCount; j++)
                    {
                        nodes[j].Distance = int.MaxValue;
                        reversedNodes[j].Distance = int.MaxValue;
                        nodes[j].QueueIndex = null;
                        nodes[j].ReversedQueueIndex = null;
                        reversedNodes[j].QueueIndex = null;
                        reversedNodes[j].ReversedQueueIndex = null;
                    }

                    var SP = BidirectionalDijkstra(nodes, reversedNodes, graph.Item1, graph.Item2, startNode, targetNode,
                        edges, NodeCount, ref result);
                }
            }

            return result;
        }

        private long BidirectionalDijkstra(Node[] nodes, Node[] reversedNodes,
           Dictionary<long, List<(Node, long)>> graph,
           Dictionary<long, List<(Node, long)>> reversedGraph,
           long startNode, long targetNode,
           long[][] edges, long NodeCount, ref long[] result)
        {
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
                        reversedNodes, processedNodes, reversedNodesProcessed,
                        ref result) : -1;

                var currentReverseNode = reversedNodesQ.ExtractPeek(true);

                ProcessNode(currentReverseNode, reversedGraph, reversedNodes,
                    reversedNodesProcessed, reversedNodesQ, true);

                if (processedNodes.Contains(currentReverseNode.Data))
                    return currentNode.Distance != int.MaxValue ? ShortestPath(startNode, targetNode, nodes,
                        reversedNodes, processedNodes, reversedNodesProcessed,
                        ref result) : -1;

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


        private void Relax(Node startNode, Node sinkNode, long weight,
            FastPriorityQueue nodesQ, bool reverseMode)
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
            List<long> reverseProcessedNodes, ref long[] result)
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

            var last = best;

            while (last != startNode)
            {
                result[last - 1]++;
                last = nodes[last - 1].Prev.Data;
            }

            last = best;

            while (last != targetNode)
            {
                
                last = reversedNodes[last - 1].Prev.Data;
                if (last != targetNode)
                    result[last - 1]++;
            }

            return distance;
        }


        public static (Dictionary<long, List<(Node, long)>>, Dictionary<long, List<(Node, long)>>)
            MakeBidirectionalGraph(long nodeCount, Node[] nodes, Node[] reversedNodes, long[][] edges,
            Dictionary<long, List<(Node, long)>> directGraph, Dictionary<long, List<(Node, long)>> reversedGraph)
        {
            foreach (var edge in edges)
            {
                directGraph[edge[0]].Add((nodes[edge[1] - 1], 1));
                reversedGraph[edge[1]].Add((reversedNodes[edge[0] - 1], 1));
            }

            return (directGraph, reversedGraph);
        }
    }
}
