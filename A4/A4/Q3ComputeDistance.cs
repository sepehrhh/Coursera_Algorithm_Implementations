using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using A2;
using A3;

namespace A4
{
    public class Q3ComputeDistance : Processor
    {
        public Q3ComputeDistance(string testDataName) : base(testDataName)
        {
            //this.ExcludeTestCaseRangeInclusive(1, 39);
            //this.ExcludeTestCaseRangeInclusive(16, 50);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long,long, long[][], long[][], long, long[][], long[]>)Solve);


        public long[] Solve(long NodeCount, 
                            long EdgeCount,
                            long[][] Points,
                            long[][] Edges,
                            long QueriesCount,
                            long[][] Queries)
        {
            var result = new long[(int)QueriesCount];
            var nodes = new Node[NodeCount];
            var directGraph = new Dictionary<long, List<(Node, long)>>();

            for (int i = 0; i < NodeCount; i++)
            {
                nodes[i] = new Node(i + 1, Points[i]);
                directGraph.Add(i + 1, new List<(Node, long)>());
            }

            var graph = MakeGraph(NodeCount, nodes, Edges, directGraph);

            for (int i = 0; i < QueriesCount; i++)
            {
                var startNode = Queries[i][0];
                var targetNode = Queries[i][1];

                for (int j = 0; j < NodeCount; j++)
                {
                    nodes[j].GScore = int.MaxValue;
                    nodes[j].Distance = int.MaxValue;
                    nodes[j].HScore = ComputeHScore(nodes[j].Coordinates, nodes[targetNode - 1].Coordinates);
                    nodes[j].FScore = int.MaxValue;
                    nodes[j].QueueIndex = null;
                }

                result[i] = AStar(nodes, graph, startNode, targetNode,
                    Edges, NodeCount, EdgeCount);
            }

            return result;
        }

        private long AStar(Node[] nodes, Dictionary<long, List<(Node, long)>> graph,
            long startNode, long targetNode,
            long[][] edges, long NodeCount, long EdgeCount)
        {
            if (startNode == targetNode)
                return 0;

            if (graph[startNode].Count == 0)
                return -1;

            nodes[startNode - 1].Distance = 0;
            nodes[startNode - 1].GScore = 0;
            nodes[startNode - 1].FScore = nodes[startNode - 1].GScore +
                nodes[startNode - 1].HScore;

            var nodesQ = new FastPriorityQueue((int)NodeCount);
            nodesQ.Enqueue(nodes[startNode - 1]);

            while (nodesQ.Size > 0)
            {
                var currentNode = nodesQ.ExtractPeek();
                ProcessNode(currentNode, graph, nodes, targetNode, nodesQ);
            }

            return nodes[(int)targetNode - 1].Distance != int.MaxValue ?
                nodes[(int)targetNode - 1].Distance : -1;
        }


        private void ProcessNode(Node currentNode,
            Dictionary<long, List<(Node, long)>> graph,
            Node[] nodes, long targetData, FastPriorityQueue nodesQ)
        {
            var targetNode = nodes[targetData - 1];
            foreach (var edge in graph[currentNode.Data])
                Relax(currentNode, edge.Item1, targetNode, edge.Item2, nodesQ);
        }


        private void Relax(Node startNode, Node sinkNode, Node targetNode, long weight,
            FastPriorityQueue nodesQ)
        {
            if (sinkNode.GScore > startNode.GScore + weight)
            {
                sinkNode.Prev = startNode;
                sinkNode.GScore = startNode.GScore + weight;
                sinkNode.Distance = startNode.Distance + weight;
                var newSinkNodePriority = sinkNode.GScore + sinkNode.HScore;
                if (sinkNode.QueueIndex != null)
                {
                    nodesQ.ChangePriority((int)sinkNode.QueueIndex,
                        newSinkNodePriority);
                }
                else
                {
                    sinkNode.FScore = newSinkNodePriority;
                    nodesQ.Enqueue(sinkNode);
                }
            }
        }

        private double ComputeHScore(long[] currentNodeCoordinates, long[] targetNodeCoordinates)
        {
            return Math.Sqrt( ( (targetNodeCoordinates[0] - currentNodeCoordinates[0]) * 
                (targetNodeCoordinates[0] - currentNodeCoordinates[0]) ) +
                ( (targetNodeCoordinates[1] - currentNodeCoordinates[1]) * 
                (targetNodeCoordinates[1] - currentNodeCoordinates[1]) ) );
        }

        public static Dictionary<long, List<(Node, long)>> 
            MakeGraph(long nodeCount, Node[] nodes, long[][] edges,
            Dictionary<long, List<(Node, long)>> directGraph)
        {
            foreach (var edge in edges)
                directGraph[edge[0]].Add((nodes[edge[1] - 1], edge[2]));

            return directGraph;
        }
    }
}
