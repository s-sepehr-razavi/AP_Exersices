using System.Security.Cryptography.X509Certificates;

namespace _2
{

    enum Genre
    {
        Horror = 11,
        Action,
        Drama,
        ScienceFiction,
        Comedy,
        Documentary,
        History,
        Fiction
    }
    class Cinema
    {
        int id;
        string name;
        string director;
        string writer;
        Genre genre;
        double price;
        public static Dictionary<Genre, List<Cinema>> movies = new Dictionary<Genre, List<Cinema>>();
        static Dictionary<string, List<Cinema>> movies_d = new Dictionary<string, List<Cinema>>();  
        static List<Cinema> movies_l = new List<Cinema>();

        public Cinema(string name, string director, string writer, Genre genre, double price)
        {
            this.name = name;
            this.director = director;
            this.writer = writer;
            this.genre = genre;
            this.price = price;

            if (movies.ContainsKey(genre))
            {
                if (movies[genre].Count == 9)
                {
                    Console.WriteLine("There are maximum number of movies in this genre.");
                    return;
                }
                else
                {
                    movies[genre].Add(this);
                    movies_l.Add(this);
                }
            }
            else
            {
                movies[genre] = new List<Cinema>
                {
                    this
                };
                movies_l.Add(this);
            }

            if (movies_d.ContainsKey(director))
            {
                movies_d[director].Add(this);
            }
            else
            {
                movies_d[director] = new List<Cinema>
                {
                    this
                };
            }

            id = (int)genre * 10 + movies[genre].Count;

            StreamWriter streamWriter = new StreamWriter("Cinema.txt", append: true);
            streamWriter.WriteLine($"name: {this.name}, director: {this.director}, writer: {this.writer}, genre: {this.genre}, price: {this.price}");
            streamWriter.Close();
        }
        
        static public Genre director_genre(string director)
        {
            if (!movies_d.ContainsKey(director))
            {
                return 0;
            }

            List<Cinema> d_movies = movies_d[director];
            Dictionary<Genre, int> counts = new Dictionary<Genre, int>();
            foreach (var item in movies.Keys)
            {
                counts.Add(item, 0);
            }
            foreach (var item in d_movies)
            {
                counts[item.genre]++;
            }

            Genre gerne_max = 0;
            int count = -1;
            foreach (var item in counts.Keys)
            {
                if (count < counts[item])
                {
                    count = counts[item];
                    gerne_max = item;
                }
            }
            return gerne_max;
        }

        public static Cinema find_movie(string name)
        {
            Cinema movie = null;
            foreach (var item in movies_l)
            {
                if (item.name == name)
                {
                    movie = item;
                    break;
                }
            }
            return movie;
        }

        static public void change_movie_info(Cinema movie, string writer)
        {

            movie.writer = writer;
        }
        
        static public void change_movie_info(Cinema movie, Genre genre)
        {
            
            movie.genre = genre;
        }

        static public void change_movie_info(Cinema movie, double price)
        {            
            movie.price = price;
        }

        static public void show_info(Cinema m)
        {
            Console.WriteLine($"name: {m.name}, director: {m.director}, writer: {m.writer}, genre: {m.genre}, price: {m.price}");
        }

        static public void show_info()
        {
            foreach (var item in movies_l)
            {
                show_info(item);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            driver();
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

        public static Genre taking_genre()
        {
            Genre genre = Genre.Horror;
            do
            {
                if ((int)genre == 0)
                {
                    Console.WriteLine("Please choose from one the available genre.");
                }
                Console.WriteLine("Enter Genre of the book: ");
                Enum.TryParse(Console.ReadLine(), out genre);
            } while ((int)genre == 0);
            return genre;
        }

        public static void driver()
        {
            while (true)
            {
                Console.WriteLine("Enter one of the following commands: add movie, number of movies, genre of director, change movies properties, show info, exit");
                string command = Console.ReadLine();

                if (command == "add movie")
                {
                    add_movie();
                }
                else if(command == "number of movies")
                {
                    movie_num();
                }
                else if (command == "genre of director")
                {
                    director_genre();
                }
                else if (command == "change movies propreties")
                {
                    change();
                }
                else if (command == "show info")
                {
                    show_info();
                }
                else if (command == "exit")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Please enter one of the specified commands.");
                }
            }
        }

        static void add_movie()
        {
            Console.WriteLine("Enter a name: ");
            string name = Console.ReadLine();
            Cinema movie = Cinema.find_movie(name);
            if (movie != null)
            {
                Console.WriteLine("There is a movie with the same name in the list of available movies.");
                return;
            }

            Console.WriteLine("Enter director's name: ");
            string director = Console.ReadLine();

            Console.WriteLine("Enter writer's name: ");
            string writer = Console.ReadLine();

            Genre genre = taking_genre();

            double price = taking_double("Enter price of the ticket: ");
            new Cinema(name, director, writer, genre, price);
        }

        static void movie_num()
        {
            Genre genre = taking_genre();
            if (Cinema.movies.ContainsKey(genre))
            {
                Console.WriteLine($"Number of movies in this genre: {Cinema.movies[genre].Count}");
            }
            else
            {
                Console.WriteLine("There is no movie in this genre");
            }
        }

        static void director_genre()
        {
            Console.WriteLine("Enter name of a director: ");
            string director = Console.ReadLine();

            Genre gerne = Cinema.director_genre(director);

            if (gerne == 0)
            {
                Console.WriteLine("There is with the specified name.");
                return;
            }

            Console.WriteLine($"{director}'s genre is {gerne}.");
        }
        static void change()
        {
            Console.WriteLine("Which property you are looking for to change?(specify a number) 1. writer   2. ticket price  3. genre");
            string command = Console.ReadLine();

            Console.WriteLine("Enter movie's name");
            string name = Console.ReadLine();
            Cinema movie = Cinema.find_movie(name);
            if (movie == null)
            {
                Console.WriteLine("There is no movie with this name.");

                return;
            }
            if (command == "2")
            {

                Cinema.change_movie_info(movie, taking_double("Enter new price: "));
            }
            else if (command == "1")
            {
                Console.WriteLine("Enter new writer's name:");
                Cinema.change_movie_info(movie, Console.ReadLine());
            }
            else if (command == "3")
            {
                Cinema.change_movie_info(movie, taking_genre());
            }
        }

        static void show_info()
        {
            Console.WriteLine("Show a: 1. specific movie    2. all the available movies  (choose a number): ");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                Console.WriteLine("Enter name of the movie: ");
                Cinema m = Cinema.find_movie(Console.ReadLine());
                if (m == null)
                {
                    Console.WriteLine("There is no movie with this name.");
                    return;
                }

                Cinema.show_info(m);
            }
            else if (choice == "2")
            {
                Cinema.show_info();
            }
            else
            {
                Console.WriteLine("Choose one the specified commands.");
            }
        }
    }
}