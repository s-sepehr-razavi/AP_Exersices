using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
//using Newtonsoft.Json;

namespace _1
{

    //[JsonConverter(typeof(StringEnumConverter))]
    enum Currency
    {
        Franc=29,
        Dirham=7,
        Pound=35,
        Dollar=30
    }
    //[JsonConverter(typeof(StringEnumConverter))]
    enum Clothes
    {
        Socks = 1,
        Pants,
        Skirts,
        Shirt,
        Tshirt,
        Gloves,
        Hat
    }

    enum Postline
    {
        Regular = 1,
        Priority,
        Media,
        Certified,
        Airmail,
        Ground
    }

    enum Vehicle
    {
        Motorcycle =1,
        Truck,
        Car
    }


    record struct Location
    {
        public string country { get; init; }
        public string province { get; init; }
        public string city { get; init; }
        public string street { get; init; }
        public string streetnumber { get; init; }

        
    }

    struct PriceInToman
    {
        public double value
        {
            get; set;
        }
    }

    struct PriceInOtherCounty
    {
        public Currency currency { get; set; }
        public double value
        {
            get; set;
            
        }

        public static PriceInToman operator ~(PriceInOtherCounty p)
        {
            return new PriceInToman {value = (int)p.currency * p.value };
        }
    }

    record struct Time
    {
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public int second { get; set; }

    }

    class Post: IComparable<Post> 
    {
        public Clothes clothes { get; set; }
        public string id { get; set; }
        public Location origin { get; set; }
        public Location dest { get; set; }
        public Time sending_time { get; set; }
        public Time recieving_time { get; set; }
        public Vehicle vehicle { get; set; }
        public Postline postline { get; set; }
        public PriceInOtherCounty price { get; set; }

        [JsonIgnore]
        //public static List<Post> posts { get; set; } = new List<Post>();
        public static Dictionary<string, Post> posts {  get; set; } = new Dictionary<string, Post>();
        [JsonIgnore]
        static public double criteria { get; set; }

        public static bool operator ==(Post p1, Post p2) 
        {
            if (p1.clothes == p2.clothes && p1.id == p2.id && p1.origin == p2.origin && p1.dest == p2.dest &&
                p1.sending_time == p2.sending_time && p1.recieving_time == p2.recieving_time &&
                p1.vehicle == p2.vehicle && p1.postline == p2.postline && (~p1.price).value == (~p2.price).value)
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(Post p1, Post p2)
        {
            if (p1 == p2)
            {
                return false;
            }
            return true;
        }

        public int CompareTo(Post? other)
        {
            double s = Math.Abs((~this.price).value - criteria) - Math.Abs((~other.price).value - criteria);

            if (s == 0)
            {
                return 0;
            }
            if (s < 0)
            {
                return -1;
            }
            return 1;
        }
    }

    internal class Program
    {        
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("0.exit, 1.add post, 2.sort post, 3. change post data");
                string cmd = Console.ReadLine();

                if (cmd == "1")
                {
                    add_post();
                }
                else if (cmd == "2")
                {
                    Console.WriteLine("sort by: 1.price, 2.place");
                    cmd = Console.ReadLine();
                    if (cmd == "1")
                    {
                        sort_by_p();
                    }
                    else if(cmd == "2")
                    {
                        sort_by_price();
                    }
                    else
                    {
                        Console.WriteLine("Invalid input.");
                    }
                }
                else if (cmd == "3")
                {
                    change_prop();
                }
                else if (cmd == "0")
                {
                    string json = JsonSerializer.Serialize(Post.posts.Values.ToList(), new JsonSerializerOptions { WriteIndented = true, Converters = { new JsonStringEnumConverter() } });
                    File.WriteAllText("post.json", json);
                    return;
                }
                else
                {
                    Console.WriteLine("Please enter one of the specified in the commands.");                    
                }
            }

            
        }

