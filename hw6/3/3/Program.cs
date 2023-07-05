using System.Collections;

namespace _3
{
    class MyQueue<T>: IEnumerable
    {
        public int size { get; private set; }
        public List<T> values = new List<T>();

        public void Enqueue(T item)
        {
            values.Add(item);
            size++;
        }

        public T Dequeue()
        {
            T item = values[0];
            values.RemoveAt(0);
            size--;
            return item;
        }

        public string print()
        {
            string x = "";

            T s;
            for (int i = size - 1; i > - 1; i--)
            {
                s = this.Dequeue();                
                x += s + " ";
                this.Enqueue(s);
            }

            return x;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            //T x;
            //for (int i = 0; i < size; i++)
            //{
            //    x = this.Dequeue();
            //    this.Enqueue(x);
            //    yield return x;      /////////////          

            //}            
            return values.GetEnumerator();
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            MyQueue<int> myQueue = new MyQueue<int>();
            for (int i = 0; i < 21; i++)
            {
                myQueue.Enqueue(i);
            }

            //Console.WriteLine(myQueue.size);
            //Console.WriteLine(myQueue.print());
            //Console.WriteLine(myQueue.size);
            //Console.WriteLine(myQueue.print());
            foreach (var item in myQueue)
            {
                Console.WriteLine(item);
                
            }

            foreach (var item in myQueue)
            {
                Console.WriteLine(item);
            }
            //List<int> x = new List<int>();
            //x.Add(1);
            //x.Add(2);
            //x.Add(3);
            //x.Add(4);
            //x.RemoveAt(0);
            //foreach (int i in x)
            //{
            //    Console.WriteLine(i);
            //}
        }
    }
}