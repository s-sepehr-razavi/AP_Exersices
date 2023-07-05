using System;
namespace _1
{
    internal class Program
    {
       
        static void Main(string[] args)
        {
            long help = long.Parse(Console.ReadLine());
            string[] strings = Console.ReadLine().Split();
            long c1 = changer(strings[0], help);
            long c2 = changer(strings[1], help);
            long g = gcd(c1, c2);
            Console.WriteLine(g + c1 * c2 / g);
        }

        static public long gcd(long a, long b)
        {
            if (b > a)
            {
                long h = a;
                a = b;
                b = h;
            }

            long d = b;
            long x = a;
            long y = b;
            
            while (a % d != 0 || b % d != 0)
            {
                d = x % d;
                x = y;
                y = d;
            }

            return d;
        }

        static public long changer(string word, long help)
        {
            long d = 0;
            for (int i = 0; i < word.Length; i++)
            {
                d += word[i] * (long)Math.Pow((double)help, (double)i);
            }

            return d;
        }

    }
}