        public static T enum_inp<T>(string prompt) where T : struct, IConvertible
        {
            Console.WriteLine(prompt);
            string inp = Console.ReadLine();
            T ret = default(T);
            if (Enum.IsDefined(typeof(T), inp))
            {
                Enum.TryParse(inp, out ret); 
            }
            return ret;
        }
        public static void add_post()
        {
            Clothes clothes = enum_inp<Clothes>("clothes type: ");
            if ((int) clothes == 0)
            {
                Console.WriteLine("Invalid clothes type.");
                return;
            }

            Console.WriteLine("ID:");
            string id = Console.ReadLine();
            string pattern = @"^[a-zA-Z]{2}\d{4}[@$#]$";

            if (!Regex.IsMatch(id, pattern))
            {
                Console.WriteLine("Invalid ID foramt.");
                return;
            }

            if (Post.posts.ContainsKey(id))
            {
                Console.WriteLine("Repetative ID.");
                return;
            }

            Console.WriteLine("Origin-----");
            Location origin = loc_inp();

            Console.WriteLine("Destination-----");
            Location destination = loc_inp();

            Console.WriteLine("Sending time-----");
            Time send = time_inp();
            if (send == default(Time))
            {
                Console.WriteLine("Invalid input for time.");
                return;
            }
            DateTime t;
            try
            {

                t = new DateTime(send.year, send.month, send.day, send.day, send.minute, send.second);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input for time");
                return;
            }

            Console.WriteLine("Receiving time-----");
            Time recieved = time_inp();
            if (recieved == default(Time))
            {
                Console.WriteLine("Invalid input for time.");
                return;
            }
            DateTime r;            
            try
            {

                r = new DateTime(recieved.year, recieved.month, recieved.day, recieved.day, recieved.minute, recieved.second);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input for time");
                return;
            }
            if (r <= t)
            {
                Console.WriteLine("Recieving time should be after sending.");
                return;
            }

            Vehicle vehicle = enum_inp<Vehicle>("vehicle: ");
            if ((int)vehicle == 0)
            {
                Console.WriteLine("Invalid vehicle type.");
                return;
            }

            Postline postline = enum_inp<Postline>("postline: ");
            if ((int)postline == 0)
            {
                Console.WriteLine("Invalid postline.");
                return;
            }

            Currency currency = enum_inp<Currency>("currency: ");
            if ((int)currency == 0)
            {
                Console.WriteLine("Invalid currency.");
                return;
            }

            double money = take_double("amount of money:");
            if (money <0)
            {
                Console.WriteLine("Invalid input.");
                return;
            }
            PriceInOtherCounty priceInOtherCounty = new PriceInOtherCounty()
            {
                currency = currency,
                value = money
            };

            Post post = new Post()
            {
                price = priceInOtherCounty,
                clothes = clothes,
                id = id,
                dest = destination,
                origin = origin,
                sending_time = send,
                recieving_time = recieved,
                vehicle = vehicle,
                postline = postline
            };

            Post.posts.Add(id, post);            
        }

        static public double take_double(string thing)
        {
            Console.WriteLine(thing);
            double id = -1;
            try
            {
                id = double.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                return -1;
            }

            return id;
        }

