using System.Text.Json;
using System.Text.Json.Serialization;

namespace test
{
    struct s1
    {
        public int id { set; get; }
        //public List<string> values { get; } = new List<string>();
        public Dictionary<string, int> values = new Dictionary<string, int>();
        public static List<s1> l = new List<s1>();

        public s1(int id)
        {
            this.id = id;
            l.Add(this);
        }
    }

        internal class Program
    {

        static void Main(string[] args)
        {

            //int m = 19;
            //for (int i = 2; i < m; i++)
            //{
            //    Console.WriteLine($"{i}: {fast_exp(i, 40, m)}");

            //}
            //int S = fast_exp(11, 4, 19);
            //Console.WriteLine(S);
            //int A = fast_exp(11, 10, 19);
            //Console.WriteLine(A);
            //Console.WriteLine(fast_exp(A, 4, 19) + " " + fast_exp(S, 10, 19));
            //for (int i = 0; i < 25; i++)
            //{
            //    Console.WriteLine(i);
            //    Console.WriteLine((char)('a' + i) + ": " + fast_exp(i, 11, 26));
            //}

            while (true)
            {
                try
                {
                    int x = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("wrong mate");
                }
            }

        }

        static int fast_exp(int b, int e, int m)
        {
            int ans = 1;
            for (int i = 1; i <= e; i++)
            {
                ans *= b;
                //Console.WriteLine($"ans before m: {ans}");
                ans %= m;
                //Console.WriteLine($"ans after m: {ans}");
            }
            return ans;
        }

        //static public void change_s1(s1 s)
        //{
        //    s.id = 2;
        //    s.values.Add("ss",2);
        //    s.values = new Dictionary<string, int>();
        //}



    }
}