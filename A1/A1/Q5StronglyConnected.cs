using System;
using System.Collections.Generic;
using TestCommon;

namespace A12
{
    public class Q5StronglyConnected: Processor
    {
        public Q5StronglyConnected(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        List<List<long>> AdjacencyList;
        List<List<long>> ReverseAdjacencyList;
        bool[] Visited;
        List<long> Order = new List<long>();
        long NodeCount;


        public long Solve(long nodeCount, long[][] edges)
        {
            NodeCount = nodeCount;
            Visited = new bool[NodeCount];
            AdjacencyList = new List<List<long>>((int)NodeCount);
            ReverseAdjacencyList = new List<List<long>>();
            for (int i = 0; i < NodeCount; i++)
            {
                AdjacencyList.Add(new List<long>());
                ReverseAdjacencyList.Add(new List<long>());
            }
            foreach (var edge in edges)
            {
                AdjacencyList[(int)edge[0] - 1].Add(edge[1] - 1);
                ReverseAdjacencyList[(int)edge[1] - 1].Add(edge[0] - 1);
            }

            return FindSCCs();
        }

        private long FindSCCs()
        {
            var SCCNum = 0;
            for (int i = 0; i < ReverseAdjacencyList.Count; i++)
                if (!Visited[i])
                    DFS(i);
            Order.Reverse();
            for (int i = 0; i < Visited.Length; i++)
                Visited[i] = false;
            for (int i = 0; i < Order.Count; i++)
                if (!Visited[Order[i]])
                {
                    Explore(Order[i]);
                    SCCNum++;
                }
            return SCCNum;
        }

        private void Explore(long v)
        {
            Visited[v] = true;
            var children = AdjacencyList[(int)v];
            for (int i = 0; i < AdjacencyList[(int)v].Count; i++)
                if (!Visited[children[i]])
                    Explore(children[i]);
        }

        private void DFS(long i)
        {
            Visited[i] = true;
            var children = ReverseAdjacencyList[(int)i];
            for (int j = 0; j < children.Count; j++)
                if (!Visited[children[j]])
                    DFS(children[j]);
            Order.Add(i);
        }
    }
}
