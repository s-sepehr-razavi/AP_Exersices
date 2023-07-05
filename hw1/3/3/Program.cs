using System;
namespace _3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            if (n == 0)
            {
                return;
            }
            draw(n);
        }

        static void draw(int n, int i = 1)
        {
            if (n == i)
            {
                print_a_line(i, n);
                return;
            }
            print_a_line(i, n);
            Console.WriteLine();
            i++;
            draw(n, i);

        }

        static void print_a_line(int s, int n, int i = 0)
        {
            if (i == 4 * n + 1)
            {
                return;
            }

            

            if (i < 2 * s && i % 2 == 0)
            {
                Console.Write("*");
            }
            else if (i >= n * 4 + 1 - s * 2 && i % 2 == 0)
            {
                Console.Write("*");
            }
            else
            {
                Console.Write(" ");
            }

            print_a_line(s, n, i + 1);
        }

        static void print_another_line(int n, int s)
        {
            string str = "";
            for (int i = 0; i < n * 4; i++)
            {
                str += " ";
            }

            
        }
    }
}