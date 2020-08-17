using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Snake____final_version
{
    [Serializable]
    class snake
    {

        public double snksScore = 0;
        public bool IsAI = false;
        static Random rnd = new Random();

        //***********************
        public neuralnetwork nn;
        double aplDisX;
        double aplDisY;
        public bool Right = false;
        public bool Left = false;
        public bool Straight = false;
        int num = 0;
        public int bonus = 0;
        
        public int numOfneuronsRows = 4;
        public int inputLength = 11;
        public int outputLength = 3;
        public int length = 8;

        double[] XsnakesHeads;
        double[] YsnakesHeads;
        //***********************

        public char Save = 'd';
        int up;
        int down;
        int right;
        int left;

        public int diagonalUpRight;
        public int diagonalUpLeft;
        public int diagonalDownRight;
        public int diagonalDownLeft;

        public int ttl = 300;
        public int ttlConst = 600;

        int dir;
        int wird = 0;

        public bool loop = false;

        public int runTime = 0;
        public bool alive = true;
        public int l = 1;
        public int[] Xsnake;
        public int[] Ysnake;
        public int snkID;
        int s;

        public snake(int i, int s1)
        {
            l = 1;
            Xsnake = new int[999];
            Ysnake = new int[999];
            Xsnake[0] = i + 1;
            Ysnake[0] = i + 1;
            Xsnake[1] = 0;
            Ysnake[1] = 0;
            snkID = i;
            s = s1;
        }

        public void SaveSnk()
        {
            nn.Save("c:\\snake_final\\_neuralnetwork.dat");
        }

        public void Load(int save)
        {
            nn.Load(save, "c:\\snake_final\\_neuralnetwork.dat");
        }
        public void Load(neuralnetwork nn2)
        {
            nn.Load(nn2);
        }

        public int identify()
        {
            return (snkID);
        }



        public int headX()
        {
            return (Xsnake[0]);
        }



        public int headY()
        {
            return (Ysnake[0]);
        }



        public bool check(int x, int y, int startfrom)
        {
            for (int i = startfrom; i < l; i++)
            {
                if (Xsnake[i] == x)
                {
                    if (Ysnake[i] == y)
                    {
                        return true;
                    }
                }
            }

            return false;
        }



        public bool isAlive(snake[] snakes)
        {
            if (ttl < 1 && IsAI)
            {
                alive = false;
                snksScore = score();
                return false;
            }

            if (!alive)
            {
                snksScore = score();

                return false;
            }

            for (int i = 0; i < snakes.Length; i++)
            {
                if (i != snkID)
                {
                    if (snakes[i].check(Xsnake[0], Ysnake[0], 0))
                    {
                        alive = false;

                        snakes[i].bonus += l;

                        snksScore = score();

                        return false;
                    }
                }
                else
                {
                    if (snakes[i].check(Xsnake[0], Ysnake[0], 1))
                    {
                        alive = false;
                        snksScore = score();

                        return false;
                    }
                }
            }

            if ((Xsnake[0] == s - 2) || (Xsnake[0] == -1))
            {
                snksScore = score();
                alive = false;

                return false;
            }

            if ((Ysnake[0] == s - 2) || (Ysnake[0] == -1))
            {
                alive = false;
                snksScore = score();

                return false;
            }

            return true;
        }



        public void advance(char dir, apple apl, snake[] snakes, bool printapl)
        {
            if (l > 2)
            { if (Save == 'w' && dir == 's')
                {
                    dir = Save;
                }
                if (Save == 's' && dir == 'w')
                {
                    dir = Save;
                }
                if (Save == 'a' && dir == 'd')
                {
                    dir = Save;
                }
                if (Save == 'd' && dir == 'a')
                {
                    dir = Save;
                }
            }

            ttl--;
            num++;
            if (!alive)
            {
                if (l > 0)
                {
                    l--;
                }
                else
                {
                    delete();
                    Xsnake[0] = -1;
                    Ysnake[0] = -1;
                    delete();
                }
            }
            else
            {
                runTime++;
                for (int i = l; i > 0; i--)
                {
                    Xsnake[i] = Xsnake[i - 1];
                    Ysnake[i] = Ysnake[i - 1];
                }

                if (dir == 's')
                {
                    Ysnake[0]++;
                }
                if (dir == 'w')
                {
                    Ysnake[0]--;
                }
                if (dir == 'd')
                {
                    Xsnake[0]++;
                }
                if (dir == 'a')
                {
                    Xsnake[0]--;
                }

                if ((apl.appleX() == Xsnake[0]) && (apl.appleY() == Ysnake[0]))
                {
                    l++;
                    num = 0;
                    apl.newCord(snakes, s);
                    if (printapl)
                    {
                        apl.print();
                    }
                    //ttl = ((ttl + ttlConst) * 6) / 10;
                    ttl = ttl + ttlConst;

                }
            }
            Save = dir;
        }



        public void write(bool arrows)
        {
            int temp = snkID % 10;

            switch (temp)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case 2:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;

                case 3:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case 4:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;

                case 5:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

                case 6:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case 7:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;

                case 8:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;

                case 9:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
            }

            Console.SetCursorPosition((Xsnake[0] + 1) * 2, Ysnake[0] + 1);
            Console.Write("#");
            if (l > 1)
            {
                Console.SetCursorPosition((Xsnake[1] + 1) * 2, Ysnake[1] + 1);

                if (arrows)
                {
                    if (Save == 'w')
                        Console.Write('^');
                    if (Save == 's')
                        Console.Write('v');
                    if (Save == 'a')
                        Console.Write('<');
                    if (Save == 'd')
                        Console.Write('>');
                }
                else
                {
                    Console.Write("O");
                }
            }
        }



        public void delete()
        {
            if ((Xsnake[l] + 1) * 2 != s * 2 - 2 && (Xsnake[l] + 1) * 2 != 0 && Ysnake[l] + 1 != s - 1 && Ysnake[l] + 1 != 0)
            {
                Console.SetCursorPosition((Xsnake[l] + 1) * 2, Ysnake[l] + 1);
                Console.Write(" ");
            }
        }



        void DisTO(snake[] snakes)
        {

            up = 0;
            down = 0;
            right = 0;
            left = 0;
            diagonalDownRight = 0;
            diagonalDownLeft = 0;
            diagonalUpRight = 0;
            diagonalUpLeft = 0;

            if (alive)
            {
                while (isAlive(snakes))
                {
                    right++;
                    Xsnake[0]++;
                }
                Xsnake[0] = Xsnake[0] - right;
                alive = true;

                while (isAlive(snakes))
                {
                    left++;
                    Xsnake[0]--;
                }
                Xsnake[0] = Xsnake[0] + left;
                alive = true;

                while (isAlive(snakes))
                {
                    down++;
                    Ysnake[0]++;
                }
                Ysnake[0] = Ysnake[0] - down;
                alive = true;

                while (isAlive(snakes))
                {
                    up++;
                    Ysnake[0]--;
                }
                Ysnake[0] = Ysnake[0] + up;
                alive = true;

                //********************

                while (isAlive(snakes))
                {
                    diagonalDownRight++;
                    Xsnake[0]++;
                    Ysnake[0]++;
                }
                Xsnake[0] = Xsnake[0] - diagonalDownRight;
                Ysnake[0] = Ysnake[0] - diagonalDownRight;
                alive = true;

                while (isAlive(snakes))
                {
                    diagonalDownLeft++;
                    Xsnake[0]--;
                    Ysnake[0]++;
                }
                Xsnake[0] = Xsnake[0] + diagonalDownLeft;
                Ysnake[0] = Ysnake[0] - diagonalDownLeft;
                alive = true;

                while (isAlive(snakes))
                {
                    diagonalUpRight++;
                    Xsnake[0]++;
                    Ysnake[0]--;
                }
                Xsnake[0] = Xsnake[0] - diagonalUpRight;
                Ysnake[0] = Ysnake[0] + diagonalUpRight;
                alive = true;

                while (isAlive(snakes))
                {
                    diagonalUpLeft++;
                    Xsnake[0]--;
                    Ysnake[0]--;
                }
                Xsnake[0] = Xsnake[0] + diagonalUpLeft;
                Ysnake[0] = Ysnake[0] + diagonalUpLeft;
                alive = true;
            }
        }



        static char WhatToReturn(int up, int down, int right, int left)
        {
            if (up >= down && up >= right && up >= left)
            {
                return ('w');
            }

            if (down >= right && down >= left)
            {
                return ('s');
            }

            if (right >= left)
            {
                return ('d');
            }

            return ('a');
        }



        public char snkalg(snake[] snakes, apple apl)
        {
            DisTO(snakes);
            int col = apl.appleX();
            int row = apl.appleY();

            if (Save == 'w' || Save == 's')
            {
                if (Ysnake[0] - row < up && Ysnake[0] - row > 0)
                {
                    return ('w');
                }

                if (row - Ysnake[0] < down && row - Ysnake[0] > 0)
                {
                    return ('s');
                }

                if (col - Xsnake[0] < right && Xsnake[0] - col < 0)
                {
                    return ('d');
                }

                if (Xsnake[0] - col < left && Xsnake[0] - col > 0)
                {
                    return ('a');
                }
            }
            else if (Save == 'a' || Save == 'd')
            {
                if (col - Xsnake[0] < right && Xsnake[0] - col < 0 && Save != 'a')
                {
                    return ('d');
                }

                if (Xsnake[0] - col < left && Xsnake[0] - col > 0 && Save != 'd')
                {
                    return ('a');
                }

                if (Ysnake[0] - row < up && Ysnake[0] - row > 0 && Save != 's')
                {
                    return ('w');
                }

                if (row - Ysnake[0] < down && row - Ysnake[0] > 0 && Save != 'w')
                {
                    return ('s');
                }
            }

            return (WhatToReturn(up, down, right, left));
        }



        //******************************************************************************************************************************************//

        public void Change(double a)
        {
            nn.Change(a);
        }

        public void AIPrep(snake[] snakes)
        {
            IsAI = true;
            XsnakesHeads = new double[snakes.Length];
            YsnakesHeads = new double[snakes.Length];

            nn = new neuralnetwork(numOfneuronsRows, length, inputLength, outputLength);
        }


        public int RunAI(snake[] snakes, apple apl)
        {

            /*if(diagonalDownLeft > 1)
            {
                diagonalDownLeft = 1;
            }
            else
            {
                diagonalDownLeft = 0;
            }
            
            if (diagonalDownRight > 1)
            {
                diagonalDownRight = 1;
            }
            else
            {
                diagonalDownRight = 0;
            }
            
            if (diagonalUpLeft > 1)
            {
                diagonalUpLeft = 1;
            }
            else
            {
                diagonalUpLeft = 0;
            }
            
            if (diagonalUpRight > 1)
            {
                diagonalUpRight = 1;
            }
            else
            {
                diagonalUpRight = 0;
            }*/

            double[] input = new double[inputLength];

            for (int i = 0; i < snakes.Length; i++)
            {
                XsnakesHeads[i] = snakes[i].headX();
            }

            for (int i = snakes.Length; i < snakes.Length * 2; i++)
            {
                YsnakesHeads[i - snakes.Length] = snakes[i - snakes.Length].headY();
            }

            aplDisY = (Ysnake[0] - apl.appleY());
            aplDisX = (Xsnake[0] - apl.appleX());

            int counter = 0;
            double a = 1;
            double b = 1;
            double c = 0; 

            

            if (Save == 'w')
            {
                counter = 0;
                input[8] = aplDisY;
                input[9] = -aplDisX;
            }
            else if (Save == 'a')
            {
                counter = 2;
                input[8] = aplDisX;
                input[9] = aplDisY;
            }
            else if (Save == 's')
            {
                counter = 4;
                input[8] = -aplDisY;
                input[9] = aplDisX;
            }
            else
            {
                counter = 6;
                input[8] = -aplDisY;
                input[9] = -aplDisX;
            }

            input[counter % 8] = down * a;
            counter++;

            input[counter % 8] = diagonalDownLeft * b;
            counter++;

            input[counter % 8] = left * a;
            counter++;

            input[counter % 8] = diagonalUpLeft * b;
            counter++;

            input[counter % 8] = up * a;
            counter++;

            input[counter % 8] = diagonalUpRight * b;
            counter++;

            input[counter % 8] = right * a;
            counter++;

            input[counter % 8] = diagonalDownRight * b;

            counter = 8;

            input[0] = 0;

            input[10] = c * ttl / 10;

            return (nn.Result(input));
        }

        public char AIsnkalg(snake[] snakes, apple apl)
        {
            IsAI = true;
            DisTO(snakes);
            int chosenOne;
            chosenOne = RunAI(snakes, apl);
            if (chosenOne == 0)
            {
                Left = true;
            }
            if (chosenOne == 1)
            {
                Straight = true;
            }
            if (chosenOne == 2)
            {
                Right = true;
            }
            return (RelativToNormal(chosenOne - 1));
        }

        public char RelativToNormal(int a)
        {
            char[] dirc = new char[4];
            dirc[0] = 'w';
            dirc[1] = 'd';
            dirc[2] = 's';
            dirc[3] = 'a';

            int location = 0;

            for (int i = 0; i < 4; i++)
            {
                if (dirc[i] == Save)
                {
                    location = i;
                }
            }

            location += a + 4;

            location = location % 4;

            return dirc[location];
        }

        public int NormalDir(char a)
        {
            char[] dirc = new char[4];
            dirc[0] = 'w';
            dirc[1] = 'd';
            dirc[2] = 's';
            dirc[3] = 'a';

            int location = 0;

            for (int i = 0; i < 4; i++)
            {
                if (dirc[i] == Save)
                {
                    location = i;
                }
            }

            return location;
        }


        public void newCord(snake[] snakes, int s)
        {
            for (int i = 1; i < l; i++)
            {
                Xsnake[i] = 0;
                Ysnake[i] = 0;
            }
            bool x = true;
            while (x)
            {
                x = false;
                Xsnake[0] = rnd.Next(0, s - 2);
                Ysnake[0] = rnd.Next(0, s - 2);
                for (int i = 0; i < snakes.Length; i++)
                {
                    if (snakes[i].check(Xsnake[0], Ysnake[0], 0) && (i != snkID))
                    {
                        x = true;
                    }
                }
            }
        }



        public double score()
        {

            double x = -1;
            int temp = bonus;
            bonus = 0;

            /*if (ttl < 1)
            {
                loop = true;
                return (-1500);
            }*/

            int asd = wird;
            wird = 0;


            //return (l * 30 + runTime);


            //return runTime;

            //return (asd/10 + l);
            return l;

            return ((l - 00) * 20 + runTime);
        }
    }
}