using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A4
{
    public class DisjointSet
    {

        private long Count { get; set; }
        public readonly long[] Parent;
        private readonly long[] Rank;
        public long[] Cluster;

        public DisjointSet(long count)
        {
            Count = count;
            Parent = new long[Count];
            Rank = new long[Count];
            Cluster = new long[Count];
        }

        public void MakeSet(long i)
        {
            Parent[i] = i;
            Rank[i] = 0;
            Cluster[i] = i;
        }

        public long Find(long i)
        {
            if (Parent[i] == i)
                return i;
            else
                return Find(Parent[i]);
        }

        public void Union(long source, long target)
        {
            var sourceParent = Find(source);
            var targetParent = Find(target);

            if (sourceParent == targetParent)
                return;

            Parent[sourceParent] = targetParent;
        }

        public void UnionByRank(long i, long j)
        {
            var iParent = Find(i);
            var jParent = Find(j);

            if (iParent == jParent)
                return;

            if (Rank[iParent] > Rank[jParent])
            {
                Parent[jParent] = iParent;
            }
            else
            {
                Parent[iParent] = jParent;
                if (Rank[iParent] == Rank[jParent])
                    Rank[jParent]++;
            }

        }

        //public void Clustering(long i)
        //{
        //    if (Parent[i] == i)
        //        return i;
        //    else
        //        return Find(Parent[i]);
        //}

    }
}
