using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2
{
    public class Node
    {
        public long Distance { get; set; }
        public long Data { get; set; }
        public Nullable<bool> Color { get; set; }

        public Node(long data, long dist)
        {
            Distance = dist;
            Data = data;
        }

        public Node(int data)
        {
            Data = data;
        }
    }
}
