using System;
using System.Xml.Linq;

namespace _2
{

    enum Sex
    {
        male = 1,
        female
    }
    
    class Person
    {
        public string SSN { get; set; }
        public string Name { get; set; }
        public string Field { get; set; }
        public Sex Sex { get; set; }
        //public static Dictionary<string, Person> people = new Dictionary<string, Person>();
        public Person(string sSN, string name, string field, Sex sex)
        {
            SSN = sSN;
            Name = name;
            Field = field;
            Sex = sex;

            //if (!people.ContainsKey(sSN))
            //{
            //    people.Add(sSN, this);
            //}
        }
    }

    class Professor : Person
    {
        public int RoomNo { get; set; }
        public int MinTRA { get; set; }
        public Dictionary<string, RA> RAs { get; set; } = new Dictionary<string, RA>();
        static public Dictionary<string, Professor> proffs { get; set; } = new Dictionary<string, Professor>();
        private static HashSet<int> room_nums = new HashSet<int>();

        public Professor(int minTRA, string ssn, string name, string field, Sex sex): base(ssn, name, field, sex)
        {            
            MinTRA = minTRA;

            Random random = new Random();
            do
            {
                RoomNo = random.Next(1, 1000);
            } while (room_nums.Contains(RoomNo));

            proffs.Add(ssn, this);
        }
    }

    // because of ta's and ra's I can't add static list of stu or I should add an if!
    class Student : Person
    {
        public bool role { set; get; }
        public int EnteringYear { set; get; }
        static public Dictionary<string, Student> Students { get; set; } = new Dictionary<string, Student>();

        public Student(int enteringYear, string sSN, string name, string field, Sex sex, bool ta_ra = false) : base(sSN, name, field, sex)
        {
            EnteringYear = enteringYear;
            if (!ta_ra)
            {
                Students.Add(sSN, this);
            }
            else
            {
                Students[sSN].role = true;
            }

        }
    }
    class RA: Student
    {
        public string ProjectName { get;set; }
        public int FreeTime { get; set; }
        public string ProfessorSSN { get; set; }
        public Dictionary<string, RA> RAs { get; set; } = new Dictionary<string, RA>();
        public RA (string projectName, int freeTime, string professorSSN, Student student): base(student.EnteringYear, student.SSN, student.Name, student.Field, student.Sex, true)
        {
            ProjectName = projectName;
            FreeTime = freeTime;
            ProfessorSSN = professorSSN;
            student.role = true;
            RAs.Add(SSN, this);
        }
    }

    class TA: Student
    {
        public int UnitID;
        public Dictionary<string, TA> TAs { get; set; } = new Dictionary<string, TA>();
        public TA(int unitID, Student student): base(student.EnteringYear, student.SSN, student.Name, student.Field, student.Sex, true)
        {
            UnitID = unitID;
            student.role = true;
            TAs.Add(SSN, this);

        }
    }

    class Unit
    {
        public int UnitID { get; set; }
        public string Name { get; set; }
        public string Field { get; set; }
        public int MaxSize { get; set; }
        public Dictionary<string, Student> students { get; set; } = new Dictionary<string, Student>();
        public string ProfessorSSN { get; set; } = "-";
        public Dictionary<string, TA> TAs { get; set; } = new Dictionary<string, TA>();
        public Dictionary<string, double> grades {get; set;} = new Dictionary<string, double>();
        static public Dictionary<int, Unit> units = new Dictionary<int, Unit>();

        public Unit(int unitID, string name, string field, int maxSize)
        {
            UnitID = unitID;
            Name = name;
            Field = field;
            MaxSize = maxSize;            

            units.Add(unitID, this);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Select one of the phases.");
                string cmd = Console.ReadLine();

                if (cmd == "1")
                {
                    phase1();
                }

                else if (cmd == "2")
                {
                    phase2();
                }

                else if (cmd == "3")
                {
                    phase3();
                }
                else if (cmd == "0")
                {
                    return;
                }
                else 
                {
                    Console.WriteLine("Invalid command.");
                }                
            }
        }

