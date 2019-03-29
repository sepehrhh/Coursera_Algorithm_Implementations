using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A2;

namespace A3
{
    class FastPriorityQueue
    {
        public List<Node> Queue { get; set; }
        public int MaxSize { get; }
        public int Size { get { return Queue.Count; } }
        public bool[] IsInQueue { get; set; }
        

        public FastPriorityQueue(int maxSize)
        {
            MaxSize = maxSize;
            Queue = new List<Node>(MaxSize);
            IsInQueue = new bool[MaxSize];
        }

        public void Enqueue(Node node)
        {
            if (Queue.Capacity == Queue.Count)
                throw new Exception("ERROR");
            Queue.Add(node);
            IsInQueue[node.Data - 1] = true;
            if (!node.ReverseMode)
                node.QueueIndex = Size - 1;
            else
                node.ReversedQueueIndex = Size - 1;
            SiftUp(Size - 1);
        }

        public int Parent(int i) => (i - 1) / 2;

        public int LeftChild(int i) => 2 * i + 1;

        public int RightChild(int i) => 2 * i + 2;

        public void SiftUp(int i)
        {
            while (i > 0 && Queue[Parent(i)].Distance >= Queue[i].Distance)
            {
                if (!Queue[i].ReverseMode)
                {
                    Queue[i].QueueIndex = Parent(i);
                    Queue[Parent(i)].QueueIndex = i;
                }
                else
                {
                    Queue[i].ReversedQueueIndex = Parent(i);
                    Queue[Parent(i)].ReversedQueueIndex = i;
                }
                (Queue[i], Queue[Parent(i)]) =
                    (Queue[Parent(i)], Queue[i]);
                i = Parent(i);

            }
        }

        public void SiftDown(int i)
        {
            var maxIndex = i;
            var l = LeftChild(i);
            if (l <= Size - 1 && Queue[l].Distance < Queue[maxIndex].Distance)
                maxIndex = l;
            var r = RightChild(i);
            if (r <= Size - 1 && Queue[r].Distance < Queue[maxIndex].Distance)
                maxIndex = r;
            if (i != maxIndex)
            {
                if (!Queue[i].ReverseMode)
                {
                    Queue[i].QueueIndex = Parent(i);
                    Queue[maxIndex].QueueIndex = i;
                }
                else
                {
                    Queue[i].ReversedQueueIndex = Parent(i);
                    Queue[maxIndex].ReversedQueueIndex = i;
                }
                (Queue[i], Queue[maxIndex]) =
                    (Queue[maxIndex], Queue[i]);
                SiftDown(maxIndex);
            }
        }

        public Node ExtractPeek()
        {
            var result = Queue[0];
            Queue[0] = Queue[Size - 1];
            SiftDown(0);
            Queue.Remove(Queue.Last());
            IsInQueue[result.Data - 1] = false;
            return result;
        }

        public void Dequeue(int i)
        {
            Queue[i].Distance = long.MaxValue;
            SiftDown(i);
            Queue.Remove(Queue.Last());
        }

        public void ChangePriority(int i, int priority)
        {
            var oldPriority = Queue[i].Distance;
            Queue[i].Distance = priority;
            if (priority < oldPriority)
                SiftUp(i);
            else
                SiftDown(i);
        }
    }
}
