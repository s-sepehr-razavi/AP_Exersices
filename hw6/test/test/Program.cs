/*using System;
using System.Collections.Generic;
using System.Data.SQLite;
using static System.Data.Entity.Infrastructure.Design.Executor;
using System.Diagnostics;

public class Customer
{
    public string name { set; get; }
    public string lastName { set; get; }
    public string id { set; get; }
    public string username { get; set; }
    public string email { get; set; }
    public string password { get; set; }
}

public class Employee
{
    public string name { set; get; }
    public string lastName { set; get; }
    public string id { set; get; }
    public string username { get; set; }
    public string email { get; set; }
    public string password { get; set; }
}

public class DataAccess
{
    private const string connectionString = "Data Source=mydatabase.db;Version=3;";

    public void CreateTables()
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // Create Table 1: Customers
            string createCustomersTableSql = "CREATE TABLE IF NOT EXISTS Customers (Name TEXT, LastName TEXT, Id TEXT PRIMARY KEY, Username TEXT, Email TEXT, Password TEXT)";
            using (SQLiteCommand createCustomersTableCommand = new SQLiteCommand(createCustomersTableSql, connection))
            {
                createCustomersTableCommand.ExecuteNonQuery();
            }

            // Create Table 2: Employees
            string createEmployeesTableSql = "CREATE TABLE IF NOT EXISTS Employees (Name TEXT, LastName TEXT, Id TEXT PRIMARY KEY, Username TEXT, Email TEXT, Password TEXT)";
            using (SQLiteCommand createEmployeesTableCommand = new SQLiteCommand(createEmployeesTableSql, connection))
            {
                createEmployeesTableCommand.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void InsertCustomer(Customer customer)
    {
        if (CustomerExists(customer.id))
        {
            return;
        }
        
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "INSERT INTO Customers (Name, LastName, Id, Username, Email, Password) VALUES (@Name, @LastName, @Id, @Username, @Email, @Password)";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", customer.name);
                command.Parameters.AddWithValue("@LastName", customer.lastName);
                command.Parameters.AddWithValue("@Id", customer.id);
                command.Parameters.AddWithValue("@Username", customer.username);
                command.Parameters.AddWithValue("@Email", customer.email);
                command.Parameters.AddWithValue("@Password", customer.password);

                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void InsertEmployee(Employee employee)
    {
        if (EmployeeExists(employee.id))
        {
            return;
        }
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "INSERT INTO Employees (Name, LastName, Id, Username, Email, Password) VALUES (@Name, @LastName, @Id, @Username, @Email, @Password)";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", employee.name);
                command.Parameters.AddWithValue("@LastName", employee.lastName);
                command.Parameters.AddWithValue("@Id", employee.id);
                command.Parameters.AddWithValue("@Username", employee.username);
                command.Parameters.AddWithValue("@Email", employee.email);
                command.Parameters.AddWithValue("@Password", employee.password);

                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public List<Customer> RetrieveCustomers()
    {
        List<Customer> customers = new List<Customer>();

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT * FROM Customers";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.name = reader.GetString(0);
                        customer.lastName = reader.GetString(1);
                        customer.id = reader.GetString(2);
                        customer.username = reader.GetString(3);
                        customer.email = reader.GetString(4);
                        customer.password = reader.GetString(5);

                        customers.Add(customer);
                    }
                }
            }

            connection.Close();
        }

        return customers;
    }

    public List<Employee> RetrieveEmployees()
    {
        List<Employee> employees = new List<Employee>();

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT * FROM Employees";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee employee = new Employee();
                        employee.name = reader.GetString(0);
                        employee.lastName = reader.GetString(1);
                        employee.id = reader.GetString(2);
                        employee.username = reader.GetString(3);
                        employee.email = reader.GetString(4);
                        employee.password = reader.GetString(5);

                        employees.Add(employee);
                    }
                }
            }

            connection.Close();
        }

        return employees;
    }

    private bool CustomerExists(string postId)
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT COUNT(*) FROM Customers WHERE ID = @ID";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ID", postId);

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }
    }

    private bool EmployeeExists(string postId)
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT COUNT(*) FROM Employees WHERE ID = @ID";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ID", postId);

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }
    }

}

    public class Program
{
    static void Main(string[] args)
    {
        DataAccess dataAccess = new DataAccess();
        dataAccess.CreateTables();

        //// Insert a customer
        Customer customer = new Customer()
        {
            name = "John",
            lastName = "Doe",
            id = "123456",
            username = "johndoe",
            email = "johndoe@example.com",
            password = "password"
        };
        dataAccess.InsertCustomer(customer);

        // Insert an employee
        Employee employee = new Employee()
        {
            name = "Jane",
            lastName = "Smith",
            id = "987654",
            username = "janesmith",
            email = "janesmith@example.com",
            password = "password"
        };
        dataAccess.InsertEmployee(employee);

        // Retrieve customers
        List<Customer> retrievedCustomers = dataAccess.RetrieveCustomers();
        foreach (Customer retrievedCustomer in retrievedCustomers)
        {
            Console.WriteLine($"Name: {retrievedCustomer.name} {retrievedCustomer.lastName}, ID: {retrievedCustomer.id}, Username: {retrievedCustomer.username}, Email: {retrievedCustomer.email}, Password: {retrievedCustomer.password}");
        }

        // Retrieve employees
        List<Employee> retrievedEmployees = dataAccess.RetrieveEmployees();
        foreach (Employee retrievedEmployee in retrievedEmployees)
        {
            Console.WriteLine($"Name: {retrievedEmployee.name} {retrievedEmployee.lastName}, ID: {retrievedEmployee.id}, Username: {retrievedEmployee.username}, Email: {retrievedEmployee.email}, Password: {retrievedEmployee.password}");
        }

        Console.ReadLine();
    }
}
*/