        static public int take_int(string thing)
        {
            Console.WriteLine(thing);
            int id = -1;
            try
            {
                id = int.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {                
                return -1;
            }

            return id;
        }
        public static Time time_inp()
        {
            int year = take_int("year:");
            Time time = default(Time);
            if (year < 0)
            {                
                return time;
            }

            int month = take_int("month:");
            if (month < 0)
            {
                
                return time;
            }

            int day = take_int("day:");
            if (day < 0)
            {
                
                return time;
            }

            int hour = take_int("hour:");
            if (hour < 0)
            {
                return time;
            }

            int minute = take_int("minute:");
            if (minute < 0)
            {
                return time;
            }

            int second = take_int("second:");
            if (second < 0)
            {
                return time;
            }

            return new Time()
            {
                year = year,
                month = month,
                day = day,
                hour = hour,
                minute = minute,
                second = second
            };

        }

        public static Location loc_inp()
        {
            
            Console.WriteLine("country:");
            string country_ = Console.ReadLine();

            Console.WriteLine("province:");
            string province_ = Console.ReadLine();

            Console.WriteLine("city:");
            string city_ = Console.ReadLine();

            Console.WriteLine("street:");
            string street_ = Console.ReadLine();

            Console.WriteLine("street number:");
            string street_number_ = Console.ReadLine();

            return new Location()
            {
                country = country_,
                city = city_,
                province = province_,
                street = street_,
                streetnumber = street_number_
            };


        }

        public static void sort_by_price()
        {
            Currency currency = enum_inp<Currency>("currency: ");
            if ((int)currency == 0)
            {
                Console.WriteLine("Invalid currency.");
                return;
            }

            double money = take_double("amount of money:");
            if (money < 0)
            {
                Console.WriteLine("Invalid input.");
                return;
            }
            PriceInOtherCounty priceInOtherCounty = new PriceInOtherCounty()
            {
                currency = currency,
                value = money
            };

            Post.criteria = (~priceInOtherCounty).value;
            List<Post> l_posts = Post.posts.Values.ToList();
            l_posts.Sort();

            string str;
            foreach (var item in l_posts)
            {
                str = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true, Converters = {new JsonStringEnumConverter() } });
                Console.WriteLine(str);
                Console.WriteLine("=====================================================================");
            }

        }

        public static void sort_by_p()
        {
            Console.WriteLine("Origin-----");
            Location origin = loc_inp();

            Console.WriteLine("Destination-----");
            Location destination = loc_inp();

            
            foreach (var item in Post.posts.Values)
            {
                if (item.dest == destination && item.origin == origin)
                {
                    Console.WriteLine(JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented =  true, Converters = { new JsonStringEnumConverter() } }));
                    Console.WriteLine("=====================================================================");
                }
            }
            
        }

        public static void change_prop()
        {

            Console.WriteLine("Enter ID:");
            string id = Console.ReadLine();
            if (!Post.posts.ContainsKey(id))
            {
                Console.WriteLine("Invalid format.");
                return;
            }
            Post post = Post.posts[id];

            Console.WriteLine("1. clothes, 2. ID, 3. origin, 4. destination, 5. sending time, 6. recieving time, 7. vehicle, 8. posline, 9. price");

            string cmd = Console.ReadLine();

            DateTime t = new DateTime(post.sending_time.year, post.sending_time.month, post.sending_time.day, post.sending_time.day, post.sending_time.minute, post.sending_time.second);
            DateTime r = t = new DateTime(post.recieving_time.year, post.recieving_time.month, post.recieving_time.day, post.recieving_time.day, post.recieving_time.minute, post.recieving_time.second);
            if (cmd == "1")
            {
                Clothes clothes = enum_inp<Clothes>("clothes type: ");
                if ((int)clothes == 0)
                {
                    Console.WriteLine("Invalid clothes type.");
                    return;
                }
                post.clothes = clothes;
            }
            else if (cmd == "2")
            {
                Console.WriteLine("ID:");
                string id_ = Console.ReadLine();
                string pattern = @"^[a-zA-Z]{2}\d{4}[@$#]$";

                if (!Regex.IsMatch(id_, pattern))
                {
                    Console.WriteLine("Invalid ID foramt.");
                    return;
                }

                if (Post.posts.ContainsKey(id_))
                {
                    Console.WriteLine("Repetative ID.");
                    return;
                }
                post.id = id_;
            }
            else if (cmd == "3")
            {
                Console.WriteLine("Origin-----");
                Location origin = loc_inp();
                post.origin = origin;
            }
            else if (cmd == "4")
            {
                Console.WriteLine("Destination-----");
                Location destination = loc_inp();
                post.dest = destination;
            }
            else if (cmd == "5")
            {
                Console.WriteLine("Sending time-----");
                Time send = time_inp();
                if (send == default(Time))
                {
                    Console.WriteLine("Invalid input for time.");
                    return;
                }                
                try
                {

                    t = new DateTime(send.year, send.month, send.day, send.day, send.minute, send.second);
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input for time");
                    return;
                }
                if (r <= t)
                {
                    Console.WriteLine("Recieving time should be after sending.");
                    return;
                }
                post.sending_time = send;
            }
            else if (cmd == "6")
            {
                Console.WriteLine("Receiving time-----");
                Time recieved = time_inp();
                try
                {

                    r = new DateTime(recieved.year, recieved.month, recieved.day, recieved.day, recieved.minute, recieved.second);
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input for time");
                    return;
                }
                if (recieved == default(Time))
                {
                    Console.WriteLine("Invalid input for time.");
                    return;
                }
                if (r <= t)
                {
                    Console.WriteLine("Recieving time should be after sending.");
                    return;
                }
                post.recieving_time = recieved;
            }
            else if (cmd == "7")
            {
                Vehicle vehicle = enum_inp<Vehicle>("vehicle: ");
                if ((int)vehicle == 0)
                {
                    Console.WriteLine("Invalid vehicle type.");
                    return;
                }
                post.vehicle = vehicle;

            }
            else if (cmd == "8")
            {
                Postline postline = enum_inp<Postline>("postline: ");
                if ((int)postline == 0)
                {
                    Console.WriteLine("Invalid postline.");
                    return;
                }
                post.postline = postline;
            }
            else if (cmd == "9") 
            {
                Currency currency = enum_inp<Currency>("currency: ");
                if ((int)currency == 0)
                {
                    Console.WriteLine("Invalid currency.");
                    return;
                }

                double money = take_double("amount of money:");
                if (money < 0)
                {
                    Console.WriteLine("Invalid input.");
                    return;
                }
                PriceInOtherCounty priceInOtherCounty = new PriceInOtherCounty()
                {
                    currency = currency,
                    value = money
                };
                post.price = priceInOtherCounty;
            }
            else
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            
        }

        
    }
}