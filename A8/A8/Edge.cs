using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A8
{
    public class Edge
    {
        public long Start;
        public long End;
        public long Capacity;
        public long Flow;
        public Edge ReturnEdge = null;
        public int ID;

        public Edge(long start, long end, long capacity, int id)
        {
            this.Start = start;
            this.End = end;
            this.Capacity = capacity;
            this.Flow = 0;
            this.ID = id;
        }

        
    }
}
