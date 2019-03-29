using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue;
using A2;

namespace A3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FastPriorityQueue fastPriorityQueue = new FastPriorityQueue(5);
            fastPriorityQueue.Enqueue(new Node(3, 0));
            fastPriorityQueue.Enqueue(new Node(4, 12));
            fastPriorityQueue.Enqueue(new Node(2, 2));
            fastPriorityQueue.Enqueue(new Node(1, 3));
            fastPriorityQueue.ChangePriority(0, 2);
            fastPriorityQueue.Dequeue(3);
            fastPriorityQueue.ExtractPeek();
        }
    }
}
