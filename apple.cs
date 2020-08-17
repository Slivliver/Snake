using System;

namespace Snake____final_version
{
    class apple
    {
        static Random rnd = new Random();
        int row;
        int col;
        public apple(snake[] snakes, int s)
        {
            newCord(snakes, s);
        }

        public void newCord(snake[] snakes, int s)
        {
            bool x = true;
            while (x)
            {
                x = false;
                row = rnd.Next(1, s - 2);
                col = rnd.Next(0, s - 2);
                for (int i = 0; i < snakes.Length; i++)
                {
                    if (snakes[i].check(col, row, 0))
                    {
                        x = true;
                    }
                }
            }
        }


        public void print()
        {
            Console.SetCursorPosition((col + 1) * 2, (row + 1));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine('@');
        }
        public int appleX()
        {
            return (col);
        }
        public int appleY()
        {
            return (row);
        }
    }
}
