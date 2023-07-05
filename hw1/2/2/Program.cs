using System;
namespace _2
{
    internal class Program
    {

        static bool is_it_fib(long n) 
        {
            if (n == 1)
            {
                return true;
            }
            long a = 1;
            long b = 1;
            long c = 0;

            while (b < n)
            {
                //Console.WriteLine(b);
                c = b;
                b = a + b;
                a = c;
            }

            if(b == n)  return true;
            else return false;
        }

        static bool is_it_prime(long n, long a)
        {
            if (n == 2 || n == 3) 
            {
                return true;
            }
            if (a == 2)
            {
                if (n % a == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            if (n % a == 0)
            {
                return false;
            }
            else
            {
                return is_it_prime(n, a - 1);
            }
        }
        static void Main(string[] args)
        {
            long x = long.Parse(Console.ReadLine());
            //Console.WriteLine(is_it_prime(x, (long)Math.Sqrt(x)));

            //Console.WriteLine(is_it_fib(x));
            for (int i = 2; i <= x; i++)
            {
                if (is_it_prime(i, (long)Math.Sqrt(i)) && is_it_fib(i))
                    Console.Write(i + " ");
            }
        }
    }
}