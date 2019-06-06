using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A8
{
    public class Q1Evaquating : Processor
    {
        public Q1Evaquating(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long>)Solve);

        public virtual long Solve(long nodeCount, long edgeCount, long[][] edges)
        {
            var source = 1;
            var sink = (int)nodeCount;
            var nodes = new long[nodeCount];
            var network = new Dictionary<long, List<Edge>>();

            for (int i = 0; i < nodeCount; i++)
            {
                nodes[i] = i + 1;
                network.Add(i + 1, new List<Edge>());
            }

            BuildNetwork((int)nodeCount, network, nodes, edges);
            return CalculateMaxFlow(nodeCount, source, sink, network);
        }

        private long CalculateMaxFlow(long nodeCount, int source, int sink,
            Dictionary<long, List<Edge>> network)
        {
            var parent = new (long, long)[nodeCount];
            long maxFlow = 0;

            while (true)
            {
                
                if (!BFS(network, nodeCount, source, sink, parent))
                    return maxFlow;
                var pathFlow = int.MaxValue;
                for (long v = sink - 1; v != source - 1; v = parent[v].Item1)
                {
                    var u = parent[v].Item1;
                    pathFlow = Math.Min(pathFlow, 
                        (int)network[u + 1].Find(x => x.ID == parent[v].Item2).Capacity);
                }

                for (long v = sink - 1; v != source - 1; v = parent[v].Item1)
                {
                    var u = parent[v].Item1;
                    var forwardEdge = network[u + 1].Find(x => x.ID == parent[v].Item2);
                    forwardEdge.Capacity -= pathFlow;
                    forwardEdge.ReturnEdge.Capacity += pathFlow;
                }

                maxFlow += pathFlow;
            }
        }

        private bool BFS(Dictionary<long, List<Edge>> network,
            long nodeCount, int source, int sink, (long, long)[] parent)
        {
            bool[] visited = new bool[nodeCount];
            List<long> queue = new List<long>() { source };
            visited[source - 1] = true;
            parent[source - 1] = (-1, -1);

            while (queue.Count != 0)
            {
                var current = queue[0];
                queue.RemoveAt(0);

                foreach (var edge in network[current])
                {
                    if (!visited[edge.End - 1] && edge.Capacity > 0)
                    {
                        queue.Add(edge.End);
                        parent[edge.End - 1] = (current - 1, edge.ID);
                        visited[edge.End - 1] = true;
                    }
                }
            }

            return (visited[sink - 1] == true);
        }      

        public static void BuildNetwork(int nodeCount, Dictionary<long, List<Edge>> network,
            long[] nodes,
            long[][] edges)
        {
            var idGenerator = 0;
            foreach (var edge in edges)
            {
                var start = nodes[edge[0] - 1];
                var end = nodes[edge[1] - 1];
                var capacity = edge[2];
                var newEdge = new Edge(start, end, capacity, idGenerator++);
                var retEdge = new Edge(end, start, 0, idGenerator++);
                newEdge.ReturnEdge = retEdge;
                retEdge.ReturnEdge = newEdge;
                network[edge[0]].Add(newEdge);
                network[edge[1]].Add(retEdge);
            }
        }
    }
}
