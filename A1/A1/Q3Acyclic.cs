using System;
using System.Collections.Generic;
using TestCommon;

namespace A12
{
    public class Q3Acyclic : Processor
    {
        public Q3Acyclic(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);


        public long Solve(long nodeCount, long[][] edges)
        {
            var adjacencyList = new List<List<long>>((int)nodeCount);
            for (int i = 0; i < nodeCount; i++)
                adjacencyList.Add(new List<long>());
            var visited = new bool[nodeCount];
            var keepTrack = new bool[nodeCount];
            foreach (var edge in edges)
                adjacencyList[(int)edge[0] - 1].Add(edge[1] - 1);
            for (int i = 0; i < adjacencyList.Count; i++)
                if (IsAcyclic(adjacencyList, visited, keepTrack, i))
                    return 1;
            return 0;
        }

        private bool IsAcyclic(List<List<long>> adjacencyList, bool[] visited,
            bool[] keepTrack, int i)
        {
            if (keepTrack[i])
                return true;
            if (visited[i])
                return false;
            visited[i] = true;
            keepTrack[i] = true;
            var children = adjacencyList[i];
            for (int j = 0; j < children.Count; j++)
                if (IsAcyclic(adjacencyList, visited, keepTrack, (int)children[j]))
                    return true;
            keepTrack[i] = false;
            return false;
        }
    }

}