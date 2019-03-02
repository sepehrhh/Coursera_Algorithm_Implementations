using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A2
{
    public class Q2BipartiteGraph : Processor
    {
        public Q2BipartiteGraph(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);


        public long Solve(long NodeCount, long[][] edges)
        {
            var nodes = new Nullable<bool>[NodeCount + 1];
            nodes[1] = false;
            var queue = new Queue<long>();
            queue.Enqueue(1);
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                for (int i = 0; i < edges.Length; i++)
                    if (edges[i].Count(x => x == currentNode) > 0)
                    {
                        var connectedNode = edges[i].Where(x =>
                        x != currentNode).First();
                        var nodeColor = nodes[connectedNode];
                        if (nodeColor == null)
                        {
                            nodes[connectedNode] = !nodes[currentNode];
                            queue.Enqueue(connectedNode);
                        }
                        else if (nodeColor == nodes[currentNode])
                            return 0;
                    }
            }
            return 1;
        }
    }

}
