using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace test
{

    enum Genre
    {
        Romance = 1,
        Classic,
        Horror,
        Science,
        Poetry,
        Novel,
        History
    }

    class MyClass
    {
        public int id { get; }
        public DateTime date;
        public static List<MyClass> myClasses = new List<MyClass>();

        public MyClass(int d, int m, int y) 
        {
            myClasses.Add(this);
            id = myClasses.Count;
            date = new DateTime(y, m, d);
        }

        static public void sort(int x)
        {
            if (x > 0)
            {
                myClasses.Sort(delegate (MyClass c1, MyClass c2)
                {
                    return c1.id.CompareTo(c2.id);
                });

            }
            else
            {
                myClasses.Sort(delegate(MyClass c1, MyClass c2)
                {
                    return c1.date.CompareTo(c2.date);
                });
            }
        }

    }
    internal class Program
    {
        public static double taking_double(string what_to_ask)
        {
            double initial_balance;
            while (true)
            {
                try
                {
                    Console.WriteLine(what_to_ask);
                    initial_balance = double.Parse(Console.ReadLine());
                    return initial_balance;
                }
                catch (Exception)
                {

                }
            }
        }
        static void Main(string[] args)
        {
            //double x = taking_double("blah blah balh");
            //string[] x;
            //string u = "SenderName: (Receiver1, NEWS, type_of_NEWS), (Receiver0, NEWS, type_of_NEWS)";
            //string r = @":\s\(|\),\s\(";
            //x = Regex.Split(u, r);
            //foreach (var item in x)
            //{
            //    Console.WriteLine("====================================");
            //    foreach (var c in Regex.Split(item, @",\s|\)"))
            //    {
            //        Console.WriteLine(c);
            //    }
            //}
            ////string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{10,}$";

            ////Console.WriteLine(Regex.IsMatch("fSfffdfsfsdfsd3fdsdfsdfdsa", pattern));
            ////User user = new User("123", "sep");
            ////User user1 = new User("321", "hi");
            ////write_users(User.users);
            ////read_users(User.users);

            ////foreach (User user in User.users)
            ////{
            ////    Console.WriteLine(user.username);
            ////    Console.WriteLine(user.username + " " + user.pass);
            ////}
            ////new User("an", "ng");
            ////write_users(User.users);
            ////User user1 = new User("123", "sep");
            ////User user2 = new User("321", "pashamk");
            ////write_users(User.users);
            ////read_users(User.users);

            ////foreach (var item in User.users)
            ////{
            ////    string x = item.username + " " + item.pass;
            ////    //Console.WriteLine(item.username);
            ////    //Console.WriteLine(item.username.Length);
            ////    //Console.WriteLine(item.pass);
            ////    //Console.WriteLine(item.pass.Length);
            ////    Console.WriteLine(x);
            ////}
            ///
            //MyClass c = new MyClass(2,3,1);
            //c.id = 1;

            //while (true)
            //{
            //    try
            //    {
            //        Console.WriteLine("Enter price of the book: ");
            //        int x = int.Parse(Console.ReadLine());
            //        break;
            //    }
            //    catch (Exception)
            //    {

            //    }
            //}
            //Genre genre = Genre.Poetry;
            //do
            //{
            //    if ((int)genre == 0)
            //    {
            //        Console.WriteLine("Please choose from one the available genre.");
            //    }
            //    Console.WriteLine("Enter Genre of the book: ");
            //    Enum.TryParse(Console.ReadLine(), out genre);
            //} while ((int)genre == 0);
            //DateTime dt = DateTime.Now;
            //string s = Console.ReadLine();
            //if (s == "h")
            //{
            //    DateTime dt2 = DateTime.Now;
            //    Console.WriteLine(dt2.Second - dt.Second);
            //}
            //Genre g = 0;
            //Console.Write(g);
            //StreamWriter streamWriter = new StreamWriter("test.txt", append: true);
            //streamWriter.WriteLine("h");
            //streamWriter.Close();
            string[] mat = Console.ReadLine().Split(' ', ',');
            List<string> list = mat.ToList();
            List<string> list2 = mat.ToList();
            foreach (var item in list2)
            {
                if (item == "")
                    list.Remove(item);
            }

            foreach (var item in list)
            {
                Console.WriteLine(item);
            }

        }

        static string Encode(string input)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        static string Decode(string input)
        {
            byte[] bytes = Convert.FromBase64String(input);
            return Encoding.Unicode.GetString(bytes);
        }
        public static void write_users(List<User> users)
        {
            StreamWriter stream = new StreamWriter("Users.txt");
            foreach (var item in users)
            {
                stream.WriteLine(item.username);
                stream.WriteLine(Encode(item.pass));
            }
            stream.Close();
        }

        public static void read_users(List<User> users)
        {
            StreamReader stream = new StreamReader("Users.txt");
            string line;

            bool odd = true;
            string username = "";
            string pass;
            while ((line = stream.ReadLine()) != null)
            {
                //Console.WriteLine($"{line}");
                if (odd)
                {
                    username = line;
                    odd = false;
                }
                else
                {
                    pass = Decode(line);
                    new User(pass, username);
                    odd = true;
                }
            }

        }

    }

    class User
    {
        public string pass;
        public string username;
        List<string> contacts = new List<string>();
        public static List<User> users = new List<User>();


        public User(string pass, string username)
        {
            this.pass = pass;
            this.username = username;

            users.Add(this);
        }
    }
}