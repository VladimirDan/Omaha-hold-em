using System;

namespace Code.Data
{
    public class CircularLinkedList<T>
    {
        public class CircularLinkedListNode
        {
            public T Data { get; set; }
            public CircularLinkedListNode Next { get; set; }

            public CircularLinkedListNode(T data)
            {
                Data = data;
                Next = null;
            }
        }

        private CircularLinkedListNode head;
        private CircularLinkedListNode tail;
        private int count;

        public int Count => count;

        public void Add(T data)
        {
            CircularLinkedListNode newCircularLinkedListNode = new CircularLinkedListNode(data);

            if (head == null)
            {
                head = newCircularLinkedListNode;
                tail = newCircularLinkedListNode;
                tail.Next = head;
            }
            else
            {
                tail.Next = newCircularLinkedListNode;
                tail = newCircularLinkedListNode;
                tail.Next = head;
            }

            count++;
        }

        public bool Remove(T data)
        {
            if (head == null)
                return false;

            CircularLinkedListNode current = head;
            CircularLinkedListNode previous = tail;

            do
            {
                if (current.Data.Equals(data))
                {
                    if (current == head)
                    {
                        head = head.Next;
                        tail.Next = head;
                    }
                    else if (current == tail)
                    {
                        tail = previous;
                        tail.Next = head;
                    }
                    else
                    {
                        previous.Next = current.Next;
                    }

                    count--;
                    return true;
                }

                previous = current;
                current = current.Next;
            } while (current != head);

            return false;
        }

        public T GetNext(ref CircularLinkedListNode current)
        {
            if (current == null)
                throw new InvalidOperationException("Итерация не инициализирована.");

            T data = current.Data;
            current = current.Next;
            return data;
        }

        public CircularLinkedListNode GetIterator() => head;
    }
}