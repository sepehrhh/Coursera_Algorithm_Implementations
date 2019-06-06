using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A8
{
    public class Q2Airlines : Processor
    {
        public Q2Airlines(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long[]>)Solve);

        public virtual long[] Solve(long flightCount, long crewCount, long[][] info)
        {
            var source = 1;
            var nodeCount = flightCount + crewCount + 2;
            var sink = (int)nodeCount;
            var nodes = new long[nodeCount];
            var network = new Dictionary<long, List<Edge>>();

            for (int i = 0; i < nodeCount; i++)
            {
                nodes[i] = i + 1;
                network.Add(i + 1, new List<Edge>());
            }

            BuildNetwork((int)flightCount, (int)crewCount, source, (int)sink, network, nodes, info);
            CalculateMaxFlow(flightCount, crewCount, nodeCount, source, sink, network);

            var matching = new long[flightCount];
            for (int i = 0; i < matching.Length; i++)
                matching[i] = -1;


            var flights = network.Skip(1).Take((int)flightCount);

            foreach (var p in flights)
                foreach (var edge in p.Value)
                    if (edge.Flow == 1 && edge.End < nodeCount && edge.End > flightCount + 1)
                        matching[edge.Start - 2] = edge.End - flightCount - 1;

            return matching;
        }

        private void CalculateMaxFlow(long flightCount, long crewCount,
            long nodeCount, int source, int sink,
           Dictionary<long, List<Edge>> network)
        {
            var parent = new long[nodeCount];
            long maxFlow = 0;
            var matchingCount = flightCount;
            

            while (true)
            {

                if (!BFS(network, nodeCount, source, sink, parent))
                    return;
                var pathFlow = int.MaxValue;
                for (long v = sink - 1; v != source - 1; v = parent[v])
                {
                    var u = parent[v];
                    pathFlow = Math.Min(pathFlow,
                        (int)network[u + 1].Find(x => x.End == v + 1).Capacity);
                }

                for (long v = sink - 1; v != source - 1; v = parent[v])
                {
                    var u = parent[v];
                    var forwardEdge = network[u + 1].Find(x => x.End == v + 1);
                    forwardEdge.Capacity -= pathFlow;
                    forwardEdge.Flow += pathFlow;

                    //if (forwardEdge.Flow == 1 && forwardEdge.Start < crewCount && forwardEdge.Start > 1 &&
                    //    forwardEdge.End < nodeCount && forwardEdge.End > flightCount + 1)
                    //    matching[forwardEdge.Start - 2] = forwardEdge.End - flightCount - 1;

                    forwardEdge.ReturnEdge.Capacity += pathFlow;
                    forwardEdge.ReturnEdge.Flow -= pathFlow;

                    //if (forwardEdge.ReturnEdge.Flow == 1 && forwardEdge.ReturnEdge.Start < crewCount && forwardEdge.ReturnEdge.Start > 1 &&
                    //    forwardEdge.ReturnEdge.End < nodeCount && forwardEdge.ReturnEdge.End > flightCount + 1)
                    //    matching[forwardEdge.ReturnEdge.Start - 2] = forwardEdge.ReturnEdge.End - flightCount - 1;

                }
                maxFlow += pathFlow;
            }
        }

        private bool BFS(Dictionary<long, List<Edge>> network,
            long nodeCount, int source, int sink, long[] parent)
        {
            bool[] visited = new bool[nodeCount];
            List<long> queue = new List<long>() { source };
            visited[source - 1] = true;
            parent[source - 1] = -1;

            while (queue.Count != 0)
            {
                var current = queue[0];
                queue.RemoveAt(0);

                foreach (var edge in network[current])
                {
                    if (!visited[edge.End - 1] && edge.Capacity > 0)
                    {
                        queue.Add(edge.End);
                        parent[edge.End - 1] = current - 1;
                        visited[edge.End - 1] = true;
                    }
                }
            }

            return (visited[sink - 1] == true);
        }

        public static void BuildNetwork(int flightCount, int crewCount, int source, int sink,
            Dictionary<long, List<Edge>> network,
            long[] nodes,
            long[][] info)
        {
            var idGenerator = 0;
            for (int i = 0; i < flightCount; i++)
            {
                var end = i + 2;
                var capacity = 1;
                var newEdge = new Edge(source, end, capacity, idGenerator++);
                var retEdge = new Edge(end, source, 0, idGenerator++);
                newEdge.ReturnEdge = retEdge;
                retEdge.ReturnEdge = newEdge;
                network[source].Add(newEdge);
                network[end].Add(retEdge);
            }

            for (int i = flightCount; i < crewCount + flightCount; i++)
            {
                var start = i + 2;
                var capacity = 1;
                var newEdge = new Edge(start, sink, capacity, idGenerator++);
                var retEdge = new Edge(sink, start, 0, idGenerator++);
                newEdge.ReturnEdge = retEdge;
                retEdge.ReturnEdge = newEdge;
                network[start].Add(newEdge);
                network[sink].Add(retEdge);
            }

            for (int i = 0; i < flightCount; i++)
                for (int j = 0; j < crewCount; j++)
                {
                    if (info[i][j] == 1)
                    {
                        var start = i + 2;
                        var end = j + flightCount + 2;
                        var capacity = 1;
                        var newEdge = new Edge(start, end, capacity, idGenerator++);
                        var retEdge = new Edge(end, start, 0, idGenerator++);
                        newEdge.ReturnEdge = retEdge;
                        retEdge.ReturnEdge = newEdge;
                        network[start].Add(newEdge);
                        network[end].Add(retEdge);
                    }
                }
        }
    }
}
