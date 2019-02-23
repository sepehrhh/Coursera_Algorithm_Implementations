using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A12
{
    public class Q1MazeExit : Processor
    {
        public Q1MazeExit(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, long, long>)Solve);

        public long Solve(long nodeCount, long[][] edges, long StartNode, long EndNode)
        {
            var disjointSet = new List<List<long>>((int)nodeCount);
            for (int i = 1; i <= nodeCount; i++)
                disjointSet.Add(new List<long> { i });
            foreach (var edge in edges)
            {
                if (!disjointSet.Exists(x => x.Contains(edge[0]) && x.Contains(edge[1])))
                {
                    var set1 = disjointSet.Find(x => x.Contains(edge[0]));
                    var set2 = disjointSet.Find(x => x.Contains(edge[1]));
                    var newSet = set1.Concat(set2).ToList();
                    disjointSet.Remove(set1);
                    disjointSet.Remove(set2);
                    disjointSet.Add(newSet);
                }
            }

            return disjointSet.Find(x => x.Contains(StartNode)) == disjointSet.Find(x => x.Contains(EndNode)) ? 1 : 0;
        }    
     }
}
