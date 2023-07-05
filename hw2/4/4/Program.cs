using System;
using System.Collections.Generic;
//2h
namespace _4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] ints = Array.ConvertAll(Console.ReadLine().Split(), s => int.Parse(s));
            r = ints[1];
            c = ints[0];
            map = new List<int[]>();

            int x = 0, y = 0, n = 0;
            for (int i = 0; i < r; i++)
            {
                map.Add(Array.ConvertAll(Console.ReadLine().Split(), s => int.Parse(s)));
                for (int j = 0; j < c; j++)
                {
                    if (map[i][j] == 0)
                    {
                        n++;
                    }

                    if (map[i][j] == 1)
                    {
                        x = i; y = j;
                    }
                }
            }

            
            f(x, y, n);
            Console.WriteLine(count);
        }

        static List<int[]> map;
        static int r;
        static int c;
        static int count = 0 ;
        
        static void display()
        {
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    Console.Write(map[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
        static void f(int x, int y, int n)
        {
            //Console.WriteLine("=============================================================");
            //display();

            if (x > 0)
            {
                if (map[x-1][y] == 0 || (map[x-1][y] == 2 && n == 0))
                {
                    map[x][y] = -1;
                    if (n == 0) count++;
                    f(x - 1, y, n - 1);
                    map[x][y] = 0;
                }

            }

            if (y > 0)
            {
                if (map[x][y-1] == 0 || (map[x][y - 1] == 2 && n == 0))
                {
                    map[x][y] = -1;
                    if (n == 0) count++;
                    f(x, y - 1, n - 1);
                    map[x][y] = 0;
                }

            }

            if (x < r - 1)
            {
                if (map[x+1][y] == 0 || (map[x+1][y] == 2 && n == 0))
                {
                    map[x][y] = -1;
                    if (n == 0) count++;
                    f(x + 1, y, n - 1);
                    map[x][y] = 0;
                }

            }

            if (y < c - 1)
            {
                if (map[x][y+1] == 0 || (map[x][y + 1] == 2 && n == 0))
                {
                    map[x][y] = -1;
                    if (n == 0) count++;
                    f(x, y + 1, n - 1);
                    map[x][y] = 0;
                }

            }
        }

    }
}