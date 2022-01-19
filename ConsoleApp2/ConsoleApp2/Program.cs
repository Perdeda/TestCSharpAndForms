using System;

namespace ConsoleApp2
{
    class ListNode<T> {
        public readonly T Value;
        public readonly ListNode<T> Next;

        public ListNode(T value, ListNode<T> next)
        {
            Value = value;
            Next = next;
        }

        public void ChangeElement(ListNode<T> node,ListNode<T> head)
        {
            ListNode<T> n = head; 
            int i = 0, j = 0;
            while(n != null)
            {
                if(n.Value.ToString() == node.Value.ToString())
                {
                    j = i;
                }
                i++;
                n = n.Next;
            }
            n = head;
            //При замене элемента по-идее в функцию должен приходить элемент с уже сделанной ссылкой на next, ибо значения можно задать только в конструкторе
            for (int i1 = 0;i1 < i; i++)
            {
                ListNode<T> tmp;
                if (i1 != j - 1)
                {
                    tmp = new ListNode<T>(n.Value, n.Next);
                }
                else
                {
                    tmp = new ListNode<T>(n.Value, node);
                }
            }
        }
        public void LinkTwoLists(ListNode<T> head1,ListNode<T> head2)
        {
            ListNode<T> n = head1;
            
            while (n != null)
            {
                ListNode<T> tmp;
                if (n.Next != null)
                {
                    tmp = new ListNode<T>(n.Value, n.Next);
                }
                else
                {
                    tmp = new ListNode<T>(n.Value,head2);
                }
                n = n.Next;
            }
        }
  }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
