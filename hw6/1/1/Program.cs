using System.Drawing;

namespace _1
{

    interface IProcessor
    {
        string compute(string str1, string str2);
    }

    interface IMemory
    {
        string Load(string address);
        void Store(string address, string data);
    }

    interface IDisplay
    {
        void display(bool error, string str);
    }

    class IntegerCPU : IProcessor
    {
        public string compute(string str1, string str2)
        {
            int x = int.Parse(str1);
            int y = int.Parse(str2);

            return (x + y).ToString();
        }
    }

    class StringCPU : IProcessor
    {
        public string compute(string str1, string str2)
        {
            return (str1 + str2).ToString();
        }
    }

    class StaticMemory : IMemory
    {
        string[] memory;
        public StaticMemory(int size)
        {
            this.memory = new string[size];
        }
        public string Load(string address)
        {
            int add;

            try
            {
                add = int.Parse(address);
                return memory[add];
            }
            catch (Exception e)
            {

                throw new ArgumentException("The address is not valid.", e);
            }


        }

        public void Store(string address, string data)
        {
            int add;

            try
            {
                add = int.Parse(address);
                memory[add] = data;
            }
            catch (Exception e)
            {

                throw new ArgumentException("The address is not valid.", e);
            }

        }
    }

    class DynamicMemory : IMemory
    {
        List<string> memory = new List<string>();
        
        public string Load(string address)
        {
            int add;

            try
            {
                add = int.Parse(address);
                return memory[add];
            }
            catch (Exception e)
            {

                throw new ArgumentException("The address is not valid.", e);
            }


        }

        public void Store(string address, string data)
        {
            int add;

            try
            {
                add = int.Parse(address);
                memory[add] = data;
            }
            catch (Exception e)
            {

                throw new ArgumentException("The address is not valid.", e);
            }

        }
    }

    class RGBDisplayer: IDisplay
    {
        ConsoleColor color;

        public RGBDisplayer(ConsoleColor color)
        {
            this.color = color;
        }

        public void display(bool error, string str)
        {
            Console.ForegroundColor = color;
            if (error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: ");
            }  
            Console.WriteLine(str);
        }
    }

    class BlackAndWhiteDisplayer : IDisplay
    {
        public void display(bool error, string str)
        {         
            if (error)
            {
         
                Console.Write("Error: ");
            }
            Console.WriteLine(str);
        }
    }

    class Computer
    {
        public IDisplay display;
        public IMemory memory;
        public IProcessor processor;

        public Computer(IDisplay display, IMemory memory, IProcessor processor)
        {
            this.display = display;
            this.memory = memory;
            this.processor = processor;
        }

        public void RunCommand(string str1, string str2, string address, bool l=false)
        {
            if (l)
            {
                str1 = memory.Load(str1);
                str2 = memory.Load(str2);
            }
            string result = processor.compute(str1, str2);
            memory.Store(address, result);
        }
        
        public void print(string str, bool err)
        {
            display.display(err, memory.Load(str));
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Computer cmp = new Computer(new RGBDisplayer(ConsoleColor.Cyan), new StaticMemory(4), new StringCPU());
            cmp.RunCommand("1", "1", "0");
            cmp.RunCommand("0", "0", "0", true);
            cmp.RunCommand("2", "2", "1");
            cmp.RunCommand("1", "1", "1", true);
            cmp.RunCommand("3", "3", "2");
            cmp.RunCommand("2", "2", "2", true);
            cmp.RunCommand("4", "4", "3");
            cmp.RunCommand("3", "3", "3", true);

            cmp.RunCommand("0", "1", "0", true);
            cmp.RunCommand("0", "2", "0", true);
            cmp.RunCommand("0", "3", "0", true);

            cmp.print("0", false);




        }
    }
}