using System;
using System.Diagnostics;

namespace _3
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

    enum UserType
    {
        active = 0,
        inactive = 1,
        debt = 2
    }
    class Book
    {
        public string name { get; set; }
        public Genre genre { get; set; }
        public string place { get; set; }
        public string author { get; set; }
        public double price { get; set; }
        public DateTime borrow_date { get; set; }

        public Book(string name, Genre genre, string place, string author, double price)
        {
            this.name = name;
            this.genre = genre;
            this.place = place;
            this.author = author;
            this.price = price;
        }

        public Book(Book book)
        {
            this.name = book.name;
            this.genre = book.genre;
            this.place = book.place;
            this.author = book.author;
            this.price = book.price;
            this.borrow_date = DateTime.Now;
        }
    }

    class User
    {
        string name;
        public string username { get; }
        bool isAdmin;
        public double balance { get; set; }
        public DateTime last_action { get; set; }
        public DateTime registration_time { get; set; }
        public List<Book> borrowed = new List<Book>();
        static public List<User> Users = new List<User>();
        public UserType UserType = UserType.active;

        public User(string name, string username, double balance, DateTime registration_time)
        {
            this.name = name;
            this.username = username;
            this.balance = balance;
            this.registration_time = registration_time;
            last_action = registration_time;
        }

        static public User find_user(string username)
        {
            foreach (var user in Users)
            {
                if (user.username == username)
                {
                    return user;
                }
            }

            return null;
        }
    }
    class Library
    {
        public string name;
        //List<Book> books = new List<Book>();
        public static List<User> users = new List<User>();
        public Dictionary<Book, int> books = new Dictionary<Book, int>();
        static public List<Library> Librarys = new List<Library>();

        public Library(string name)
        {
            this.name = name;
            Librarys.Add(this);
        }

        static public Library find_library(string name)
        {
            foreach(var library in Librarys)
            {
                if (library.name == name)
                {
                    return library;
                }
            }
            return null;
        }
        public bool add_user(User user)
        {
            if (users.Contains(user))
            {
                return false;
            }
            users.Add(user);
            return true;
        }

        public void add_book(Book book)
        {
            if (books.Keys.Contains(book))
            {
                books[book]++;
                return;
            }
            books.Add(book, 1);
            return;
        }

        public Book find_book(string name)
        {
            foreach (Book book in books.Keys)
            {
                if (book.name == name)
                {
                    return book;
                }
            }
            return null;
        }

        public User find_user(string username)
        {
            DateTime dateTime = DateTime.Now;

            List<User> users_cop = new List<User>();
            foreach (var item in users)
            {
                users_cop.Add(item);
            }

            foreach (User user in users_cop)
            {                

                if (user.last_action.Second - dateTime.Second > 120)
                {
                    users.Remove(user);
                }
                
                else if (user.username == username)
                {
                    user.last_action = DateTime.Now;
                    return user;
                }
            }
            return null;
        }   

        public List<Book> find_books(string author)
        {
            List<Book> list = new List<Book>();

            foreach (Book book in books.Keys)
            {
                if (book.name == name)
                {
                    list.Add(book);
                }
            }

            if (list.Count > 0)
            {
                return list;
            }

            return null;
        }

        public List<Book> find_books(Genre genre)
        {
            List<Book> list = new List<Book>();

            foreach (Book book in books.Keys)
            {
                if (book.genre == genre)
                {
                    list.Add(book);
                }
            }

            if (list.Count > 0)
            {
                return list;
            }

            return null;
        }

        
        public bool buy_book(Book book, User user)
        {
            if(book.price <= user.balance)
            {                
                    user.balance -= book.price;
                    books[book]--;
                    Console.WriteLine($"{book.name} purchased.");
                    if (books[book] == 0)
                    {
                        books.Remove(book);
                    }                
                return true;
            }
            else
            {
                Console.WriteLine("There is not enough moeny in the user's account.");
                return false;
            }
        }

        public void lend_book(Book book, User user)
        {            
            if (user.borrowed.Count < 3)
            {
                user.borrowed.Add(new Book(book));
                books[book]--;
                Console.WriteLine("User borrowed the book.");
                if (books[book] == 0)
                {
                    books.Remove(book);
                }
            }
            else
            {
                Console.WriteLine("User has maximum number of borrowed books");
            }            
            
        }

        
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            
            intitil_state();
            

        }

        static void write()
        {
            StreamWriter streamWriter = new StreamWriter("Books.txt");
            foreach (var lib in Library.Librarys)
            {
                streamWriter.WriteLine($"Library's name: {lib.name}");
                streamWriter.WriteLine("Books that are available in this library: ");
                foreach (var book in lib.books.Keys)
                {
                    streamWriter.WriteLine($"{book.name}, {book.author}, {book.price}, {book.genre}, {book.place}");
                }
            }
        }



        static void intitil_state()
        {
            while (true)
            {
                Console.WriteLine("Enter as admin/user or exit the program");
                string command = Console.ReadLine();

                if (command == "admin")
                {
                    Console.WriteLine("Enter admin's code:");
                    command = Console.ReadLine();
                    if (command == "@DM!N")
                    {
                        admin_state();
                    }
                }
                else if (command == "user")
                {
                    try
                    {
                        Library library = lib();
                        Console.WriteLine("Enter your username: ");
                        User user = library.find_user(Console.ReadLine());
                        if (user != null)
                        {
                            user_state(user, library);
                        }
                        else
                        {
                            Console.WriteLine("There is no user with this username.");
                        }
                    }
                    catch (LibException e)
                    {
                        Console.WriteLine(e.Message);                        
                    }
                }
                else if (command == "exit")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter one of the specified commands.");
                }
            }
        }

        class LibException: Exception
        {
            string message;
            public LibException(string message) : base(message)
            {
                this.message = message;
            }
        }
        public static Library lib()
        {
            Console.WriteLine("Specify a library: ");
            Library library = null;            
            string lib_name = Console.ReadLine();
            library = Library.find_library(lib_name);
            if (library == null)
            {
                throw new LibException("Library not found.");
            }
            return library;
        }

        static void admin_state()
        {
            while (true)
            {
                Console.WriteLine("Choose command: add library, add new book, add member, search book, display book, change book info, return");
                string command = Console.ReadLine();

                if (command == "add library")
                {
                    Console.WriteLine("Choose a name for the library: ");
                    string name = Console.ReadLine();
                    Library library = new Library(name);
                }
                else
                {
                    try
                    {

                        if (command == "add new book")
                        {
                            add_book(lib());
                        }
                        else if (command == "add member")
                        {
                            add_member(lib());
                        }
                        else if (command == "search book")
                        {
                            find_book(lib());
                        }
                        else if (command == "display book")
                        {
                            display(lib());
                        }
                        else if (command == "change book info")
                        {
                            change_info(lib());
                        }
                        else if (command == "return")
                        {
                            return;
                        }
                    }
                    catch (LibException e)
                    {

                        Console.WriteLine(e.Message);
                    }
                    
                    
                }
            }
        }

        public static double taking_double(string what_to_ask)
        {
            double initial_balance;
            while (true)
            {
                try
                {
                    Console.WriteLine(what_to_ask);
                    initial_balance = double.Parse(Console.ReadLine());
                    if (initial_balance < 0)
                    {
                        continue;
                    }
                    return initial_balance;
                }
                catch (Exception)
                {

                }
            }
        }

        static public void change_info(Library library)
        {
            Console.WriteLine("Enter name of the book: ");
            string old_name = Console.ReadLine();
            Console.WriteLine("Enter new name of the book: ");
            string name = Console.ReadLine();
            Book book_x = library.find_book(old_name);
            if (book_x == null)
            {
                Console.WriteLine("This book does not exist.");
                return;
            }

            Genre genre = Genre.Poetry;
            do
            {
                if ((int)genre == 0)
                {
                    Console.WriteLine("Please choose one of the from one the availble genre.");
                }
                Console.WriteLine("Enter Genre of the book: ");
                Enum.TryParse(Console.ReadLine(), out genre);
            } while ((int)genre == 0);

            Console.WriteLine("Enter place of the book on the shelf: ");
            string place = Console.ReadLine();

            Console.WriteLine("Enter name of the author: ");
            string author = Console.ReadLine();

            double price = taking_double("Enter price of the book: ");
            book_x.price = price;
            book_x.place = place;
            book_x.name = name;
            book_x.author = author;
            book_x.genre = genre;
            return;

        }
        public static void display(Library library)
        {
            foreach (var item in library.books.Keys)
            {
                Console.WriteLine($"name: {item.name}\nprice: {item.price}\ncount: {library.books[item]}");
                Console.WriteLine("====================================================================================");
            }
        }
        public static void find_book(Library library)
        {
            Console.WriteLine("select an option for finding books: 1.by name   2.by name of author   3.by genre (choose a number)");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.WriteLine("Enter name of the book: ");
                string name = Console.ReadLine();
                Book book = library.find_book(name);
                if (book == null)
                {
                    Console.WriteLine("There is no book with the specified name.");
                    return;
                }

                if (library.books[book] == 0)
                {
                    Console.WriteLine("There is no book with the specified name.");
                    return;
                }

                Console.WriteLine($"This book can be found in {book.place}");
                return;
            }

            else if (choice == "3")
            {
                Genre genre = Genre.Poetry;
                do
                {
                    if ((int)genre == 0)
                    {
                        Console.WriteLine("Please choose one of the from one the availble genre.");                        
                    }
                    Console.WriteLine("Enter Genre of the book: ");
                    Enum.TryParse(Console.ReadLine(), out genre);
                } while ((int)genre == 0);

                List<Book> books = library.find_books(genre);
                if (books == null)
                {
                    Console.WriteLine("There is no book with this genre in library.");
                    return;
                }
                foreach (var item in books)
                {
                    if (library.books[item] > 0)
                        Console.WriteLine($"book: {item.name} / place {item.place}");
                }

            }

            else if (choice == "2")
            {
                Console.WriteLine("Enter name of the author: ");
                List<Book> books = library.find_books(Console.ReadLine());
                if (books == null)
                {
                    Console.WriteLine("There is no book written by this author.");
                    return;
                }
                foreach (var item in books)
                {
                    if (library.books[item] > 0)
                        Console.WriteLine($"book: {item.name} / place {item.place}");
                }
            }
            else
            {
                Console.WriteLine("Non of the specified commands have been chosen.");
            }
        }
        public static void add_member(Library library)
        {
            User user;
            string username;
            while (true)
            {
                Console.WriteLine("Enter a username: ");
                username = Console.ReadLine();
                user = library.find_user(username);
                if (username == "QUIT")
                {
                    return;
                }
                if (user == null)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("A user with this name already exits.");
                }

            }

            double balance = taking_double("Enter balance of the user: ");
            while (balance < 100)
            {
                Console.WriteLine("The user does not have enough moeny for the initial registration. Do you want to continue? (Enter QUIT if you want to exit)"); ;
                string q = Console.ReadLine();
                if (q == "QUIT")
                {
                    return;
                }
                balance = taking_double("Enter balance of the user: ");
            }

            Console.WriteLine("Enter the user's name: ");
            string name = Console.ReadLine();

            User user1 = new User(name, username, balance, DateTime.Now);
            library.add_user(user1);
            Console.WriteLine("Registration was successful.");
        }
        public static void add_book(Library library)
        {
            Console.WriteLine("Enter name of the book: ");
            string name = Console.ReadLine();
            Book book_x = library.find_book(name);
            if (book_x != null)
            {
                Console.WriteLine("The count of this book increased.");
                library.books[book_x]++;
                return;
            }

            Genre genre = Genre.Poetry;
            do
            {
                if ((int)genre == 0)
                {
                    Console.WriteLine("Please choose from one the available genre.");
                }
                Console.WriteLine("Enter Genre of the book: ");
                Enum.TryParse(Console.ReadLine(), out genre);
            } while ((int)genre == 0);

            Console.WriteLine("Enter place of the book on the shelf: ");
            string place = Console.ReadLine();

            Console.WriteLine("Enter name of the author: ");
            string author = Console.ReadLine();

            double price = taking_double("Enter price of the book: ");
            Book book = new Book(name, genre, place, author, price);
            library.add_book(book);
        }
        static void user_state(User user, Library library)
        {
           
            while (true)
            {
                Console.WriteLine("Choose command: search book, buy book, borrow book, charge account, display books, return book, return");
                string command = Console.ReadLine();

                if (command == "search book")
                {
                    find_book(library);
                }
                else if (command == "buy book")
                {
                    buy_book(library);
                }
                else if (command == "borrow book")
                {
                    borrow_book(library, user);
                }
                else if (command == "charge account")
                {
                    charge_account(library);
                }
                else if (command == "display books")
                {
                    display(library);
                }
                else if (command == "return book")
                {
                    return_book(library, user);
                }
                else if (command == "return")
                {
                    return;
                }
            }
        }

        public static void return_book(Library library, User user)
        {
            Book book = null;
            while (book == null)
            {
                Console.WriteLine("Enter name of the book that you want to return: ");
                string name = Console.ReadLine();                
                foreach (var item in user.borrowed)
                {
                    if (item.name == name)
                    {
                        book = item; break;
                    }
                }

                if (book == null)
                {
                    Console.WriteLine("This book is not available.");
                    Console.WriteLine("Do you want to continue? (Enter YES if you want to do so.");
                    if (Console.ReadLine() != "YES")
                    {
                        return; 
                    }
                }
                else
                {
                    break;
                }
            }

            Book book_on_shelf = library.find_book(book.name);

            if (book_on_shelf != null)
            {
                library.add_book(book_on_shelf);
            }
            else
            {
                library.add_book(book);
            }
            user.borrowed.Remove(book);
            Console.WriteLine($"User {user.username} returned {book.name}");

            int x = DateTime.Now.Second - book.borrow_date.Second;
            if (x> 10)
            {
                user.UserType = UserType.debt;
                Console.WriteLine($"The user has returned this book {x} after the specified time. They will be penalized by {2*x} dollars");
                user.balance -= 2 * x;
            }
            
        }
        public static void borrow_book(Library library, User user)
        {
            Console.WriteLine("Enter name of the book that you want to borrow: ");
            Book book = library.find_book(Console.ReadLine());
            if (book == null)
            {
                Console.WriteLine("This book is not available.");
                return;
            }

            library.lend_book(book, user);

        }
        public static void buy_book(Library library)
        {
            Console.WriteLine("Enter username of the person that you want to buy the book for: ");
            User user = library.find_user(Console.ReadLine());
            if (user == null)
            {
                Console.WriteLine("Username is invalid.");
                return;
            }

            Console.WriteLine("Enter name of the book that you want to purchase: ");
            Book book = library.find_book(Console.ReadLine());
            if (book == null)
            {
                Console.WriteLine("This book is not available.");
                return;
            }

            while (!library.buy_book(book, user))
            {
                Console.WriteLine("Do you want to charge your account? (Enter YES if you want to do so.)");
                if (Console.ReadLine() == "YES")
                {
                    charge_account(library);
                }
                else
                {
                    return;
                }
            }
        }

        static public void charge_account(Library library)
        {
            Console.WriteLine("Enter username of the person that you want charge their account: ");
            User user = library.find_user(Console.ReadLine());
            if (user == null)
            {
                Console.WriteLine("Username is invalid.");
                return;
            }

            double x = taking_double("How much do you want to charge?");
            user.balance += x;
        }

    }
}