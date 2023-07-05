using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _1
{
    internal class Program
    {

        static int how_many_d(string s)
        {
            int c = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsDigit(s[i]))
                {
                    c++;
                }
            }
            return c;
        }

        static int how_many_v(string s)
        {
            int c = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == 'a' || s[i] == 'e' || s[i] == 'o' || s[i] == 'i' || s[i] == 'u')
                {
                    c++;
                }

                if (s[i] == 'A' || s[i] == 'E' || s[i] == 'O' || s[i] == 'I' || s[i] == 'U')
                {
                    c++;
                }

            }
            return c;
        }

        static bool is_it_ae(string s)
        {
            if (s[0] == 'a' && s[s.Length - 1] == 'e')
            {
                return true;
            }

            if (s[0] == 'A' && s[s.Length - 1] == 'E')
            {
                return true;
            }

            return false;
        }

        static bool stu(string s)
        {
            string ss = s.ToLower();
            string p = "student";
            return Regex.IsMatch(ss, p);
        }

        static void Main(string[] args)
        {
            StreamReader reader;
            try
            {
                reader = new StreamReader("a1.txt");
            }
            catch (Exception)
            {

                Console.WriteLine("Make a text file with a1 as it's name.");
                return;
            }

            string line;
            int l_count = 0;
            int d_count = 0;
            int asx_count = 0;
            int v_count = 0;
            int ae_count = 0;
            int stu_count = 0;

            string[] words;            

            string s = "";
            while ((line = reader.ReadLine()) != null)
            {                

                words = line.Split();
                l_count++;
                asx_count += (words.Length - 1);

                
                for (int i = 0; i < words.Length; i++)
                {
                    string item = words[i];
                    d_count += how_many_d(item);
                    v_count += how_many_v(item);
                    ae_count += (is_it_ae(item) ? 1 : 0);
                    stu_count += (stu(item) ? 1 : 0);
                    //if(i < words.Length - 1)
                    //    writer.Write(item + "*");
                    //else writer.Write(item + "\n");
                    if (i < words.Length - 1)
                        s += (item + "*");
                    else s += (item + "\n");
                }

            }
            reader.Close();
            Console.WriteLine("number of line: " + l_count);
            Console.WriteLine("digit count: " + d_count);
            Console.WriteLine("vowvels count: " + v_count);
            Console.WriteLine("number of words start with a and end with e: " + ae_count);
            Console.WriteLine("student count: " + stu_count);
            Console.WriteLine("* count: " + asx_count);

            StreamWriter streamWriter = new StreamWriter("a2.txt");
            streamWriter.WriteLine(s);
            streamWriter.Close();
        }
    }
}