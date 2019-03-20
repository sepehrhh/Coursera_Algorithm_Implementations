using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2
{
    class PriorityQueue
    {
        public Node[] MinHeap { get; set; }
        public int MaxSize { get; set; }
        public int Size { get; set; }

        public PriorityQueue(int maxSize, Node[] keys)
        {
            MaxSize = maxSize;
            Size = 0;
            MinHeap = new Node[MaxSize];
            for (int i = 0; i < keys.Length; i++)
                Insert(keys[i]);
        }

        public void Insert(Node node)
        {
            if (Size == MaxSize)
                throw new Exception("ERROR");
            MinHeap[Size] = node;
            SiftUp(Size);
            Size++;
        }

        public int Parent(int i) => (i - 1) / 2;

        public int LeftChild(int i) => 2 * i + 1;

        public int RightChild(int i) => 2 * i + 2;

        public void SiftUp(int i)
        {
            while (i > 0 && MinHeap[Parent(i)].Distance > MinHeap[i].Distance)
            {
                (MinHeap[i], MinHeap[Parent(i)]) =
                    (MinHeap[Parent(i)], MinHeap[i]);
                i = Parent(i);
            }
        }

        public void SiftDown(int i)
        {
            var maxIndex = i;
            var l = LeftChild(i);
            if (l <= Size && MinHeap[l].Distance < MinHeap[maxIndex].Distance)
                maxIndex = l;
            var r = RightChild(i);
            if (r <= Size && MinHeap[r].Distance < MinHeap[maxIndex].Distance)
                maxIndex = r;
            if (i != maxIndex)
            {
                (MinHeap[i], MinHeap[maxIndex]) =
                    (MinHeap[maxIndex], MinHeap[i]);
                SiftDown(maxIndex);
            }
        }

        public Node ExtractMin()
        {
            var result = MinHeap[0];
            MinHeap[0] = MinHeap[Size - 1];
            Size--;
            SiftDown(0);
            return result;
        }

        public void Remove(int i)
        {
            MinHeap[i].Distance = long.MaxValue;
            SiftUp(i);
            ExtractMin();
        }

        public void ChangePriority(int i, int priority)
        {
            var oldPriority = MinHeap[i].Distance;
            MinHeap[i].Distance = priority;
            if (priority < oldPriority)
                SiftUp(i);
            else
                SiftDown(i);
        }
    }
}
