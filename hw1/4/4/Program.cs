using System;
using System.Collections.Generic;
namespace _4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            long[] ints = Array.ConvertAll(Console.ReadLine().Split(), s => long.Parse(s));
            long roundest = ints[0];
            //for (long i = 1; i <= ints[1]; i++)
            //{
            //    roundest = rounder_one(i * ints[0], roundest);
            //}

            //List<long> list = find_largest_nums(ints[1]);
            //list.Add(ints[0] * ints[1]);
            //foreach (var item in list)
            //{
            //    Console.WriteLine(item);
            //    roundest = rounder_one(roundest, item, true);
            //}

            roundest = diff_comb_2_5(ints[0], ints[1]);
            Console.WriteLine(roundest);
        }

        static List<int> ints = new List<int>();
        static int how_round_it_is(long x) 
        {
            int c = 0;

            try
            {
                return ints[(int)x];
            }
            catch (Exception)
            {

                
            }

            while (x % 10 == 0)
            {
                x /= 10;
                c++;
            }
            //Console.WriteLine(x);
            ints.Add(c);
            return c;
        }

        static long rounder_one(long x, long y, bool max)
        {
            int x_c = how_round_it_is(x);
            int y_c = how_round_it_is(y);

            if (x_c > y_c)
            {
                return x;
            }
            else if (y_c > x_c)
            {
                return y;
            }
            else
            {
                return  max? Math.Max(x, y) : Math.Min(x, y);
            }
        }

        static long find_largest_divisor(long n, int d)
        {
            int c = 0;
            while (n >= d)
            {
                n /= d;
                c++;
            }

            return (long)Math.Pow(d, c);
        }

        static List<long> find_largest_nums(long n)
        {
            long large_10 = find_largest_divisor(n, 10);
            long large_5 = find_largest_divisor(n, 5);
            long large_2 = find_largest_divisor(n, 2);
            Console.WriteLine($"10: {large_10}, 5: {large_5}, 2 : {large_2}");
            List<long> list = new List<long>();
            for (int i = 1; i < 10; i++)
            {
                list.Add(large_10 * i);
            }

            for (int i = 1; i < 5; i++)
            {
                list.Add(large_2 * i);
            }

            return list;
        }

        static long find_largest_pow(long n, int d)
        {
            int c = 0;
            while (n >= d)
            {
                n /= d;
                c++;
            }

            return c;
        }
        static long diff_comb_2_5(long n, long m) 
        {
            long p_2 = find_largest_pow(m, 2);
            long p_5 = find_largest_pow(m, 5);

            List<long> list = new List<long>();
            long num;
            for (int i = 1; i <= p_2; i++)
            {
                for (int j = 1; j <= p_5; j++)
                {
                    num = (long)Math.Pow(2, i) * (long)Math.Pow(5, j);
                    if (num <= n * m)
                    list.Add(num);
                }
            }

            long smallest_and_roundest = m * n;

            foreach (var item in list)
            {
                
                if (item * n <= m * n)
                {
                    smallest_and_roundest = rounder_one(item * n, smallest_and_roundest, false);
                }
            }

            long roundest = smallest_and_roundest;
            //Console.WriteLine(smallest_and_roundest);
            for (int i = 1; i <= m; i++)
            {

                if (smallest_and_roundest * i <= m * n)
                {
                    roundest = smallest_and_roundest * i;
                }
                else
                {
                    return roundest;
                }
            }

            return roundest;
        }

    }
}