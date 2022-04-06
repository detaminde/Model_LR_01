using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_LR_01
{
    public class Node<T>
    {
        public Node(T data)
        {
            Data = data;
        }
        public T Data { get; set; }
        public Node<T> next { get; set; }
    }

    public class MyStack<T> : IEnumerable<T>
    {
        Node<T> head;
        int count;

        public MyStack()
        {
            count = 0;
        }
        public void Push(T data)
        {
            Node<T> node = new Node<T>(data);
            node.next = head;
            head = node;
            count++;
        }
        public int Count { get { return count; } }
        public bool IsEmpty
        {
            get { return count == 0; }
        }
        public T Pop()
        {
            if(IsEmpty)
            {
                throw new InvalidOperationException("Stack is empty");
            }
            Node<T> current_head = head;
            head = head.next;
            count--;
            return current_head.Data;
        }
        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Stack is empty");
            return head.Data;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)((IEnumerable)this).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            Node<T> current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.next;
            }
        }
    }
}
