namespace _5
{

    class Restuarant
    {
        public static int balance;
        public static List<Tuple<int, int>> discounts = new List<Tuple<int, int>>();
    }

    class Customer
    {
        public string name;
        public int id;
        public int balance;
        public int num_special_discount;
        public bool special_discount = false;
        public static List<Customer> customers = new List<Customer>();
        public int discount_code = -1;

        public Customer(string name, int id, bool special_discount)
        {
            this.name = name;
            this.id = id;
            this.special_discount = special_discount;
            customers.Add(this);
        }

        public Customer(string name, int id, int disc_num)
        {
            this.name=name;
            this.id = id;
            this.num_special_discount = disc_num;
            customers.Add(this);
        }
    }

    class Food
    {
        public string name;
        public int price;
        public List<Tuple<string, int>> ingridients = new List<Tuple<string, int>>();
        public static List<Food> menu = new List<Food>();
        public static Dictionary<Food, int> food_count = new Dictionary<Food, int>();

        public Food(string name, int price, List<Tuple<string, int>> ingridients)
        {
            this.name = name;
            this.price = price;
            this.ingridients = ingridients;
        }
    }

    class Warehouse
    {
        public string ingridient_name;
        public int amount;
        public static List<Warehouse> warehouses = new List<Warehouse>();

        public Warehouse(string ingridient_name, int amount)
        {
            this.ingridient_name = ingridient_name;
            this.amount = amount;
            warehouses.Add(this);
        }

        static public bool make_food(Food food, int amount) 
        {
            List<Tuple<string, int>> ingridients = food.ingridients;
            foreach (var item in ingridients)
            {
                foreach (var ware in warehouses)
                {
                    if (ware.ingridient_name == item.Item1 && ware.amount < item.Item2 * amount)
                    {
                        return false;
                    }
                }
            }

            foreach (var item in ingridients)
            {
                foreach (var ware in warehouses)
                {
                    if (ware.ingridient_name == item.Item1)
                    {
                        ware.amount -= amount * item.Item2;
                    }
                }
            }
            Food.food_count[food] += amount;
            return true;
        }
    }

    class Transaction
    {       
        public int transaction_id;
        public int customer_id;
        public int cost;
        public int discount_id;
        public bool done=false;
        public int final;
        public static List<Transaction> transactions = new List<Transaction>();

        public Transaction(int customer_id, int cost, int disount_id)
        {
            this.cost = cost;
            this.customer_id = customer_id;
            this.discount_id = disount_id;

            transactions.Add(this);
            this.transaction_id = transactions.Count;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            //driver();
            //Console.Write("hh: ");
            //string x = Console.ReadLine();
            //Console.Write("df: ");
            //string y = Console.ReadLine();
            //Console.WriteLine(x);
            //Console.WriteLine(y);

            //string[] x = Console.ReadLine().Split(',', ' ');
            //List<string> y = x.ToList();
            //for (int i = 0; i < y.Count; i++)
            //{
            //    if (y[i] == "")
            //    {
            //        y.Remove("");
            //    }
            //}
            //foreach (var item in y)
            //{
            //    Console.WriteLine(item);
            //}
            driver();
        }

