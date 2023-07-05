namespace _2
{

    abstract class Piece
    {
        protected int x;
        protected int y;
        protected int x_i;
        protected int y_i;
        static public Dictionary<string, Piece> pieces = new Dictionary<string, Piece>();
        static public List<Piece> pieces_list = new List<Piece>();

        public Piece(int x, int y)
        {
            this.x = x;
            this.y = y;
            x_i = x; y_i = y;

            pieces.Add(current_pos(), this);
            pieces_list.Add(this);
        }

        public virtual bool move(int x, int y) 
        {
            if (pieces.ContainsKey(x + " " + y))
            {
                pieces[x + " " + y].move_to_initial_pos();                
            }
            pieces.Remove(current_pos());
            this.x = x;
            this.y = y;
            pieces.Add(current_pos(), this);
            return true;
        }

        public string current_pos()
        {
            return x+" "+y;
        }

        public void move_to_initial_pos()
        {
            pieces.Remove(current_pos());
            x = x_i; y = y_i;
            pieces.Add(current_pos(), this);
        }

        public bool has_reached_the_end()
        {
            return y == 32;
        }
    }

    class Rook : Piece
    {
        public Rook(int x, int y) : base(x, y)
        {

        }

        public override bool move(int x, int y)
        {
            if ((Math.Abs(this.y - y) == 2 && this.x == x) || (Math.Abs(this.y - y) == 1 && this.x == x) ||
                (this.y == y && Math.Abs(this.x - x) == 1))
            {
               return base.move(x, y);                
            }

            return false;
        }
    }

    class Knight : Piece
    {
        public Knight(int x, int y) : base(x, y)
        {

        }

        public override bool move(int x, int y)
        {
            if ((this.y + 2 == y && (this.x == x + 1 || this.x == x - 1)) || (this.y - 2 == y && (this.x == x + 1 || this.x == x - 1)) ||
                ((this.y == y + 1 || this.y == y - 1) && this.x + 2 == x) || ((this.y == y + 1 || this.y == y - 1) && this.x - 2 == x))
            {
                return base.move(x, y);
            }

            return false;
        }
    }

    class Queen : Piece
    {
        public Queen(int x, int y) : base(x, y)
        {

        }

        public override bool move(int x, int y)
        {
            if ((Math.Abs(this.y - y)  == 2 && this.x == x) || (Math.Abs(this.y - y) == 1 && this.x == x) ||
                (this.y == y && Math.Abs(this.x - x) == 1) || Math.Abs(y - this.y) == Math.Abs(x - this.x))
            {
                return base.move(x, y);
            }

            return false;
        }
    }

    class Bishop : Piece
    {
        public Bishop(int x, int y) : base(x, y)
        {

        }

        public override bool move(int x, int y)
        {
            if (Math.Abs(y - this.y) == Math.Abs(x - this.x))
            {
                return base.move(x, y);
            }

            return false;
        }
    }


    internal class Program
    {

        public static void print_board()
        {
            for (int i = 1; i <= 4; i++)
            {
                for (int j = 1; j <= 32; j++)
                {
                    if (Piece.pieces.ContainsKey(i + " " + j))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(Piece.pieces[i + " " + j].GetType().Name[0]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write("0");
                    }
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            add_piece();

            bool flag = false;
            while (!flag)
            {
                foreach (var piece in Piece.pieces_list)
                {
                    while (true)
                    {
                        print_board();
                        Console.WriteLine($"Enter {(piece.GetType()).Name} next move: ");
                        string[] move = Console.ReadLine().Split();
                        int x = int.Parse(move[0]);
                        int y = int.Parse(move[1]);

                        if (x < 1 || x > 4 || y < 1 || y > 32)
                        {
                            Console.WriteLine("Input is out of board.");
                            continue;
                        }

                        if (!piece.move(x,y))
                        {
                            Console.WriteLine("This move is not possible for this piece.");
                            continue;
                        }
                        else
                        {
                            foreach (var item in Piece.pieces.Values)
                            {
                                if (item != piece && item.current_pos() == piece.current_pos())
                                {
                                    item.move_to_initial_pos();
                                }
                            }
                            if (piece.has_reached_the_end())
                            {
                                flag = true;
                            }
                            break;
                        }
                    }

                    if (flag)
                    {
                        break;
                    }
                }
            }
            //Piece piece = new Rook(3, 3);

            //Console.WriteLine((piece.GetType()).Name);

        }
        static public void add_piece()
        {
            int i = 1;
            while(i < 5)
            {
                Console.WriteLine("Choose one the following pieces: 1.Rook, 2.Bishop, 3.Knight, 4.Queen");
                string choice = Console.ReadLine();

                bool f = false;
                if (choice == "1")
                {
                    foreach (var item in Piece.pieces.Values)
                    {
                        if (item is Rook)
                        {
                            Console.WriteLine("This piece is already on the board.");
                            f = true;
                        }
                    }
                    if (f)
                    {
                        continue;
                    }
                    new Rook(i, 1);
                    i++;
                }
                else if (choice == "2")
                {
                    foreach (var item in Piece.pieces.Values)
                    {
                        if (item is Bishop)
                        {
                            Console.WriteLine("This piece is already on the board.");
                            continue;
                        }
                    }
                    if (f)
                    {
                        continue;
                    }
                    new Bishop(i, 1);
                    i++;
                }
                else if(choice == "3")
                {
                    foreach (var item in Piece.pieces.Values)
                    {
                        if (item is Knight)
                        {
                            Console.WriteLine("This piece is already on the board.");
                            continue;
                        }
                    }
                    if (f)
                    {
                        continue;
                    }
                    new Knight(i, 1);
                    i++;
                }
                else if (choice == "4")
                {
                    foreach (var item in Piece.pieces.Values)
                    {
                        if (item is Queen)
                        {
                            Console.WriteLine("This piece is already on the board.");
                            continue;
                        }
                    }
                    if (f)
                    {
                        continue;
                    }
                    new Queen(i, 1);
                    i++;
                }
                else { Console.WriteLine("Enter one the specified commands."); continue; }

            }
        }
    }

}