using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Net.Mail;
using System.Net;
using System.Text.Json;

class Program
{
    static string connectionString = "Data Source=posts.db;Version=3;";

    static public void CreateTables()
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // Create Table 1: Customers
            string createCustomersTableSql = "CREATE TABLE IF NOT EXISTS Customers (Name TEXT, LastName TEXT, Id TEXT PRIMARY KEY, Username TEXT, Email TEXT, Password TEXT, Phonenumber TEXT, AccountBalance REAL)";
            using (SQLiteCommand createCustomersTableCommand = new SQLiteCommand(createCustomersTableSql, connection))
            {
                createCustomersTableCommand.ExecuteNonQuery();
            }

            // Create Table 2: Employees
            string createEmployeesTableSql = "CREATE TABLE IF NOT EXISTS Employees (Name TEXT, LastName TEXT, Id TEXT PRIMARY KEY, Username TEXT, Email TEXT, Password TEXT)";
            using (SQLiteCommand createEmployeesTableCommand = new SQLiteCommand(createEmployeesTableSql, connection))
            {
                createEmployeesTableCommand.ExecuteNonQuery();
            }

            // Create Table 3: Posts
            string createPostsTableSql = "CREATE TABLE IF NOT EXISTS Posts (RecieverAddress TEXT, SenderAddress TEXT, Content TEXT, Expensive INTEGER, Weight REAL, PhoneNumber TEXT, Express INTEGER, ID INTEGER PRIMARY KEY AUTOINCREMENT, SSN TEXT, Price TEXT, CustomerOpinion TEXT, PostStatus TEXT)";
            using (SQLiteCommand createPostsTableCommand = new SQLiteCommand(createPostsTableSql, connection))
            {
                createPostsTableCommand.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public static void SendEmail(string email, string input, string subject)
    {
        string senderEmail = "asgharshakib51@gmail.com"; // enter sender email
        string senderPassword = "euzgrxithgrupvqb"; // enter sender password        
        string body = input;

        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(senderEmail);
        mail.To.Add(new MailAddress(email));
        mail.Subject = subject;
        mail.Body = body;

        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
        smtpClient.EnableSsl = true;

        try
        {
            smtpClient.Send(mail);
            Console.WriteLine("Email sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to send email. Error: " + ex.Message);
        }
    }

    static void Main()
    {

        SendEmail("s.sepehr.razavi@gmail.com", "blah blah blah", "some bs");

        // Create and insert sample posts
        //CreateTables();
        //List<Customer> customers = RetrieveCustomers();
        ////Customer customer = customers[0];
        ////customer.AccountBalance = 100;
        ////InsertCustomer(customer);
        ////UpdateCustomer(customer);
        //JsonSerializerOptions j = new JsonSerializerOptions();
        //j.WriteIndented = true;
        //foreach (var item in customers)
        //{
        //    Console.WriteLine(JsonSerializer.Serialize(item, j));
        //}
        ////InsertPost(post1);
        //InsertPost(post2);

        //UpdatePost(post1);


        // Retrieve and display all posts
        //List<Post> posts = RetrievePosts();

        //Console.WriteLine("Retrieved Posts:");
        //foreach (Post post in posts)
        //{
        //    Console.WriteLine($"ID: {post.id}");
        //    Console.WriteLine($"Receiver Address: {post.recieverAddress}");
        //    Console.WriteLine($"Sender Address: {post.senderAddress}");
        //    Console.WriteLine($"Content: {post.content}");
        //    Console.WriteLine($"Expensive: {post.expensive}");
        //    Console.WriteLine($"Weight: {post.weight}");
        //    Console.WriteLine($"Phone Number: {post.phonenumber}");
        //    Console.WriteLine($"Express: {post.express}");
        //    Console.WriteLine($"Sender ID: {post.senderID}");
        //    Console.WriteLine($"SSN: {post.SSN}");
        //    Console.WriteLine($"Price: {post.price}");
        //    Console.WriteLine($"Customer Opinion: {post.CustomerOpinion}");
        //    Console.WriteLine($"Post Status: {post.postStatus}");
        //    Console.WriteLine();
        //}

        //Console.ReadLine();
    }

    public static void InsertPost(Post post)
    {
        if (PostExists(post.id))
        {
            Console.WriteLine("Post with the same ID already exists.");
            return;
        }

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "INSERT INTO Posts (RecieverAddress, SenderAddress, Content, Expensive, Weight, PhoneNumber, Express, ID, SenderID, SSN, Price, CustomerOpinion, PostStatus) VALUES (@RecieverAddress, @SenderAddress, @Content, @Expensive, @Weight, @PhoneNumber, @Express, @ID, @SenderID, @SSN, @Price, @CustomerOpinion, @PostStatus)";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@RecieverAddress", post.recieverAddress);
                command.Parameters.AddWithValue("@SenderAddress", post.senderAddress);
                command.Parameters.AddWithValue("@Content", post.content.ToString());
                command.Parameters.AddWithValue("@Expensive", post.expensive ? 1 : 0);
                command.Parameters.AddWithValue("@Weight", post.weight);
                command.Parameters.AddWithValue("@PhoneNumber", post.phonenumber);
                command.Parameters.AddWithValue("@Express", post.express ? 1 : 0);
                command.Parameters.AddWithValue("@ID", post.id);
                command.Parameters.AddWithValue("@SenderID", post.senderID);
                command.Parameters.AddWithValue("@SSN", post.SSN);
                command.Parameters.AddWithValue("@Price", post.price);
                command.Parameters.AddWithValue("@CustomerOpinion", post.CustomerOpinion);
                command.Parameters.AddWithValue("@PostStatus", post.postStatus.ToString());

                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    static private bool PostExists(int postId)
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT COUNT(*) FROM Posts WHERE ID = @ID";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ID", postId);

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }
    }

    public static void UpdatePost(Post post)
    {

        if (!PostExists(post.id))
        {
            return;
        }

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "UPDATE Posts SET RecieverAddress = @RecieverAddress, SenderAddress = @SenderAddress, Content = @Content, Expensive = @Expensive, Weight = @Weight, PhoneNumber = @PhoneNumber, Express = @Express, SSN = @SSN, Price = @Price, CustomerOpinion = @CustomerOpinion, PostStatus = @PostStatus WHERE ID = @ID";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@RecieverAddress", post.recieverAddress);
                command.Parameters.AddWithValue("@SenderAddress", post.senderAddress);
                command.Parameters.AddWithValue("@Content", post.content.ToString());
                command.Parameters.AddWithValue("@Expensive", post.expensive ? 1 : 0);
                command.Parameters.AddWithValue("@Weight", post.weight);
                command.Parameters.AddWithValue("@PhoneNumber", post.phonenumber);
                command.Parameters.AddWithValue("@Express", post.express ? 1 : 0);
                command.Parameters.AddWithValue("@SSN", post.SSN);
                command.Parameters.AddWithValue("@Price", post.price);
                command.Parameters.AddWithValue("@CustomerOpinion", post.CustomerOpinion);
                command.Parameters.AddWithValue("@PostStatus", post.postStatus.ToString());
                command.Parameters.AddWithValue("@ID", post.id);

                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public static List<Post> RetrievePosts()
    {
        List<Post> posts = new List<Post>();

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT * FROM Posts";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Post post = new Post();
                        post.recieverAddress = reader.GetString(0);
                        post.senderAddress = reader.GetString(1);
                        post.content = (Content)Enum.Parse(typeof(Content), reader.GetString(2));
                        post.expensive = reader.GetInt32(3) == 1;
                        post.weight = reader.GetDouble(4);
                        post.phonenumber = reader.GetString(5);
                        post.express = reader.GetInt32(6) == 1;
                        post.id = reader.GetInt32(7);
                        post.senderID = reader.GetString(8);
                        post.SSN = reader.GetString(9);
                        post.price = reader.GetString(10);
                        post.CustomerOpinion = reader.GetString(11);
                        post.postStatus = (PostStatus)Enum.Parse(typeof(PostStatus), reader.GetString(12));

                        posts.Add(post);
                    }
                }
            }

            connection.Close();
        }

        return posts;
    }

    static public void InsertCustomer(Customer customer)
    {
        if (CustomerExists(customer.id))
        {
            return;
        }

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "INSERT INTO Customers (Name, LastName, Id, Username, Email, Password, Phonenumber, AccountBalance) VALUES (@Name, @LastName, @Id, @Username, @Email, @Password, @Phonenumber, @AccountBalance)";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", customer.name);
                command.Parameters.AddWithValue("@LastName", customer.lastName);
                command.Parameters.AddWithValue("@Id", customer.id);
                command.Parameters.AddWithValue("@Username", customer.username);
                command.Parameters.AddWithValue("@Email", customer.email);
                command.Parameters.AddWithValue("@Password", customer.password);
                Console.WriteLine($"cust phone {customer.phonenumber}");
                command.Parameters.AddWithValue("@Phonenumber", customer.phonenumber);
                command.Parameters.AddWithValue("@AccountBalance", customer.AccountBalance);


                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    static public List<Customer> RetrieveCustomers()
    {
        List<Customer> customers = new List<Customer>();

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT * FROM Customers";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.name = reader.GetString(0);
                        customer.lastName = reader.GetString(1);
                        customer.id = reader.GetString(2);
                        customer.username = reader.GetString(3);
                        customer.email = reader.GetString(4);
                        customer.password = reader.GetString(5);
                        customer.phonenumber = reader.GetString(6);
                        customer.AccountBalance = reader.GetDouble(7);

                        customers.Add(customer);
                    }
                }
            }

            connection.Close();
        }

        return customers;
    }

    static private bool CustomerExists(string postId)
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "SELECT COUNT(*) FROM Customers WHERE ID = @ID";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ID", postId);

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }
    }

    public static void UpdateCustomer(Customer customer)
    {
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string sql = "UPDATE Customers SET Name = @Name, LastName = @LastName, Username = @Username, Email = @Email, Password = @Password, PhoneNumber = @PhoneNumber, AccountBalance = @AccountBalance WHERE ID = @ID";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", customer.name);
                command.Parameters.AddWithValue("@LastName", customer.lastName);
                command.Parameters.AddWithValue("@Username", customer.username);
                command.Parameters.AddWithValue("@Email", customer.email);
                command.Parameters.AddWithValue("@Password", customer.password);
                command.Parameters.AddWithValue("@PhoneNumber", customer.phonenumber);
                Console.WriteLine($"ph:{customer.phonenumber}");
                command.Parameters.AddWithValue("@AccountBalance", customer.AccountBalance);
                Console.WriteLine($"acc: {customer.AccountBalance}");
                command.Parameters.AddWithValue("@ID", customer.id);

                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }


    public class Post
    {
        public string recieverAddress { get; set; }
        public string senderAddress { get; set; }
        public Content content { get; set; }
        public bool expensive { get; set; }
        public double weight { get; set; }
        public string phonenumber { get; set; }
        public bool express { get; set; }
        public int id { get; set; }
        public string senderID { get; set; }
        public string SSN { get; set; }
        public string price { get; set; }
        public string CustomerOpinion { get; set; }
        public PostStatus postStatus { get; set; }
    }

    public class Customer
    {
        public string name { set; get; }
        public string lastName { set; get; }
        public string id { set; get; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phonenumber { get; set; }
        public double AccountBalance { get; set; }
        public static List<Customer> customers { get; set; } = new List<Customer>();

        public Customer()
        {

        }

        public Customer(string name, string lastName, string email, string id, string phonenumber)
        {
            this.name = name;
            this.lastName = lastName;
            this.email = email;
            this.id = id;
            this.phonenumber = phonenumber;
            this.AccountBalance = 0;


            Random randomPass = new Random();
            string password = randomPass.Next(10000000, 100000000).ToString();
            this.password = password;

            Random random = new Random();
            while (true)
            {
                int rand = random.Next(0, 10000);
                bool flag = true;

                foreach (var item in customers)
                {
                    if (item.username == "user" + rand.ToString())
                    {
                        flag = false; break;
                    }
                }

                if (flag)
                {
                    this.username = "user" + rand.ToString();
                    break;
                }
            }

            customers.Add(this);
        }

        static public Customer findCustomer(string id)
        {
            foreach (var item in customers)
            {
                if (item.id == id)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
    public enum Content
{
    Object,
    Document,
    Fragile
}

public enum PostStatus
{
    Registered,
    ReadyToSend,
    Sending,
    Delivered
}
