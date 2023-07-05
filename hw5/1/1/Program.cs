namespace _1
{
    public static class e
    {
        static public int n, m;
        public static string c(this int x, int y)
        {
            return x + "-" + y;
        }
    }
    class Animal
    {
        protected int age;
        protected string name;
        protected readonly int x;
        protected readonly int y;
        //protected static List<Animal> animals = new List<Animal>();
        protected static Dictionary<string, Animal> animals = new Dictionary<string, Animal>();


        public Animal(int age, string name, int x, int y)
        {
            this.age = age;
            this.name = name;
            this.x = x;
            this.y = y;
        }

        public Animal(Animal animal)
        {
            this.age = animal.age;
            this.name = animal.name;
            this.x = animal.x;
            this.y = animal.y;
            animals.Add(e.c(x, y), this);
        }

        public virtual double calculate()
        {
            double tot = 0;
            foreach (var animal in animals.Values)
            {
                tot += animal.calculate();
            }

            Console.WriteLine($"total value of the farm: {tot}");
            return tot;
        }

        static public bool is_there_animal(int x, int y)
        {
            return animals.ContainsKey(e.c(x, y));
        }

        static public bool kill_bird(int x, int y) 
        {
            string coor = e.c(x, y);
            if (animals.ContainsKey(coor))
            {
                if (animals[coor] is Bird)
                {
                    animals.Remove(coor);
                    return true;
                }
            }

            return false;
        }

        static public bool explode_tree(int x, int y)
        {

            HashSet<string> protected_area = new HashSet<string>();
            Bird bird;
            foreach (var item in animals.Values)
            {
                if (item is Bird)
                {
                    bird = (Bird)item;
                    protected_area.UnionWith(bird.protection_area);
                }
            }

            string coor = e.c(x, y);
            if (animals.ContainsKey(coor))
            {
                if (animals[coor] is Tree)
                {
                    if (!protected_area.Contains(coor))
                    {
                        animals.Remove(coor);
                        return true;
                    }
                }
            }

            return false;
        }

        static public void add_to_protection(Bird bird, HashSet<string> set = null, HashSet<string> visited = null)
        {
            if (visited == null)
            {
                visited = new HashSet<string> ();
            }
            if (set == null)
            {
                set = bird.protection_area;
            }
            Tree tree;
            foreach (var animal in animals.Values)
            {
                if (animal is Tree)
                {
                    tree = (Tree) animal;
                    if (set.Contains(e.c(tree.x, tree.y)) && !visited.Contains(e.c(tree.x, tree.y)))
                    {
                        bird.protection_area.UnionWith(tree.potentials);
                        visited.Add(e.c(tree.x, tree.y));
                        add_to_protection(bird, tree.potentials, visited);
                    }
                }
            }
        }

        static public void update_protection()
        {
            foreach (var animal in animals.Values)
            {
                if (animal is Bird)
                {
                    add_to_protection((Bird)animal);
                }
            }
        }

        static public void show_map()
        {
            Animal animal = null;            
            for (int i = 1; i <= e.n; i++)
            {
                
                for (int j = 1; j <= e.m; j++)
                {
                    if (animals.ContainsKey(e.c(i, j)))
                    {
                        animal = animals[e.c(i, j)];
                        if (animal is Bird)
                        {
                            Console.Write("B ");
                        }
                        else
                        {
                            Console.Write("T ");
                        }
                    }
                    else
                    {
                        Console.Write("O ");
                    }
                }
                Console.Write("\n");
            }
        }

        static public void evaluate()
        {
            foreach (var item in animals.Values)
            {
                Console.WriteLine($"{item.name}: {item.calculate()}");
            }

            new Animal(1,"d",1,1).calculate();
        }
    }

    class Bird : Animal
    {

        public int weight { get; set; }
        public double egg_ave { get; set; }
        public int length { get; set; }
        public int width { get; set; }
        public HashSet<string> protection_area = new HashSet<string>();
        public Bird(int weight, int egg_ave, int length, int width, int age, string name, int x, int y) : base(age, name, x, y)
        {
            this.weight = weight;
            this.egg_ave = egg_ave;
            this.length = length;
            this.width = width;
        }

        public Bird(int weight, double egg_ave, int length, int width, Animal animal) : base(animal)
        {
            this.weight = weight;
            this.egg_ave = egg_ave;
            this.length = length;
            this.width = width;

            for (int i = x - length; i <= x + length; i++)
            {
                for (int j = y - width; j <= y + width; j++)
                {
                    if (x > -1 && x <= e.n && y > -1 && y <= e.m) // it's not important to check the input's ranges
                    {
                        protection_area.Add(e.c(i, j));
                    }
                }
            }
        }

        public override double calculate()
        {
            
            return egg_ave * weight;
        }
    }

    class Tree : Animal
    {

        public int fruit_count { get; set; }
        public int height { get; set; }
        public HashSet<string> potentials = new HashSet<string>();  
        public Tree(int fruit_count, int height, int age, string name, int x, int y) : base(age, name, x, y)
        {
            this.fruit_count = fruit_count;
            this.height = height;
        }

        public Tree(int fruit_count, int height, Animal animal) : base(animal)
        {
            this.fruit_count = fruit_count;
            this.height = height;
            //Animal.animals.Add(this);

            for (int i = x - height; i <= x + height; i++)
            {
                for (int j = y - height; j <= y + height; j++)
                {
                    if (x > -1 && x <= e.n && y > -1 && y <= e.m)
                    {
                        //if (i == x && j == y)
                        //{
                        //    continue;
                        //}
                        potentials.Add(e.c(i, j));
                    }
                }
            }
        }

        public override double calculate()
        {
            return this.age * fruit_count;
        }

    }

    class Gun
    {
        public string name { get; set; }
        public int bullet { get; set; }
        public static Dictionary<string, Gun> guns = new Dictionary<string, Gun>();

        public Gun(string name, int bullet)
        {
            this.name = name;
            this.bullet = bullet;
            guns.Add(name, this);
        }
    }

    class RocketLauncher: Gun
    {
        public int explosion_range;

        public RocketLauncher(string name, int bullet, int xplosion_range) : base(name, bullet)
        {
            explosion_range = xplosion_range;
        }
    }

    internal class Program
    {


        static void Main(string[] args)
        {
            bool flag = false;
            while (true)
            {
                while (!flag)
                {
                    string[] s = Console.ReadLine().Split();
                    try
                    {
                        e.n = int.Parse(s[0]);
                        e.m = int.Parse(s[1]);
                        flag = true;
                    }
                    catch (Exception)
                    {

                        Console.WriteLine("Invalid input");
                    }
                }
                Console.WriteLine("1.add animal, 2. add gun, 3. shoot, 4. show, 5. evaluate 0. exit");
                string cmd = Console.ReadLine();

                if (cmd == "1")
                {
                    add_animal();
                }
                else if (cmd == "2")
                {
                    add_gun();
                }
                else if (cmd == "3")
                {
                    shoot();
                }
                else if (cmd == "4")
                {
                    Animal.show_map();
                }
                else if (cmd == "5")
                {
                    Animal.evaluate();
                }
                else if (cmd == "0")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }
        }

        public static int to_int(string str)
        {
            try
            {
                return int.Parse(str);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static double to_double(string str)
        {
            try
            {
                return double.Parse(str);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static Animal make_animal(string[] strings)
        {
            if (strings.Length < 4)
            {
                Console.WriteLine("Invalid input.");
                return null;
            }
            string name = strings[0];
            int x = to_int(strings[1]);
            int y = to_int(strings[2]);
            int age = to_int(strings[3]);

            if (x < 1 || y < 1 || age < 0 || x > e.n || y > e.m)
            {
                Console.WriteLine("Invalid input");
                return null;
            }

            if (Animal.is_there_animal(x, y))
            {
                Console.WriteLine("There is already an animal at this spot.");
                return null;
            }

            return new Animal(age, name, x, y);
        }

        public static void add_gun()
        {
            Console.WriteLine("1. gun, 2. rocket launcher");
            string cmd = Console.ReadLine();

            if (cmd == "1")
            {
                make_gun();
            }
            else if (cmd == "2")
            {
                make_launcher();
            }
            else
            {
                Console.WriteLine("Invalid input.");
                return;
            }
        }

        public static void make_gun()
        {
            string[] strings = Console.ReadLine().Split();
            if (strings.Length != 2)
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            string name = strings[0];
            int bullet = to_int(strings[1]);
            if (bullet < 1)
            {
                Console.WriteLine("Invalid input");
                return;
            }
            new Gun(name, bullet);
        }

        public static void make_launcher()
        {
            string[] strings = Console.ReadLine().Split();
            if (strings.Length != 3)
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            string name = strings[0];
            int bullet = to_int(strings[1]);
            if (bullet < 1)
            {
                Console.WriteLine("Invalid input");
                return;
            }
            int range = to_int(strings[2]);
            if (range < 1)
            {
                Console.WriteLine("Invalid input");
                return;
            }
            new RocketLauncher(name, bullet, range);
        }

        public static void add_animal()
        {
            Console.WriteLine("1. bird, 2. tree");
            string cmd = Console.ReadLine();

            if (cmd == "1")
            {
                add_bird();
            }
            else if (cmd == "2")
            {
                add_tree();
            }
            else
            {
                Console.WriteLine("Invalid input");
                return;
            }
        }

        public static void add_bird()
        {
            string[] strings = Console.ReadLine().Split();

            if (strings.Length != 8)
            {
                Console.WriteLine("Invalid input");
                return;
            }

            Animal animal = make_animal(strings);
            if (animal == null)
            {
                return;
            }

            int weight = to_int(strings[4]);
            double egg = to_double(strings[5]);
            int x = to_int(strings[6]);
            int y = to_int(strings[7]);
            if (weight < 0 || egg < 0 || x < 0 || y < 0)
            {
                Console.WriteLine("Invalid input");
                return;
            }

            Bird b = new Bird(weight, egg, x, y, animal);
            Animal.add_to_protection(b);
        }

        public static void add_tree()
        {
            string[] strings = Console.ReadLine().Split();

            if (strings.Length != 6)
            {
                Console.WriteLine("Invalid input");
                return;
            }

            Animal animal = make_animal(strings);
            if (animal == null)
            {
                return;
            }

            int height = to_int(strings[4]);
            int fruit_count = to_int(strings[5]);

            if (height < 1 || fruit_count < 0)
            {
                Console.WriteLine("Invalid input");
                return;
            }

            new Tree(fruit_count, height, animal);
            Animal.update_protection();
        }

        public static void shoot()
        {
            Console.WriteLine("name of weapon: ");
            string weapon_str = Console.ReadLine();
            if (!Gun.guns.ContainsKey(weapon_str))
            {
                Console.WriteLine("Invalid name");
                return;
            }
            Gun weapon = Gun.guns[weapon_str];

            if (weapon is RocketLauncher)
            {
                launcher((RocketLauncher)weapon);
            }
            else 
            {
                gun(weapon);
            }
        }

        public static void gun(Gun gun)
        {
            if (gun.bullet == 0)
            {
                Console.WriteLine("This gun has no bullet");
                return;
            }
            string[] coor = Console.ReadLine().Split();

            int x = to_int((string)coor[0]);
            if (x < 1 || x > e.n)
            {
                Console.WriteLine("Invalid input");
                return;
            }

            int y = to_int(((string)coor[1]));
            if (y < 1 || y > e.m)
            {
                Console.WriteLine("Invalid input");
                return;
            }

            Animal.kill_bird(x, y);
            gun.bullet--;
        }

        public static void launcher(RocketLauncher rocket)
        {

            if (rocket.bullet == 0)
            {
                Console.WriteLine("This gun has no bullet");
                return;
            }

            string[] coor = Console.ReadLine().Split();

            int x = to_int((string)coor[0]);
            if (x < 1 || x > e.n)
            {
                Console.WriteLine("Invalid input");
                return;
            }

            int y = to_int(((string)coor[1]));
            if (y < 1 || y > e.m)
            {
                Console.WriteLine("Invalid input");
                return;
            }

            for (int i = x - rocket.explosion_range; i < x + rocket.explosion_range; i++)
            {
                for (int j = y - rocket.explosion_range; j < y + rocket.explosion_range; j++)
                {
                    Animal.kill_bird(i, j);
                }
            }

            for (int i = x - rocket.explosion_range; i < x + rocket.explosion_range; i++)
            {
                for (int j = y - rocket.explosion_range; j < y + rocket.explosion_range; j++)
                {
                    Animal.explode_tree(i, j);
                }
            }

            rocket.bullet--;
        }

    }
}