namespace _2
{

    class Point
    {
        private double x, y;

        public Point(double x=0, double y=0)
        {
            this.x = x;
            this.y = y;
        }

        public double DistanceTo(Point p)
        {
            return Math.Sqrt(Math.Pow(p.x - this.x, 2) + Math.Pow(p.y - this.y, 2));
        }

        public void move(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double get_x()
        {
            return this.x;
        }

        public double get_y()
        {
            return this.y;
        }

    }

    class Circle
    {
        private double radius;
        private Point center;

        public Circle(double radius, Point center)
        {
            if(radius >= 0) 
            this.radius = radius;
            this.center = center;
        }

        public Circle(double radius, double x, double y) 
        {
            this.radius = radius;
            this.center = new Point(x, y);
        }

        public double area()
        {
            return Math.Pow(this.radius, 2)  * Math.PI;
        }

        public double perimeter()
        {
            return 2 * Math.PI * this.radius;
        }

        public int GetRelativePosition(Point p)
        {
            double dist = p.DistanceTo(center);

            if (dist > this.radius)
            {
                return 1;
            }
            else if (dist == this.radius)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }

    class Rectangle
    {
        Point a;
        Point b;
        Point c;
        Point d;

        public Rectangle(Point a, Point b, Point c, Point d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public Rectangle(Point a, Point c)
        {
            this.a = a;
            this.c = c;
            this.b = new Point(c.get_x(), a.get_y());
            this.d = new Point(a.get_x(), c.get_y());

        }

        public double area()
        {
            double x = a.DistanceTo(b);
            double y = b.DistanceTo(c);

            return x * y;
        }

        public double perimeter()
        {
            double x = a.DistanceTo(b);
            double y = b.DistanceTo(c);

            return (x + y) * 2;
        }

        public void move(double x, double y)
        {
            this.a.move(a.get_x() + x, a.get_y() + y);
            this.c.move(c.get_x() + x, c.get_y() + y);
            this.b.move(b.get_x() + x, b.get_y() + y);
            this.d.move(d.get_x() + x, d.get_y() + y);

        }

    }
    internal class Program
    {

        
        static void Main(string[] args)
        {

            Point p = new Point();
            Point p2 = new Point(1,2);
            p.move(2, 4);
            Console.WriteLine(p.DistanceTo(p2));

            Circle circle1 = new Circle(4, p);
            Circle circle2 = new Circle(5, 13, 2);

            Console.WriteLine(circle2.area() + " " + circle2.perimeter());

            Console.WriteLine(circle1.GetRelativePosition(p2));

            Rectangle rec1 = new Rectangle(p2, p);

            Rectangle rec2 = new Rectangle(new Point(0,0), new Point(3, 0), new Point(3, 3), new Point(0, 3));
            Console.WriteLine(rec2.area() + " " + rec2.perimeter());

            rec2.move(1, 1);


        }
    }
}