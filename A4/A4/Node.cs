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
        public long[] Coordinates { get; }
        public long FScore { get; set; } //F is the total cost of the node.
        public long GScore { get; set; } //G is the distance between the current node and the start node.
        public long HScore { get; set; } //H is the heuristic — estimated distance from the current node to the end node.

        public Node(long data, long dist)
        {
            Distance = dist;
            Data = data;
        }

        public Node(int data)
        {
            Data = data;
        }

        public Node(long data, long[] coordinates)
        {
            Data = data;
            Coordinates = coordinates;
        }
    }
}
