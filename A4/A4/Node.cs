using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2
{
    public class Node
    {
        public Node Prev { get; set; }
        public long Distance { get; set; }
        public long Data { get; set; }
        public Nullable<long> QueueIndex { get; set; }
        public Nullable<long> ReversedQueueIndex { get; set; }
        public long[] Coordinates { get; }
        public double FScore { get; set; } //F is the total cost of the node.
        public double GScore { get; set; } //G is the distance between the current node and the start node.
        public double HScore { get; set; } //H is the heuristic — estimated distance from the current node to the end node.
        //public long FScoreReverse { get; set; } //F is the total cost of the node.
        //public long GScoreReverse { get; set; } //G is the distance between the current node and the start node.
        //public long HScoreReverse { get; set; } //H is the heuristic — estimated distance from the current node to the end node.


        public Node(long data, /*long dist,*/ long[] coordinates)
        {
            //Distance = dist;
            Data = data;
            Coordinates = coordinates;
            QueueIndex = null;
            ReversedQueueIndex = null;
        }

        //public Node(long data, long dist, Node prev)
        //{
        //    Distance = dist;
        //    Data = data;
        //    Prev = prev;
            
        //}

        //public Node(int data)
        //{
        //    Data = data;
        //}
    }
}
