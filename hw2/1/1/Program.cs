using System;
//2h
namespace _1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(lat(int.Parse(Console.ReadLine())));

        }

        static string lat(int n, int i = 1)
        {
            if (n == 1)
            {
                return $"{i}";
            }

            //return $"a + ({lat(n - 1)}) / ({lat(n - 1)})";
            return $"{i}+" + "\\frac" + "{" + lat(n - 1, 2 * i) + "}" + "{" + lat(n - 1, i * 2 + 1) + "}";
        }

    }
}