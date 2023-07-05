using System.Data;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace _2
{
    enum Tuition
    {
        tuition_free = 0,
        tuition_based = 1,
    }

    enum Credit
    {
        General = 2200,
        Lab = 7000,
        Final_Project = 5620,
        Internship = 4100
    }

    class Field
    {
        public int id { get; }
        public string name { get; set; }
        public int total_units { get; set; }
        public Dictionary<int, Student> students { get; } = new Dictionary<int, Student>();  
        public Dictionary<int, Course> courses { get; } = new Dictionary<int, Course>();
        static int count = 0;

        public Field(int id, string name, int total_units)
        {
            this.name = name;            
            this.total_units = total_units;
            this.id = id;
        }
    }

    class University
    {
        int id;
        string name;
        public Dictionary<int, Field> fields { get; } = new Dictionary<int, Field>();
        public Dictionary<int, Proffesor> proffs { get; } = new Dictionary<int, Proffesor>();
        public Dictionary<int, Student> students { get; } = new Dictionary<int, Student>();
        public List<Semester> semesters { get; } = new List<Semester>();

        
        public Semester find_semester(int year, Season season)
        {            
            foreach (var item in semesters)
            {
                if (item.id == season && item.year == year)
                    return item;
            }

            return null;
        }

    }
    class FieldWrapper
    {
        public Field content;
    }

    class PhoneWrapper
    {
        public string number;
    }
    enum Gender
    {
        male = 1,
        female = 2
    }
    struct Student
    {
        public string name;
        public Tuition tuition;
        public string last_name;
        public int age;
        public Gender gender;
        public PhoneWrapper phone_number = new PhoneWrapper();
        //public Field field;
        public FieldWrapper field = new FieldWrapper();
        public int id;
        public University uni; 
        public Dictionary<int, Course> courses { get; } = new Dictionary<int, Course> ();

        public Student(string name, string last_name, int age, Gender gender, string phone_number, Field field, int id, University university, Tuition tuition)
        {
            this.name = name;
            this.last_name = last_name;
            this.age = age;
            this.gender = gender;
            this.phone_number.number = phone_number;
            this.field.content = field;
            this.id = id;
            uni = university;
            this.tuition = tuition;
        }

        public void EditProfile(int id)
        {                        
            this.field.content = uni.fields[id];
        }

        public void EditProfile(string phone)
        {
            phone_number.number = phone;
        }
    }

    struct Proffesor
    {
        string name;
        string last_name;
        int age;
        Gender gender;
        //public string phone_number { get; set; }
        public PhoneWrapper phone_number;
        int id;
        public Dictionary<int, Course> courses { get; } = new Dictionary<int, Course>();

        public Proffesor(string name, string last_name, int age, Gender gender, string phone_number, int id) : this()
        {
            this.name = name;
            this.last_name = last_name;
            this.age = age;
            this.gender = gender;
            this.phone_number.number = phone_number;
            this.id = id;
            
        }

        public void EditPhoneNumber(string number)
        {
            phone_number.number = number;
        }
    }

    [Flags]
    public enum Days
    {
        None = 0b_0000_0000,  // 0
        monday = 0b_0000_0001,  // 1
        tuesday = 0b_0000_0010,  // 2
        wednesday = 0b_0000_0100,  // 4
        thursday = 0b_0000_1000,  // 8
        friday = 0b_0001_0000,  // 16
        saturday = 0b_0010_0000,  // 32
        sunday = 0b_0100_0000
    }
    struct ClassTime
    {
        Days day;
        double start;
        
        public bool does_it_conflict(ClassTime time)
        {
            if ((time.day & this.day) == 0)
            {
                return false;
            }

            if (time.start >= this.start && this.start + 1.5 >= time.start)
            {
                return true;
            }

            return false;
        }

        public ClassTime(Days day, double start)
        {
            this.day = day;
            this.start = start;
        }
    }

    class Course
    {
        public int id { get; }
        public int capacity { get; }
        public string name { get; set; }
        public int units { get; set; }
        public ClassTime time { get; set; }
        public Credit credit { get; set; }
        public Proffesor proffesor { get; set; }
        public Semester semester { get; set; }
        public Dictionary<int, Student> students { get; } = new Dictionary<int, Student>();
        public Dictionary<Student, double> grades { get; } = new Dictionary<Student, double>();

        public Course(int id, string name, int units, ClassTime time, Credit credit, Proffesor proffesor, Semester semester)
        {
            this.id = id;
            this.name = name;
            this.units = units;
            this.time = time;
            this.credit = credit;
            this.proffesor = proffesor;
            this.semester = semester;
            this.capacity = 30;
        }
    }

    enum Season
    {
        Fall = 1,
        Spring,
        Summer
    }
    class Semester
    {
        public Season id { get; }
        public int year { get; }
        public Dictionary<int, Course> courses { get; } = new Dictionary<int, Course>();
        public Dictionary<int, Proffesor> proffs { get; } = new Dictionary<int, Proffesor>();
        public Dictionary<int, Student> student { get; } = new Dictionary<int, Student>();

        public Semester(Season id, int year)
        {
            this.id = id;
            this.year = year;
        }
    }


    internal class Program
    {

        static public string take_phone_number(bool stu, string prompt)
        {
            Console.WriteLine(prompt);
            string pattern = @"^09\d{9}$";
            string input = Console.ReadLine();
            if (stu && input == "-")
            {
                return "-";
            }
            bool f = Regex.IsMatch(input, pattern);
            if (f)
            {
                return input;
            }           

            return null;
        }

        static public int take_id(string thing)
        {
            Console.WriteLine($"Enter {thing}: ");
            int id = -1;
            try
            {
                id = int.Parse(Console.ReadLine()); 
            }
            catch (Exception e)
            {

                Console.WriteLine("Please enter a number"); 
                return -1;
            }

            if (id <= 0)
            {
                Console.WriteLine("Please enter a positive integer");
            }

            return id;
        }

        static void Main(string[] args)
        {
            main_menu();
        }

        static void main_menu()
        {
            University university = new University();            
            while (true)
            {
                Console.WriteLine("Enter as (enter Exit if you want to end the program): 1.Admin  2.Professor  3.Student");
                string cmd = Console.ReadLine();

                if (cmd == "Exit" || cmd == "0")
                {
                    return;
                }

                if (cmd == "Admin" || cmd == "1")
                {
                    Console.WriteLine("Enter admin's code:");
                    string code = Console.ReadLine();
                    if (code == "ADMIN")
                    {
                        admin_menu(university);
                    }
                    else
                    {
                        Console.WriteLine("Wrong password.");
                    }
                }

                if (cmd == "Professor" || cmd == "2")
                {
                    int prof_id = take_id("professor ID");
                    if (university.students.ContainsKey(prof_id))
                    {                        
                        prof_menu(university.proffs[prof_id]);
                    }
                    else
                    {
                        Console.WriteLine("There is no professor with the specifed ID.");
                    }
                }

                if (cmd == "Student" || cmd == "3")
                {
                    int stu_id = take_id("student ID");
                    if (university.students.ContainsKey(stu_id))
                    {
                        student_menu(university.students[stu_id]);
                    }
                    else
                    {
                        Console.WriteLine("There is no student with this ID.");
                    }
                }

            }
        }

        static void admin_menu(University university)
        {
            while (true)
            {
                Console.WriteLine("0. exit, 1. add field, 2. add student, 3. add professor, 4. add course, 5. change student's major, 6. add new semester");
                string cmd = Console.ReadLine();

                if (cmd == "0")
                {
                    return;
                }

                else if (cmd == "1")
                {
                    add_field(university);
                }
                else if (cmd == "2")
                {
                    add_student(university);
                }
                else if (cmd == "3")
                {
                    add_professor(university);
                }
                else if (cmd == "4")
                {
                    add_course(university);
                }
                else if (cmd == "5")
                {
                    change_major(university);
                }
                else if(cmd == "6")
                {
                    add_semester(university);
                }
                else
                {
                    Console.WriteLine("Please enter one of the specifed commands.");
                }
            }

        }
        
        static void add_semester(University university)
        {
            int year = take_id("semester's year");
            if (year <= 0)
            {
                return;
            }
            Console.WriteLine("Enter the semester's season");
            string seas = Console.ReadLine();
            Season season;            
            Enum.TryParse(seas, out season);

            if (season != Season.Spring && season != Season.Summer && season != Season.Fall)
            {
                Console.WriteLine("The semester's season was not valid.");
                return;
            }

            Semester s = university.find_semester(year, season);
            if (s != null)
            {
                Console.WriteLine("This semseter already exists.");
                return;
            }

            university.semesters.Add(new Semester(season, year));
        }
        static void already_exits(string thing)
        {
            Console.WriteLine($"A {thing} with the same id already exists.");
        }

        static Field find_field(University university)
        {
            int id = take_id("field id");
            if (id <= 0)
            {
                return null;
            }
            try
            {
                return university.fields[id];
            }
            catch (Exception)
            {

                Console.WriteLine("There is no field with the specified id.");                
            }

            return null;
        }


        static void add_field(University university)
        {
            int id = take_id("id");
            if (id <= 0)
            {
                return;
            }
            if (university.fields.ContainsKey(id))
            {
                already_exits("field");
                return;
            }
            Console.WriteLine("Enter name of the field: ");
            string name = Console.ReadLine();
            int units = take_id("units' count");
            if (units <= 0)
            {
                return;
            }
            university.fields.Add(id, new Field(id, name, units));
        }

        static void add_student(University university)
        {
            int id = take_id("id");
            if (id <= 0)
            {
                return;
            }
            if (university.students.ContainsKey(id))
            {
                already_exits("student");
                return;
            }

            Console.WriteLine("Enter student's name: ");
            string name = Console.ReadLine();

            Console.WriteLine("Enter student's last name: ");
            string l_name = Console.ReadLine();

            int age = take_id("student's age");
            if (age <= 0)
            {
                return;
            }

            Console.WriteLine("Enter student's gender: ");
            string g = Console.ReadLine();
            Gender gender;
            Enum.TryParse(g, out gender);

            if ((int)gender != 1 && (int) gender != 2)
            {
                Console.WriteLine((int)gender);
                Console.WriteLine("Invalid input.");
                return;
            }

            string phone = take_phone_number(true, "Enter student's phone number (enert '-' if there is none):");
            if (phone == null)
            {
                Console.WriteLine("Invalid phone number.");
                return;
            }
                        
            Field field = find_field(university);

            if (field == null)
            {                
                return;
            }

            Console.WriteLine("Should student pay tuition?: ");
            string t = Console.ReadLine();

            Tuition tuition;
            if (t.ToLower() == "yes")
            {
                tuition = Tuition.tuition_based;
            }
            else if(t.ToLower() == "no")
            {
                tuition = Tuition.tuition_free;
            }
            else
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            Student student = new Student(name, l_name, age, gender, phone, field, id, university, tuition);
            university.students.Add(id, student);
            field.students.Add(id, student);
        }

        static void add_professor(University university)
        {
            int id = take_id("id");
            if (id <= 0)
            {
                return;
            }
            if (university.proffs.ContainsKey(id))
            {
                already_exits("professor");
                return;
            }

            Console.WriteLine("Enter professor's name: ");
            string name = Console.ReadLine();

            Console.WriteLine("Enter professor's last name: ");
            string l_name = Console.ReadLine();

            int age = take_id("professor's age");
            if (age <= 0)
            {
                return;
            }

            Console.WriteLine("Enter professor's gender: ");
            string g = Console.ReadLine();
            Gender gender;
            Enum.TryParse(g, out gender);

            if ((int)gender != 1 && (int)gender != 2)
            {
                Console.WriteLine((int)gender);
                Console.WriteLine("Invalid input.");
                return;
            }

            string phone = take_phone_number(true, "Enter professor's phone number:");
            if (phone == null)
            {
                Console.WriteLine("Invalid phone number.");
                return;
            }            

            university.proffs.Add(id, new Proffesor(name, l_name, age, gender, phone, id));
            
        }

        static void change_major(University university)
        {
            int stu_id = take_id("student's id");
            if (!university.students.ContainsKey(stu_id))
            {
                Console.WriteLine("There is no student with the specified id.");
                return;
            }
            Student student = university.students[stu_id];

            int major_id = take_id("new major's ID");
            if (!university.fields.ContainsKey(major_id))
            {
                Console.WriteLine("There is no major with the specified id.");
                return;
            }
            
            if (student.field.content.id == major_id)
            {
                Console.WriteLine("This is the current major of this student.");
                return;
            }

            foreach (var course in student.courses.Values)
            {
                course.students.Remove(stu_id);
            }
            student.courses.Clear();

            student.field.content.students.Remove(stu_id);

            student.EditProfile(major_id);

        }

        static void add_course(University university)
        {
            Field field = find_field(university);
            if (field == null) 
            {
                return;
            }
            int course_id = take_id("course id");
            if (course_id <= 0)
            {
                return;
            }

            if (field.courses.ContainsKey(course_id))
            {
                Console.WriteLine("There is already a course with the specified name in this field.");
                return;
            }

            Console.WriteLine("Enter course's name: ");
            string name = Console.ReadLine();

            int units = take_id("number of units");
            if (units <= 0)
            {
                return;
            }

            Console.WriteLine("Enter days that the class will be held (e.g, sunday monday):");
            Days days = taking_days(Console.ReadLine());

            if (days == Days.None)
            {
                Console.WriteLine("Days have been entered in an invalid format.");
                return;
            }

            int h = take_id("starting hour of the course");
            if (h < 7 || h > 18)
            {
                Console.WriteLine("University closes in the hours other than those which are specified.");
                return;
            }
            double m = take_id("starting minutes of the course:");
            if (m > 59)
            {
                Console.WriteLine("Invalid time.");
                return;
            }
            ClassTime time = new ClassTime(days, h + m / 60);

            Console.WriteLine("Enter type of units:");
            string uni = Console.ReadLine();

            Credit uni_type;
            Enum.TryParse(uni, out uni_type);

            if (uni_type != Credit.Lab && uni_type != Credit.Internship && uni_type != Credit.Final_Project && uni_type != Credit.General)
            {
                Console.WriteLine("Credit type is not available in the list.");
                return;
            }

            int prof_id = take_id("professor's ID");
            if (prof_id <= 0)
            {
                return;
            }
            if (!university.proffs.ContainsKey(prof_id))
            {
                Console.WriteLine("There is no professor with this ID.");
                return;
            }

            Dictionary<int, Course> p_courses = university.proffs[prof_id].courses;
            foreach (var item in p_courses.Values)
            {
                if (item.time.does_it_conflict(time))
                {
                    Console.WriteLine("This course has conflict with other courses of this professor.");
                    return;
                }
            }

            int sem_year = take_id("semester's year");
            if (sem_year <= 0)
            {
                return;
            }

            Console.WriteLine("Enter the semester's season");
            string seas = Console.ReadLine();
            Season season;
            Enum.TryParse(seas, out season);

            if (season != Season.Spring && season != Season.Summer && season != Season.Fall)
            {
                Console.WriteLine("The semester's season was not valid.");
                return;
            }

            Semester semester = university.find_semester(sem_year, season);
            if (semester == null)
            {
                Console.WriteLine("The specifed semester is not available");
                return;
            }

            Course course = new Course(course_id, name, units, time, uni_type, university.proffs[prof_id], semester);
            field.courses.Add(course_id, course);
            semester.courses.Add(course_id, course);
            university.proffs[prof_id].courses.Add(course_id, course);
            
        }        

        static Days taking_days(string str)
        {
            string[] s = str.Split();            

            Days days = Days.None;
            Days day;
            foreach (string s2 in s)
            {
                Enum.TryParse(s2.ToLower() , out day);
                if (!((int) day > 0 && (int) day <= 64))
                {
                    return Days.None;
                }
                days |= day;
            }

            return days;
        }

        static void prof_menu(Proffesor proffesor)
        {
            while (true)
            {

                Console.WriteLine("0. exit, 1. assign score to course, 2. sort students, 3. average score, 4. edit phone number");
                string cmd = Console.ReadLine();

                if (cmd == "0")
                {
                    return;
                }
                else if (cmd == "1")
                {
                    assign_score(proffesor);
                }
                else if (cmd == "2")
                {
                    sort_stu_grades(proffesor);
                }
                else if (cmd == "3")
                {
                    average(proffesor);
                }
                else if (cmd == "4")
                {
                    change_number(proffesor);
                }
                else
                {
                    Console.WriteLine("Invalid command.");
                }
            }
        }

        static void student_menu(Student student)
        {
            while (true)
            {

                Console.WriteLine("0. exit, 1. add course, 2. remove course, 3. calculate tuition, 4. show grade, 5. edit phone number");
                string cmd = Console.ReadLine();

                if (cmd == "0")
                {
                    return;
                }
                else if (cmd == "1")
                {
                    add_course(student);
                }
                else if (cmd == "2")
                {
                    delete_course(student);
                }
                else if (cmd == "3")
                {
                    calc_tuition(student);
                }
                else if (cmd == "4")
                {
                    show_grade(student);
                }
                else if (cmd == "5")
                {
                    change_number(student);
                }
            
            }
        }

        static void add_course(Student student)
        {
            int course_id = take_id("course ID");
            if (!student.field.content.courses.ContainsKey(course_id))
            {
                Console.WriteLine("There is no course with the specified id for this student.");
                return;
            }

            if (student.courses.ContainsKey(course_id))
            {
                Console.WriteLine("This student has already enrolled in this course.");
                return;
            }

            Course course = student.field.content.courses[course_id];

            if (course.capacity == course.students.Count)
            {
                Console.WriteLine("This course has reached to it's maximum capacity.");
                return;
            }

            foreach (var c in student.courses.Values)
            {
                if (c.time.does_it_conflict(course.time))
                {
                    Console.WriteLine($"This course has conflict with {c.name}");
                    return;
                }
            }

            student.courses.Add(course_id, course);
            course.students.Add(student.id, student);

        }

        static void delete_course(Student student)
        {
            int course_id = take_id("course ID");
            if (!student.field.content.courses.ContainsKey(course_id))
            {
                Console.WriteLine("There is no course with the specified id for this student.");
                return;
            }

            if (!student.courses.ContainsKey(course_id))
            {
                Console.WriteLine("This student is not enrolled in this course.");
                return;
            }

            Course course = student.field.content.courses[course_id];

            student.courses.Remove(course_id);
            course.students.Remove(student.id);

        }

        public static void calc_tuition(Student student)
        {
            if (student.tuition == Tuition.tuition_free)
            {
                Console.WriteLine("Your tuition is waived.");
                return;
            }
            Console.WriteLine("1. course price, 2. total cost of the semester");
            string cmd = Console.ReadLine();

            if (cmd == "1")
            {
                int id = take_id("course ID");
                if (!student.courses.ContainsKey(id))
                {
                    Console.WriteLine("There is no course with this ID in your current class.");
                    return;
                }

                Course course = student.courses[id];
                double fee = (int)course.credit * course.units;
                Console.WriteLine($"Price: {fee}");
            }
            else if (cmd == "2") 
            {
                double fee = 0;
                foreach (var course in student.courses.Values)
                {
                    fee += (int)course.credit * course.units;
                }
                Console.WriteLine($"Total cost of the semeseter: {fee}");
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }

        }

        public static void show_grade(Student student)
        {
            int course_id = take_id("course ID");
            if (!student.courses.ContainsKey(course_id))
            {
                Console.WriteLine("There is no course with the specified id in the student's courses.");
                return;
            }
            Course course = student.courses[course_id];

            if (!course.grades.ContainsKey(student))
            {
                Console.WriteLine("Student's graded has not been added.");
                return;
            }

            Console.WriteLine($"Student's grade in this course: {course.grades[student]}");
        }

        public static void change_number(Student student)
        {
            string n = take_phone_number(false, "Enter student's phone number:");
            if (n == null)
            {
                return;
            }

            student.EditProfile(n);
        }

        public static void assign_score(Proffesor proffesor)
        {
            int course_id = take_id("course ID");
            if (!proffesor.courses.ContainsKey(course_id))
            {
                Console.WriteLine("There is no course in the list with the given ID");
                return;
            }
            Course course = proffesor.courses[course_id];

            int stu_id = take_id("student's ID");
            if (!course.students.ContainsKey(stu_id))
            {
                Console.WriteLine("There is no student in the list with the given ID");
                return;
            }
            Student student = course.students[stu_id];
            
            Console.WriteLine($"Enter student's grade: ");
            double grade;
            try
            {
                grade = double.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {

                Console.WriteLine("Please enter a number");
                return;
            }

            if (grade <= 0 || grade > 20)
            {
                Console.WriteLine("Grades should be between 0 and 20");
                return;
            }

            if (course.grades.ContainsKey(student))
            {
                Console.WriteLine($"Student's grade changed from {course.grades[student]} to {course.grades[student]}");
                return;
            }
            
            course.grades.Add(student, grade);


        }

        public static void sort_stu_grades(Proffesor proffesor)
        {
            int course_id = take_id("course ID");

            if (!proffesor.courses.ContainsKey(course_id))
            {
                Console.WriteLine("There is no course with this ID in the professor's list.");
                return;
            }
            Course course = proffesor.courses[course_id];

            if (course.grades.Count != course.capacity)
            {
                Console.WriteLine("List of grades is incomplete.");
                return;
            }

            List<KeyValuePair<Student, double>> myList = course.grades.ToList();

            myList.Sort(
                delegate (KeyValuePair<Student, double> pair1,
                KeyValuePair<Student, double> pair2)
                {
                    return pair1.Value.CompareTo(pair2.Value);
                }
            );

            foreach (var item in myList)
            {
                Console.WriteLine($"{item.Key.id}: {item.Value}");
            }

        }

        public static void average(Proffesor proffesor)
        {
            int course_id = take_id("course ID");

            if (!proffesor.courses.ContainsKey(course_id))
            {
                Console.WriteLine("There is no course with this ID in the professor's list.");
                return;
            }
            Course course = proffesor.courses[course_id];

            if (course.grades.Count != course.capacity)
            {
                Console.WriteLine("List of grades is incomplete.");
                return;
            }

            double ave = 0;
            foreach (var item in course.grades.Values)
            {
                ave += item;
            }

            ave /= course.capacity;

            Console.WriteLine($"The average is {ave}");
        }

        public static void change_number(Proffesor student)
        {
            string n = take_phone_number(false, "Enter professor's new phone number:");
            if (n == null)
            {
                return;
            }

            student.EditPhoneNumber(n);
        }

    }
}