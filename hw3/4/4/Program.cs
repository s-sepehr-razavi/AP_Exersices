using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace _4
{

    enum NewsType
    {
        economical = 12,
        social = 23,
        events = 34,
        tech = 45,
        sports = 56,
        weather = 67
    }

    class User
    {
        string pass;
        string username;
        List<string> contacts = new List<string>();
        List<News> recieved_news = new List<News>();
        List<News> sent_news = new List<News>();
        static List<User> users = new List<User>();


        public User(string pass, string username) 
        {
            this.pass = pass;
            this.username = username;
            
            if (return_user_uname(username) != null)
            {
                Console.WriteLine("A user with this username already exists.");
                return;
            }

            users.Add(this);
        }
        static public User return_user(string username, string password)
        {
            foreach (var user in users)
            {
                if (user.username == username && user.pass == password)
                {
                    return user;
                } 
            }

            return null;
        }
        

        static public User return_user_uname(string username)
        {
            foreach (var item in users)
            {
                if (item.username == username)
                {
                    return item;
                }
            }

            return null;
        }

        public bool contains_contact(string username)
        {
            return contacts.Contains(username);
        }
        public void change_pass()
        {
            Console.Write("Enter the current password: ");
            string password = Console.ReadLine();

            if (password == this.pass)
            {
                Console.Write("Enter new password: ");
                string n_pass = Console.ReadLine();
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{10,}$";                
                if (Regex.IsMatch(n_pass, pattern))
                {
                    this.pass = n_pass;

                }
                else
                {
                    Console.WriteLine("The password does not follow the required conditions.");
                }
                
            }
            else
            {
                Console.WriteLine("Entered password was incorrect.");
            }
        }

        public void delete_news(News to_del, bool recieved)
        {
            if (recieved)
            {
                recieved_news.Remove(to_del);
            }
            else
            {
                sent_news.Remove(to_del);
            }
        }
        public void add_contact(string username)
        {
            User user = return_user_uname(username);
            if (user == null)
            {
                Console.WriteLine("There is no user with the specified username.");
                return;
            }
            if (this.contains_contact(user.username))
            {
                Console.WriteLine("The contact already exists");
                return;
            }
            this.contacts.Add(user.username);
        }

        public void delete_contact(string username)
        {
            User user = return_user_uname(username);
            if (user == null)
            {
                Console.WriteLine("There is no user with the specified username.");
                return;
            }

            contacts.Remove(user.username);

            List<News> news_to_remove = new List<News>();
            foreach (var item in sent_news)
            {
                if (item.get_reciever() == username)
                {
                    news_to_remove.Add(item);
                }
            }

            foreach (var item in news_to_remove)
            {
                sent_news.Remove(item);
            }
            
        }

        private News find_news(int id)
        {
            foreach(var item in sent_news)
            {
                if (item.get_id() == id)
                {
                    return item;
                }
            }
            return null;
        }
        public void edit_news(int id, string new_content)
        {
            News news = find_news(id);
            if (news == null)
            {
                Console.WriteLine("There is no sent news with the given id.");
                return;
            }

            news.edit_news(new_content);
        }
        public void add_news(bool recieved, News news)
        {
            if (recieved)
            {
                recieved_news.Add(news);
            }
            else
            {
                sent_news.Add(news);
            }
        }

        public void print_all_news()
        {
            Console.WriteLine("sent news:");
            foreach (var item in sent_news)
            {
                Console.WriteLine(item.get_news());
            }
            Console.WriteLine("================================================================================================");
            Console.WriteLine("recieved news:");
            foreach (var item in recieved_news)
            {
                Console.WriteLine(item.get_news());
            }
        }
        
        public string get_username()
        {
            return this.username;
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

        public static void write_users()
        {
            StreamWriter stream = new StreamWriter("Users.txt");
            foreach(var item in users)
            {
                stream.WriteLine(item.username);
                stream.WriteLine(Encode(item.pass));
            }
            stream.Close();
        }

        public static void read_users()
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
            stream.Close ();
        }

        public static void write_contacts()
        {
            StreamWriter streamWriter = new StreamWriter("Contacts.txt");

            string s;
            User u;
            for (int j = 0; j < User.users.Count; j++)
            {
                s = "";
                u = User.users[j];

                //Console.WriteLine(u.username + u.contacts.Count);
                if (u.contacts.Count == 0)
                {
                    continue;
                }
                s += (u.username + ": ");
                //streamWriter.Write(u.username + ": ");
                //Console.Write(u.username + ": ");
                for (int i = 0; i < u.contacts.Count - 1; i++)
                {
                    s += (u.contacts[i] + ", ");
                    //streamWriter.Write(u.contacts[i] + ", ");
                    //Console.Write(u.contacts[i] + ", ");
                }
                
                s += (u.contacts[u.contacts.Count - 1]);
                
                //streamWriter.Write(u.contacts[u.contacts.Count - 1] + "\n");
                streamWriter.WriteLine(s);
                Console.WriteLine(s);
                //Console.Write(u.contacts[u.contacts.Count - 1] + "\n");

            }

           

            streamWriter.Close();
        }

        public static void read_contatct()
        {
            StreamReader stream = new StreamReader("Contacts.txt");
            string line;
            string r = @":\s|,\s";
            string[] x;
            User user;
            while ((line = stream.ReadLine()) != null)
            {
                
                x = Regex.Split(line, r);
                user = return_user_uname(x[0]);
                for (int i = 1; i < x.Length; i++)
                {
                    user.contacts.Add(x[i]);
                }
            }
            stream.Close();
        }

        public static void write_news()
        {
            StreamWriter streamWriter = new StreamWriter("News.txt");
            string s;
            foreach (var u in users)
            {
                s = "";
                if (u.sent_news.Count == 0)
                {
                    continue;
                }
                //streamWriter.Write(u.username + ": ");
                s += u.username + ": ";
                for (int i = 0; i < u.sent_news.Count - 1; i++)
                {
                    //streamWriter.Write($"({u.sent_news[i].get_reciever}, {u.sent_news[i].content}, {u.sent_news[i].NewsType}), ");
                    s += $"({u.sent_news[i].get_reciever()}, {u.sent_news[i].content}, {u.sent_news[i].NewsType}), ";
                }
                //streamWriter.Write($"({u.sent_news[u.sent_news.Count - 1].get_reciever()}, {u.sent_news[u.sent_news.Count - 1].content}, {u.sent_news[u.sent_news.Count - 1].NewsType})\n");
                s += $"({u.sent_news[u.sent_news.Count - 1].get_reciever()}, {u.sent_news[u.sent_news.Count - 1].content}, {u.sent_news[u.sent_news.Count - 1].NewsType})";
                streamWriter.WriteLine(s);
            }
            streamWriter.Close();
        }

        public static void read_news()
        {
            StreamReader stream = new StreamReader("News.txt");
            string line;
            string r = @":\s\(|\),\s\(";
            string[] x;
            User sender;
            while ((line = stream.ReadLine()) != null)
            {

                x = Regex.Split(line, r);
                string sender_username = x[0];
                sender = return_user_uname(sender_username);

                string[] y;
                for (int i = 1; i < x.Length; i++)
                {
                    y = Regex.Split(x[i], @",\s|\)");
                    User reciever = return_user_uname(y[0]);
                    string news_content = y[1];
                    NewsType type;
                    Enum.TryParse(y[2], out type);

                    News this_news = new News(sender_username, y[0], news_content, type);
                    reciever.add_news(true, this_news);
                    sender.add_news(false, this_news);
                }
            }
            stream.Close();
        }

    }

    class News
    {
        int id;
        string sender_name;
        string reciever_name;
        DateTime sending_date;
        string news_content;
        NewsType type;
        public static List<News> news = new List<News>();
        
        public News(string sender_name, string reciever_name, string news_content, NewsType type)
        {            
            this.sender_name = sender_name;
            this.reciever_name = reciever_name;
            this.sending_date = DateTime.Now;
            this.news_content = news_content;
            this.type = type;
            news.Add(this);
            this.id = news.Count;
        }

        public int get_id()
        {
            return id;
        }

        public string get_sender()
        {
            return sender_name;
        }

        public string get_reciever()
        {
            return reciever_name;
        }

        public void edit_news(string content)
        {
            this.news_content = content;
        }

        public string get_news()
        {
           return this.news_content;
        }

        public NewsType NewsType { get { return type; } }
        public string content { get { return news_content; } }

        public static void sort(string choice)
        {
            if (choice == "1")
            {
                news.Sort(delegate (News news1, News news2)
                {
                    return news1.id.CompareTo(news2.id);
                });
            }
            else if (choice == "2")
            {
                news.Sort(delegate (News news1, News news2)
                {
                    return news1.sending_date.CompareTo(news2.sending_date);
                });
            }
            else
            {
                Console.WriteLine("You have not specified any ordering.");
                return;
            }

            foreach (var item in news)
            {
                Console.WriteLine(item.news_content);
            }

        }

        public static List<News> find_news(string choice)
        {
            List<News> result = new List<News>();

            if (choice == "1")
            {
                Console.Write("Pleaes enter a username: ");
                string username = Console.ReadLine();

                foreach (var item in news)
                {
                    if (item.sender_name == username)
                    {
                        result.Add(item);
                    }
                }
            }

            else if (choice == "2")
            {
                NewsType newsType;
                Enum.TryParse(Console.ReadLine(), out newsType);

                if ((int)newsType == 0)
                {
                    Console.WriteLine("The specified news category is not valid.");
                    return null;
                }

                foreach(var item in news)
                {
                    if (item.type == newsType)
                    {
                        result.Add(item);
                    }
                }
            }
            else
            {
                Console.WriteLine("Non of the options have been chosen.");
                return null;
            }
            return result;
        }

        
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            driver();
            //NewsType x = (NewsType)int.Parse(Console.ReadLine());
            //Enum.TryParse(Console.ReadLine(), out x);
            //Console.WriteLine(x);
        }

        static void driver()
        {
            try
            {
                User.read_users();
                User.read_contatct();
                User.read_news();
            }
            catch (Exception)
            {

                
            }
            User user = initial_state();
            if (user == null) {
                User.write_users();
                return; 
            }

            while (true)
            {
                Console.WriteLine("Please enter a command: exit, add contact, delete contact, send news, edit news, show news, sort news, find news, delete news, change password: ");
                string command = Console.ReadLine();
                if (command == "exit")
                {
                    user = initial_state();
                    if (user == null ) 
                    {
                        User.write_users();
                        User.write_contacts();
                        User.write_news();
                        return; 
                    }
                }

                else if (command == "add contact")
                {
                    add_contact(user);
                }

                else if (command == "delete contact")
                {
                    delete_contact(user);
                }

                else if (command == "send news")
                {
                    send_news(user);
                }

                else if (command == "edit news")
                {
                    edit_news(user);
                }

                else if (command == "show news")
                {
                    user.print_all_news();
                }

                else if(command == "sort news")
                {
                    sort_news();
                }

                else if (command == "find news")
                {
                    find_news();
                }

                else if (command == "delete news")
                {
                    delete_news();
                }

                else if (command  == "change password")
                {                    
                    user.change_pass();
                }

                else
                {
                    Console.WriteLine("Please enter one of the specified commands.");
                }
            }

            
        }

        static User initial_state()
        {
            while (true)
            {
                Console.Write("Please log in (with 1) or sign up (with 2): ");
                string option = Console.ReadLine();
                if (option == "exit")
                {
                    return null;
                }

                Console.Write("Please enter your username: ");
                string username = Console.ReadLine();
                if (option == "exit")
                {
                    return null;
                }

                Console.Write("Pleae enter your password: ");
                string password = Console.ReadLine();
                if (option == "exit")
                {
                    return null;
                }

                if (option == "1")
                {

                    User user = User.return_user(username, password);
                    if (user != null)
                    {                        
                        return user;
                    }
                    else
                    {
                        Console.WriteLine("There is no user with the specified username or password.");                        
                    }
                }

                if (option == "2")
                {
                    string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{10,}$";
                    if (Regex.IsMatch(password, pattern))
                    {
                    User user = new User(password, username);
                    
                    }
                    else
                    {
                        Console.WriteLine("The password does not follow the required conditions.");
                    }
                }
            }
            
            return null;
        }
        
        static void add_contact(User user)
        {
            Console.Write("Please enter name of the user: ");
            string username = Console.ReadLine();
            user.add_contact(username);
            
        }

        static void delete_contact(User user)
        {
            Console.Write("Please enter name of the user: ");
            string username = Console.ReadLine();
            user.delete_contact(username);

        }

        static void send_news(User user)
        {
            Console.Write("Please enter name of the user: ");
            string username = Console.ReadLine();
            if (!user.contains_contact(username))
            {
                Console.WriteLine("There is no contact with the specified username.");
                return;
            }
            Console.Write("Please enter type of your news: ");
            string t = Console.ReadLine();
            NewsType type;
            Enum.TryParse(t, out type);
            if ((int)type == 0)
            {
                Console.WriteLine("This type of news is invalid.");
                return;
            }
            string news_content = Console.ReadLine();
            News news = new News(user.get_username(), username, news_content, type);
            
            User receiver = User.return_user_uname(username);

            user.add_news(false, news);
            receiver.add_news(true, news);
        }

        static void edit_news(User user)
        {
            Console.Write("Please enter news ID: ");
            int id = int.Parse(Console.ReadLine());
            string new_content = Console.ReadLine();    

            user.edit_news(id, new_content);
        }

        static void sort_news()
        {
            Console.WriteLine("sort based on (please choose a number): 1.ID   2.date time");
            string choice = Console.ReadLine();

            News.sort(choice);            
        }

        static void find_news()
        {
            Console.WriteLine("Find news based on(please choose a number): 1.sender username    2. news type");
            string choice = Console.ReadLine();
            List<News> list = News.find_news(choice);

            foreach (News news in list)
            {
                Console.WriteLine($"news type:{news.NewsType}/  {news.content}");
            }
        }

        static void delete_news()
        {
            Console.WriteLine("Find news based on(please choose a number): 1.sender username    2. news type");
            string choice = Console.ReadLine();
            List<News> list = News.find_news(choice);

            User reciever;
            User sender;
            foreach (var item in list)
            {
                reciever = User.return_user_uname(item.get_reciever());
                sender = User.return_user_uname(item.get_sender());
                News.news.Remove(item);
                reciever.delete_news(item, true);
                sender.delete_news(item, false);
            }
        }

        

    }

    
}