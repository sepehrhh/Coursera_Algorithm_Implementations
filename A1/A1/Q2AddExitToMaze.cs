using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace A12
{
    public class Q2AddExitToMaze : Processor
    {
        public Q2AddExitToMaze(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
        {
            var adjacencyList = new List<List<long>>((int)nodeCount);
            for (int i = 0; i < nodeCount; i++)
                adjacencyList.Add(new List<long>());
            foreach (var edge in edges)
            {
                adjacencyList[(int)edge[0] - 1].Add(edge[1] - 1);
                adjacencyList[(int)edge[1] - 1].Add(edge[0] - 1);
            }
            
            return DFS(adjacencyList, nodeCount) - 1;
        }

        private int DFS(List<List<long>> adjacencyList, long nodeCount)
        {
            Stack<long> s = new Stack<long>();
            var visited = new bool[nodeCount];
            var CCNum = new int[nodeCount];
            int CC = 1;

            for (int i=0; i < adjacencyList.Count; i++)
            {
                if (!visited[i])
                {
                    CC++;
                    s.Push(i);
                    while (s.Count > 0)
                    {
                        var current = s.Pop();
                        CCNum[current] = CC;
                        visited[current] = true;

                        foreach (var v in adjacencyList[(int)current])
                            if (!visited[v])
                                s.Push(v);
                    }
                }
            }

            return CC;
        }
    }
}
