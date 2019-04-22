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
        public int LastIndex { get { return Size - 1; } }
        private bool ReverseMode { get; set; }
        

        public FastPriorityQueue(int maxSize)
        {
            MaxSize = maxSize;
            Queue = new List<Node>(MaxSize);
        }

        public void Enqueue(Node node, bool mode)
        {
            ReverseMode = mode;
            if (Queue.Capacity == Queue.Count)
                throw new Exception("ERROR");
            Queue.Add(node);
            if (!ReverseMode)
                node.QueueIndex = LastIndex;
            else
                node.ReversedQueueIndex = LastIndex;
            SiftUp(LastIndex);
        }

        public int Parent(int i) => (i - 1) / 2;

        public int LeftChild(int i) => 2 * i + 1;

        public int RightChild(int i) => 2 * i + 2;

        public void SiftUp(int i)
        {
            while (i > 0 && Queue[Parent(i)].Distance >= Queue[i].Distance)
            {
                if (Queue[Parent(i)].Distance == Queue[i].Distance)
                    if (Queue[Parent(i)].Data > Queue[i].Data)
                        break;

                if (!ReverseMode)
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

            else
                if (l <= Size - 1 && Queue[l].Distance == Queue[maxIndex].Distance)
                    if (Queue[l].Data > Queue[maxIndex].Data)
                        maxIndex = l;

            var r = RightChild(i);

            if (r <= Size - 1 && Queue[r].Distance < Queue[maxIndex].Distance)
                maxIndex = r;
            else
                if (r <= Size - 1 && Queue[r].Distance == Queue[maxIndex].Distance)
                    if (Queue[r].Data > Queue[maxIndex].Data)
                        maxIndex = r;

            if (i != maxIndex)
            {
                (Queue[i], Queue[maxIndex]) =
                    (Queue[maxIndex], Queue[i]);
                if (!ReverseMode)
                {
                    Queue[i].QueueIndex = i;
                    Queue[maxIndex].QueueIndex = maxIndex;
                }
                else
                {
                    Queue[i].ReversedQueueIndex = i;
                    Queue[maxIndex].ReversedQueueIndex = maxIndex;
                }
                SiftDown(maxIndex);
            }
            else
            {
                if (!ReverseMode)
                    Queue[i].QueueIndex = i;
                else
                    Queue[i].ReversedQueueIndex = i;
            }
        }


        public Node ExtractPeek(bool mode)
        {
            ReverseMode = mode;
            var result = Queue[0];
            Queue[0] = Queue[LastIndex];
           
            SiftDown(0);
            Queue.RemoveAt(LastIndex);
            if (!ReverseMode)
                result.QueueIndex = null;
            else
                result.ReversedQueueIndex = null;
            return result;
        }

        public void Dequeue(int i)
        {
            Queue[i].Distance = long.MaxValue;
            SiftDown(i);
            Queue.Remove(Queue.Last());
        }


        public void ChangePriority(int i, long newPriority, bool mode)
        {
            ReverseMode = mode;
            var oldPriority = Queue[i].Distance;
            Queue[i].Distance = newPriority;
            if (newPriority < oldPriority)
                SiftUp(i);
            else
                SiftDown(i);
        }
    }
}