        public static void phase1()
        {
            while(true)
            {
                Console.WriteLine("Enter a command: ");
                string[] strings = (Console.ReadLine()).Split();

                if (strings[0] == "register_student")
                {
                    register(strings, true);
                }

                else if (strings[0] == "register_professor")
                {
                    register(strings, false);
                }

                else if (strings[0] == "make_unit")
                {
                    make_unit(strings);
                }
                else if (strings[0] == "0")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid command");
                }
            }
        }

        public static void make_unit(string[] cmds)
        {
            if (cmds.Length != 5)
            {
                Console.WriteLine("Invalid command");
                return;
            }

            int num;
            try
            {
                num = int.Parse(cmds[1]);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid number");
                return;
            }

            if (!(num > 0 && num <= 100000))
            {
                Console.WriteLine("Invalid id.");
                return;
            }

            if (Unit.units.ContainsKey(num))
            {
                Console.WriteLine("A unit with same id already exists.");
                return;
            }

            string name = cmds[2];

            if (!(name.Length >= 3 && name.Length <= 20))
            {
                Console.WriteLine("Invalid name format.");
                return;
            }

            string field = cmds[3];

            if (!(field.Length >= 3 && field.Length <= 20))
            {
                Console.WriteLine("Invalid field format.");
                return;
            }

            int max;
            try
            {
                max = int.Parse(cmds[4]);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid number");
                return;
            }

            if (!(max >= 10 && max <= 180))
            {
                Console.WriteLine("Invalid max count.");
                return;
            }

            new Unit(num, name, field, max);

        }

        public static void register(string[] cmds, bool stu)
        {
            if (!(cmds.Length == 6  || cmds.Length == 5))
            {
                Console.WriteLine("Invalid command");
                return;
            }

            string name = cmds[1];

            if (!(name.Length >= 3 && name.Length <= 20))
            {
                Console.WriteLine("Invalid name format.");
                return;
            }

            string ssn = cmds[2];
            if (ssn.Length != 10)
            {
                Console.WriteLine("SSN should have 10 characters.");
                return;
            }

            if (stu && Student.Students.ContainsKey(ssn))
            {
                Console.WriteLine("A student with the same SSN already exists.");
                return;
            }

            if (!stu && Professor.proffs.ContainsKey(ssn))
            {
                Console.WriteLine("A professor with the same SSN already exists.");
                return;
            }

            string field = cmds[3];

            if (!(field.Length >= 3 && field.Length <= 20))
            {
                Console.WriteLine("Invalid field format.");
                return;
            }            

            if (!Enum.IsDefined(typeof(Sex), cmds[4]))
            {
                Console.WriteLine("Invalid gender.");
                return;
            }

            Sex s;
            Enum.TryParse(cmds[4], out s);

            int num = 0;
            try
            {
                num = int.Parse(cmds[5]);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid number");
                return;
            }

            //if (!(stu && num > 1350 && num <= DateTime.Now.Year))
            //{
            //    Console.WriteLine("Invalid entering year.");
            //    return;
            //}

            //if (!(!stu && num > 0 && num < 57))
            //{
            //    Console.WriteLine("The number of hours does not meet the required conditions.");
            //    return;
            //}

            if (stu)
            {
                if (!(num > 1350 && num <= DateTime.Now.Year))
                {
                    Console.WriteLine("Invalid entering year.");
                    return;
                }
            }
            else
            {
                if (!(num > 0 && num < 57))
                {
                    Console.WriteLine("The number of hours does not meet the required conditions.");
                    return;
                }
            }

            if (stu)
            {
                new Student(num, ssn, name, field, s);
            }
            else
            {
                new Professor(num, ssn, name, field, s);
            }

            
            
        }
        

        public static void phase2()
        {
            while (true)
            {
                Console.WriteLine("Enter a command: ");
                string[] strings = (Console.ReadLine()).Split();

                if (strings[0] == "add_student")
                {
                    add_student(strings);
                }

                else if (strings[0] == "add_professor")
                {
                    add_proff(strings);
                }

                else if (strings[0] == "set_student_ta")
                {
                    set_ta(strings);
                }
                else if (strings[0] == "set_student_ra")
                {
                    set_ra(strings);
                }
                else if (strings[0] == "0")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid command");
                }
            }
        }

        public static void add_student(string[] strings)
        {
            if (strings.Length != 3)
            {
                Console.WriteLine("Invalid command");
                return;
            }
            string ssn = strings[1];
            int unitID;

            try
            {
                unitID = int.Parse(strings[2]);
            }
            catch (Exception)
            {
                Console.WriteLine("Please enter a number for unit ID");
                return;
            }

            if (!Student.Students.ContainsKey(ssn))
            {
                Console.WriteLine("There is no student with the specified SSN");
                return;
            }

            Student student = Student.Students[ssn];

            if (!Unit.units.ContainsKey(unitID))
            {
                Console.WriteLine("There is no unit with the specified ID");
                return;
            }

            Unit unit = Unit.units[unitID];

            if (unit.Field != student.Field)
            {
                Console.WriteLine("Student's and unit's field does not match.");
                return;
            }

            if (unit.students.Count == unit.MaxSize)
            {
                Console.WriteLine("There is no capacity for new students.");
                return;
            }

            if (unit.students.ContainsKey(ssn))
            {
                Console.WriteLine("This student has already taken this course");
                return;
            }
            unit.students.Add(ssn, student);
        }


        public static void add_proff(string[] strings)
        {
            if (strings.Length != 3)
            {
                Console.WriteLine("Invalid command");
                return;
            }
            string ssn = strings[1];
            int unitID;

            try
            {
                unitID = int.Parse(strings[2]);
            }
            catch (Exception)
            {
                Console.WriteLine("Please enter a number for unit ID");
                return;
            }

            if (!Professor.proffs.ContainsKey(ssn))
            {
                Console.WriteLine("There is no professor with the specified SSN");
                return;
            }

            Professor professor = Professor.proffs[ssn];

            if (!Unit.units.ContainsKey(unitID))
            {
                Console.WriteLine("There is no unit with the specified ID");
                return;
            }

            Unit unit = Unit.units[unitID];

            if (unit.Field != professor.Field)
            {
                Console.WriteLine("Professor's and unit's field does not match.");
                return;
            }

            if (unit.ProfessorSSN != "-")
            {
                Console.WriteLine("This unit already have a professor.");
                return;
            }

            unit.ProfessorSSN = professor.SSN;
        }

        public static void set_ta(string[] strings)
        {
            if (strings.Length != 3)
            {
                Console.WriteLine("Invalid command");
                return;
            }
            string ssn = strings[1];
            int unitID;

            try
            {
                unitID = int.Parse(strings[2]);
            }
            catch (Exception)
            {
                Console.WriteLine("Please enter a number for unit ID");
                return;
            }

            if (!Student.Students.ContainsKey(ssn))
            {
                Console.WriteLine("There is no student with the specified SSN");
                return;
            }

            Student student = Student.Students[ssn];

            if (!Unit.units.ContainsKey(unitID))
            {
                Console.WriteLine("There is no unit with the specified ID");
                return;
            }

            Unit unit = Unit.units[unitID];

            if (unit.Field != student.Field)
            {
                Console.WriteLine("Student's and unit's field does not match.");
                return;
            }

            if (unit.ProfessorSSN == "-")
            {
                Console.WriteLine("The unit must be assinged to a professor.");
                return;
            }

            if (student.role)
            {
                Console.WriteLine("This student has already taken a role.");
                return;
            }

            TA ta = new TA(unitID, student);
            unit.TAs.Add(ssn, ta);
        }

        public static void set_ra(string[] strings)
        {
            if (strings.Length != 5)
            {
                Console.WriteLine("Invalid command");
                return;
            }
            string ssn_stu = strings[1];
            string ssn_proff = strings[2];
            string project = strings[3];

            if (!(project.Length >= 1 && project.Length <= 30))
            {
                Console.WriteLine("Invalid name format.");
                return;
            }

            int time_in_week;

            try
            {
                time_in_week = int.Parse(strings[4]);
            }
            catch (Exception)
            {
                Console.WriteLine("Please enter a number for unit ID");
                return;
            }

            if (!Student.Students.ContainsKey(ssn_stu))
            {
                Console.WriteLine("There is no student with the specified SSN");
                return;
            }

            Student student = Student.Students[ssn_stu];

            if (!Professor.proffs.ContainsKey(ssn_proff))
            {
                Console.WriteLine("There is no professor with the specified SSN"); ;
                return;
            }

            Professor proff = Professor.proffs[ssn_proff];

            if (proff.Field != student.Field)
            {
                Console.WriteLine("Student's and professor's field does not match.");
                return;
            }

            if (proff.MinTRA > time_in_week)
            {
                Console.WriteLine("Time considered for this project should be more that " + time_in_week);
                return;
            }

            if (student.role)
            {
                Console.WriteLine("This student has already taken a role.");
                return;
            }

            RA ra = new RA(project, time_in_week, proff.SSN, student);
            proff.RAs.Add(ssn_stu, ra);
        }

        public static void phase3()
        {
            while (true)
            {
                Console.WriteLine("Enter a command: ");
                string[] strings = (Console.ReadLine()).Split();

                if (strings[0] == "unit_status")
                {
                    unit_status(strings);
                }

                else if (strings[0] == "set_final_mark")
                {
                    set_grade(strings);
                }

                else if (strings[0] == "mark_list")
                {
                    grades_list(strings);
                }                
                else if (strings[0] == "0")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid command");
                }
            }
        }

        public static void set_grade(string[] strings)
        {
            if (strings.Length != 5)
            {
                Console.WriteLine("Invalid command");
                return;
            }

            int unitID;

            try
            {
                unitID = int.Parse(strings[3]);
            }
            catch (Exception)
            {
                Console.WriteLine("Please enter a number for unit ID");
                return;
            }

            if (!Unit.units.ContainsKey(unitID))
            {
                Console.WriteLine("There is no unit with the given ID");
                return;
            }

            Unit unit = Unit.units[unitID];

            string ssn_stu = strings[1];

            if (!unit.students.ContainsKey(ssn_stu))
            {
                Console.WriteLine("There is no student enrolled in this course with the given SSN");
                return;
            }

            string ssn_proff = strings[2];

            if (unit.ProfessorSSN != ssn_proff)
            {
                Console.WriteLine("Wrong SSN for the professor");
                return;
            }

            double grade;

            try
            {
                grade = double.Parse(strings[4]);
            }
            catch (Exception)
            {
                Console.WriteLine("Please enter a number for mark");
                return;
            }

            if (!(grade >= 0 && grade <= 20))
            {
                Console.WriteLine("Grade should be a number between 0 and 20");
                return;
            }

            unit.grades.Add(ssn_stu, grade);
        }
        
        public static void unit_status(string[] strings)
        {
            if (strings.Length != 2)
            {
                Console.WriteLine("Invalid command");
                return;
            }

            int unitID;

            try
            {
                unitID = int.Parse(strings[1]);
            }
            catch (Exception)
            {
                Console.WriteLine("Please enter a number for unit ID");
                return;
            }

            if (!Unit.units.ContainsKey(unitID))
            {
                Console.WriteLine("There is no unit with the given ID");
                return;
            }

            Unit unit = Unit.units[unitID];

            Console.WriteLine($"{(unit.ProfessorSSN != "-" ?Professor.proffs[unit.ProfessorSSN].Name: "None")} {unit.MaxSize} {unit.Field}");

            foreach (var item in unit.students.Values)
            {
                Console.Write(item.Name + " ");
            }
            Console.WriteLine();

            foreach (var item in unit.TAs.Values)
            {
                Console.Write(item.Name + " ");
            }
            Console.WriteLine();
        }

        public static void grades_list(string[] strings)
        {
            if (strings.Length != 2)
            {
                Console.WriteLine("Invalid command");
                return;
            }

            int unitID;

            try
            {
                unitID = int.Parse(strings[1]);
            }
            catch (Exception)
            {
                Console.WriteLine("Please enter a number for unit ID");
                return;
            }

            if (!Unit.units.ContainsKey(unitID))
            {
                Console.WriteLine("There is no unit with the given ID");
                return;
            }

            Unit unit = Unit.units[unitID];

            if (unit.students.Count == 0)
            {
                Console.WriteLine("No student");
                return;
            }

            foreach (var item in unit.grades.Keys)
            {
                Console.WriteLine($"{item}: {unit.grades[item]}");
            }
        }

    }

    
}