        public static int taking_int(string what_to_ask)
        {
            int initial_balance;
            while (true)
            {
                try
                {
                    Console.WriteLine(what_to_ask);
                    initial_balance = int.Parse(Console.ReadLine());
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
        static void add_customer(bool first_time)
        {
            int id = taking_int("ID: ");
            Console.Write("Name: ");
            string name = Console.ReadLine();

            foreach (var c in Customer.customers)
            {
                if (c.id == id)
                {
                    Console.WriteLine("A customer with this ID already exists.");
                    return;
                }
            }

            Customer customer = new Customer(name, id, first_time);
        }

        static void read_txt()
        {
            try
            {
                StreamReader reader;
                reader = new StreamReader("user.txt");
                string[] strings;
                while (reader.EndOfStream == false)
                {
                    strings = reader.ReadLine().Split(", ");
                    Customer customer = new Customer(strings[0], int.Parse(strings[1]), int.Parse(strings[2]));
                }
                reader.Close();

            }
            catch (Exception)
            {

            }
        }

        static void increase_balance()
        {            
            int id = taking_int("ID: ");            
            int amount = taking_int("Amount: ");
            foreach (var c in Customer.customers)
            {
                if (c.id == id)
                {
                    c.balance += amount;
                }
            }
        }

        static void sell_food()
        {
            Console.Write("Food name: ");
            string name = Console.ReadLine();            
            int amount = taking_int("Amount: ");            
            int id = taking_int("ID: ");

            Food food = null;
            foreach (var item in Food.menu)
            {
                if (item.name == name)
                {
                    food = item;
                }
            }

            if (food == null)
            {
                Console.WriteLine("There is no food on the menu with the specified name.");
                return;
            }

            Customer customer = null;
            foreach (var c in Customer.customers)
            {
                if (c.id == id)
                {
                    customer = c;
                    break;
                }
            }

            if (customer == null)
            {
                Console.WriteLine("There is no customer with the given ID.");
                return;
            }

            if (food.price * amount > customer.balance)
            {
                Console.WriteLine("Customer does not have enough money.");
                return;
            }

            if (Food.food_count[food] < amount)
            {
                Console.WriteLine("There is not enough material for this amount of food");
                return;
            }

            Transaction transaction = new Transaction(id, amount * food.price, customer.discount_code);

            //Tuple<int, int> tuple = null;
            //foreach (var item in Restuarant.discounts)
            //{
            //    if (item.Item1 == customer.discount_code)
            //    {
            //        tuple = item;
            //    }
            //}
            //if (tuple == null)
            //{
            //    return;
            //}
            //Restuarant.discounts.Remove(tuple);
            //customer.discount_code = -1;
        }

        static void add_warehouse()
        {
            Console.Write("Material name: ");
            string name = Console.ReadLine();
            foreach (var item in Warehouse.warehouses)
            {
                if (item.ingridient_name == name)
                {
                    Console.WriteLine("This item already exists in the warehouse.");
                    return;
                }
            }            
            int amount = taking_int("Amount: ");
            Warehouse warehouse = new Warehouse(name, amount);
        }

        static void add_new_food()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();
            foreach (var item in Food.menu)
            {
                if (item.name == name)
                {
                    Console.WriteLine("This food already exists.");
                    return;
                }
            }            
            int price = taking_int("Price: ");

            Console.Write("Materials: ");
            try
            {
                string[] mat = Console.ReadLine().Split(' ', ',');
                List<string> list = mat.ToList();
                List<string> list2 = mat.ToList();
                foreach (var item in list2)
                {
                    if (item == "")
                        list.Remove(item);
                }

                List<Tuple<string, int>> l = new List<Tuple<string, int>>();
                for (int i = 0; i < list.Count; i += 2)
                {
                    bool is_it_there = false;
                    foreach (var item in Warehouse.warehouses)
                    {
                        if (item.ingridient_name == list[i] && item.amount >= int.Parse(list[i + 1]))
                        {
                            is_it_there = true;
                            break;
                        }
                    }
                    if (!is_it_there)
                    {
                        Console.WriteLine("The ingridinet could not be found or it was not sufficent.");
                        return;
                    }

                    l.Add(new Tuple<string, int>(list[i], int.Parse(list[i + 1])));
                }

                Food food = new Food(name, price, l);
                Food.menu.Add(food);
                Food.food_count.Add(food, 0);
            }
            catch (Exception)
            {

                Console.WriteLine("Invalid format.");
                return;
            }
            
        }

        static void increase_food()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();            
            int amount = taking_int("Amount: ");

            Food food = null;
            foreach (var item in Food.menu)
            {
                if (item.name == name)
                {
                    food = item;
                }
            }

            if (food == null)
            {
                Console.WriteLine("There is no food on the menu with the specified name.");
                return;
            }

            if (!Warehouse.make_food(food, amount))
            {
                Console.WriteLine("There is not enough ingredient for this amount of food.");
                return;
            }
        }

        static void add_discount()
        {            
            int code = taking_int("Code: ");            
            int amount = taking_int("Amount: ");

            foreach (var item in Restuarant.discounts)
            {
                if (item.Item1 == code)
                {
                    Console.WriteLine("A discount with the same code already exists.");
                    return;
                }
            }

            Restuarant.discounts.Add(new Tuple<int, int> (code, amount));

        }

        static void cust_disc()
        {            
            int code = taking_int("Code: ");            
            int id = taking_int("Customer ID: ");

            Customer customer = null;

            foreach (var c in Customer.customers)
            {
                if (c.id == id)
                {
                    customer = c;
                    break;
                }
            }

            if (customer == null) 
            {
                Console.WriteLine("There is no customer with the given ID.");
                return;
            }

            Tuple<int, int> discount = null;
            foreach (var d in Restuarant.discounts)
            {
                if (d.Item1 == code)
                {
                    discount = d;
                }
            }

            if (discount == null)
            {
                Console.WriteLine("There is no discount with the given code.");
                return;
            }

            customer.discount_code = code;

        }

        static void accept_transaction()
        {            
            int id = taking_int("ID: ");
            Transaction transaction = null;
            foreach (var tran in Transaction.transactions)
            {
                if (tran.transaction_id == id)
                {
                    if (tran.done)
                    {
                        Console.WriteLine("This transaction has been completed.");
                        return;
                    }
                   transaction = tran;
                }
            }

            if (transaction == null)
            {
                Console.WriteLine("There is no transaction with the specified ID.");
                return;
            }

            Customer customer = null;
            foreach(var c in Customer.customers)
            {
                if (c.id == transaction.customer_id) 
                {
                    customer = c;
                    break;
                }
            }

            Tuple<int,int> discount = null;
            foreach (var d in Restuarant.discounts)
            {
                if (d.Item1 == customer.discount_code)
                {
                    discount = d;
                }
            }

            int cost = transaction.cost;
            if (discount != null)
            {
                cost -= discount.Item2;
            }

            
            if (transaction.transaction_id == 1)
            {
                cost = cost * 95 / 100;
                customer.special_discount = false;
                customer.num_special_discount += 1;
            }

            customer.balance -= cost;
            Restuarant.balance += cost;

            transaction.done = true;
            transaction.final = cost;
        }

        static void list_tran()
        {
            foreach(var t in Transaction.transactions)
            {
                if (t.done)
                {
                    Console.WriteLine($"Transaction [{t.transaction_id}]: [{t.customer_id}] [{t.cost}] [{t.discount_id}] [{t.final}]");
                }
            }
        }

        static void write_txt()
        {
            StreamWriter write = new StreamWriter("user.txt");
            foreach (var c in Customer.customers)
            {
                write.WriteLine($"{c.name}, {c.id}, {c.num_special_discount}");
            }
            write.Close();
        }

        //static void read_txt()
        //{
        //    StreamReader reader = new StreamReader("user.txt");
        //    string line;
        //    string[] words;
        //    while ((line = reader.ReadLine()) != null)
        //    {
        //        words = line.Split();

        //    }
        //}

        static void increase_warehouse()
        {
            Console.Write("Material name: ");
            string name = Console.ReadLine();
            Warehouse warehouse = null;
            foreach (var item in Warehouse.warehouses)
            {
                if (item.ingridient_name == name)
                {
                    warehouse = item;
                    break;
                }
            }

            if (warehouse == null)
            {
                Console.WriteLine("There is no item with this name in the warehouse.");
                return;
            }
            int amount = taking_int("Amount: ");            
            warehouse.amount += amount;
        }
        static void driver()
        {
            read_txt();
            bool first_time = true;
            while (true)
            {
                Console.WriteLine("Choose a command (choose a number): 1. add customer, 2. increase balance of customer, 3. sell food, 4. add warehouse material, 5. add food, 6. increase food, 7. add discount code, 8. add discount code to the customer, 9. list transactions, 10. restuarant balance, 11. accept transaction, 12. increase warehouse item, exit");
                string command = Console.ReadLine();
                if (command == "exit")
                {
                    write_txt();
                    return;
                }
                else
                if (command == "1")
                {
                    add_customer(first_time);
                    first_time = false;
                }
                else
                if ((command == "2"))
                {
                    increase_balance();
                }
                else
                if (command == "3")
                {
                    sell_food();
                }
                else
                if (command == "4")
                {
                    add_warehouse();
                }
                else
                if (command == "5")
                {
                    add_new_food();
                }
                else
                if (command == "6")
                {
                    increase_food();
                }
                else
                if (command == "7")
                {
                    add_discount();
                }
                else
                if (command == "8")
                {
                    cust_disc();
                }
                else
                if(command == "9")
                {
                    list_tran();
                }
                else
                if (command == "10")
                {
                    Console.WriteLine(Restuarant.balance);
                }
                else
                if (command == "11")
                {
                    accept_transaction();
                }
                else
                if (command == "12")
                {
                    increase_warehouse();
                }
                else
                {
                    Console.WriteLine("Please enter one of the specifed commands.");
                }
            }
        }
    }
}