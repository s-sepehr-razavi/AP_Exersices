using System;
namespace _5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] ints = Array.ConvertAll(Console.ReadLine().Split(), s => int.Parse(s));
            int[] a = Array.ConvertAll(Console.ReadLine().Split(), s => int.Parse(s));
            Console.WriteLine(func(a, ints[1]));
        }

        static int[] count_bits(int[] a)
        {
            int[] bits_num = new int[32];
            
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < a.Length; j++)
                {
                    if (((1 << i) & a[j]) != 0)
                    {
                        bits_num[i]++;
                    }
                }
            }

            return bits_num;
        }

        static int func(int[] a, int k)
        {
            int[] bit_nums = count_bits(a);
            bool[] bits_to_stay = new bool[32];

            for (int i = 30; i >= 0; i--)
            {
                //Console.WriteLine(bit_nums[i]);                
                if (a.Length - bit_nums[i] <= k)
                {
                    k -= (a.Length - bit_nums[i]);
                    bits_to_stay[i] = true;
                }
            }

            int or_this = 0;
            for (int i = 0; i < bits_to_stay.Length - 1; i++)
            {
                //Console.WriteLine(bits_to_stay[i]);
                if (bits_to_stay[i])
                {
                    or_this |= (1 << i);
                    //Console.WriteLine(or_this);
                }
            }

            int ans = a[0];
            foreach (var item in a)
            {
                ans &= item;
            }

            return or_this | ans;
        }

    }
}