namespace test
{
    public static class e
    {
        static public int n, m;
        public static string c(this int x, int y)
        {
            return x + "-" + y;
        }
    }
    class x
    {
        static public void xx()
        {
            e.m = 1;
        }
    }

    class A
    {
        public virtual void xx()
        {
            Console.WriteLine(0);
        }
    }

    class B: A
    {
        public override void xx()
        {
            Console.WriteLine(1);
        }
    }

    class C : A
    {
        public override void xx()
        {
            Console.WriteLine(2);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<A> list = new List<A>();

            list.Add(new B());
            list.Add(new C());

            foreach (A a in list)
            {
                a.xx();
            }
        }
    }
}