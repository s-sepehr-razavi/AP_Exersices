namespace _3
{
    internal class Program
    {

        class Employee 
        {
            public string name;
            public string last_name;
            private double income;
            public int id;
            private int employee_id;
            static List<Employee> employees = new List<Employee>();

            public Employee(string last_name, double income, int id, int employee_id = -1)
            {
                this.name = name;
                this.last_name = last_name;
                this.income = income;
                this.id = id;
                this.employee_id = employees.Count;
                bool flag = true;
                for (int i = 0; i < ; i++)
                {
                    if (employees[i].id == id)
                    {
                        flag= false;                        
                    }
                }
                if(flag)
                employees.Add(this);

            }

            public static Employee find_employee(int id)
            {
                foreach (var item in employees)
                {
                    if (item.id == id)
                    {
                        return item;
                    }
                }

                Console.WriteLine("not found");
                return null;
            }

            public int what_is_my_work_ID()
            {
                return employee_id;
            }

            public void employee_comparator(int id1, int id2)
            {
                Employee emp1 = find_employee(id1);
                Employee emp2 = find_employee(id2);

                if (emp1 == null || emp2 == null)
                {
                    Console.WriteLine("Invalid task");
                    return;
                }

                if (emp2.income > emp1.income)
                {
                    Console.WriteLine(emp2.name + " " + emp2.last_name);
                    return;
                }

                if (emp1.income > emp2.income)
                {
                    Console.WriteLine(emp1.name + " " + emp1.last_name);
                    return;
                }

                Console.WriteLine("equal");
                return;

            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}