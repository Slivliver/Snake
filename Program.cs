//Made by Daniel Silver

using System;
using System.Threading;
namespace Snake____final_version
{
    class Program
    {
        static Random rnd = new Random();
 
        static int Partition(int low, int high)
        {
            //1. Select a pivot point.
            double pivot = snakes1[high].snksScore;

            int lowIndex = (low - 1);

            //2. Reorder the collection.
            for (int j = low; j < high; j++)
            {
                if (snakes1[j].snksScore <= pivot)
                {
                    lowIndex++;

                    snake temp = snakes1[lowIndex];
                    snakes1[lowIndex] = snakes1[j];
                    snakes1[j] = temp;
                }
            }

            snake temp1 = snakes1[lowIndex + 1];
            snakes1[lowIndex + 1] = snakes1[high];
            snakes1[high] = temp1;

            return lowIndex + 1;
        }

        static void Sort(int low, int high)
        {
            if (low < high)
            {
                int partitionIndex = Partition(low, high);

                //3. Recursively continue sorting the array
                Sort(low, partitionIndex - 1);
                Sort(partitionIndex + 1, high);
            }
        }

        //****************************************************************************************************************


        static int s = 30 + 2; // board size
        static snake[] snakes1; // array of snakes
        
        static bool arrows = false;

        // the evolution function for the machine learning.
        // gets the number of snakes that plays simultaneously (Competing with each other).
        static snake Evolution(int snknum)
        {
            snake[] snakes = new snake[snknum];
            for (int i = 0; i < snakes.Length; i++)
            {
                snakes[i] = new snake(i, s);
            }

            for (int i = 0; i < snakes.Length; i++)
            {
                snakes[i].newCord(snakes, s);
            }

            bool erase = false;
            char dir = ' '; //snake diraction by the neural network results.
            bool life = true;
            bool othersLife = true;
            snakes1 = new snake[60]; // the number of snakes in every generation
            int numOfGeneration = 100000000;
            
            int evr = 1; // the num of iterations each snake do in one generation
            int a = 40; // snakes1.length - a = the num of new random snakes.
            int b = 5; // the num of parents snakes 
            int c = 10; // the num of snakes that survives to the next generation (includes b)
            // a - (b + c) = the number of childrens.

            char pressedKey = ' '; // changes to the key pressed by the user


            for (int i = 0; i < snakes1.Length; i++)
            {
                snakes1[i] = new snake(0, s);
                snakes1[i].AIPrep(snakes);
            }

            Console.WriteLine("if you already have a saved version and want to keep training it, please write the version num. Otherwise write anything else.");
            int ErlySnk = int.Parse(Console.ReadLine());
            Console.Beep(261, 50);

            Console.Clear();
            PrintBoard();
            //loading the save version to all the snakes. Also adding mutations.
            snakes1[0].Load(ErlySnk);
            snakes1[0].snksScore = 10000;
            for (int i = 1; i < snakes1.Length; i++)
            {
                snakes1[i].Load(ErlySnk);
                snakes1[i].Change(10);
            }

            for (int t = 0; t < numOfGeneration; t++)
            {
                Console.SetCursorPosition(0, s);
                Console.WriteLine("Press '*' to stop the learning and save the neural network to file.");
                Console.WriteLine("Press 'w' to watch the current snake.");
                Console.WriteLine((t / (numOfGeneration + 0.0)) * 100 + "% (epoch " + t + " )                      ");


                for (int x = 0; x < snakes1.Length; x++)
                {

                    if ((x / (snakes1.Length + 0.0)) * 100 % 10 == 0)
                    {
                        Console.SetCursorPosition(0, s + 3);
                        Console.WriteLine((x / (snakes1.Length + 0.0)) * 100 + "%                        ");
                    }

                    //initialization of variables before the game begins
                    for (int i = 0; i < snakes.Length; i++)
                    {
                        snakes[i] = new snake(i, s);
                        snakes[i].newCord(snakes, s);
                        snakes[i].l = 1;
                    }
                    life = true;
                    apple apl = new apple(snakes, s);
                    snakes[0] = snakes1[x];
                    snakes[0].newCord(snakes, s);
                    double score = 0;

                    
                    for (int m = 0; m < evr; m++)
                    {
                        apl.newCord(snakes, s);

                        // print the current game state to the screan
                        if ((Console.KeyAvailable || pressedKey == 'w') && x > snakes1.Length - a) 
                        {
                            if (Console.KeyAvailable)
                            {
                                pressedKey = Console.ReadKey().KeyChar;
                            }
                            Console.Clear();
                            PrintBoard();
                            Console.Write(x + " / " + snakes1.Length);
                            apl.print();
                        }

                        while (Console.KeyAvailable)
                        {
                            pressedKey = Console.ReadKey().KeyChar;
                        }
                        
                        snakes[0].l = 1; //initial snake length

                        //one game
                        while (life)
                        {
                            if (erase && (pressedKey != 'w'))
                            {
                                Console.Clear();
                                PrintBoard();
                                erase = false;
                            }
                            for (int i = 0; i < snakes.Length; i++)
                            {
                                if (i == 0)
                                {
                                    //  one move of the tested snake, according to the neural network.
                                    dir = snakes[0].AIsnkalg(snakes, apl);
                                    snakes[i].advance(dir, apl, snakes, (Console.KeyAvailable || pressedKey == 'w') && x > snakes1.Length - a);
                                    life = snakes[0].isAlive(snakes);
                                    snakes[0].Save = dir;
                                }
                                else
                                {
                                    //one move of snakes[i], according to the fixed algorithm
                                    dir = snakes[i].snkalg(snakes, apl);
                                    snakes[i].Save = dir;
                                    snakes[i].advance(dir, apl, snakes, (Console.KeyAvailable || pressedKey == 'w') && x > snakes1.Length - a);
                                    othersLife = snakes[i].isAlive(snakes);
                                    if (!othersLife)
                                    {
                                        snakes[i] = new snake(i, s);
                                        snakes[i].newCord(snakes, s);
                                    }
                                }

                                // print the current game state to the screan
                                if ((Console.KeyAvailable || pressedKey == 'w') && x > snakes1.Length - a)
                                {
                                    erase = true;
                                    if (Console.KeyAvailable)
                                    {
                                        pressedKey = Console.ReadKey().KeyChar;
                                    }
                                    snakes[i].delete();
                                    Console.SetCursorPosition(0, 0);
                                    snakes[i].write(arrows);
                                    Console.SetCursorPosition(2*s+1, 0);
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine("score: " + snakes1[x].score());
                                    if (i == 0)
                                    {
                                        Thread.Sleep(50);
                                    }
                                    apl.print();
                                }
                            }
                        }

                        snakes[0].ttl = snakes[0].ttlConst;
                        life = true;
                        snakes[0].alive = true;
                        snakes[0].runTime = 0;
                        snakes[0].l = 1;
                        snakes[0].bonus = 0;
                        snakes[0].newCord(snakes, s);
                        score += snakes[0].snksScore;
                        snakes[0].snksScore = 0;
                    }

                    score = score / (evr * 1.0);

                    double temp = -0.5;
                    if (snakes1[x].Right)
                    {
                        temp += 0.5;
                    }
                    if (snakes1[x].Left)
                    {
                        temp += 0.5;
                    }
                    if (snakes1[x].Straight)
                    {
                        temp += 0.5;
                    }

                    score *= temp;

                    snakes1[x].Right = false;
                    snakes1[x].Left = false;
                    snakes1[x].Straight = false;
                    snakes1[x] = snakes[0];
                    snakes1[x].snksScore = score;
                }

                Console.WriteLine("last epoch's first place score in this epoch:  " + snakes1[snakes1.Length - 1].snksScore + "       ");
                Sort(0, snakes1.Length - 1); // sort snakes1 by score

                if (pressedKey == '*')
                {
                    return (snakes1[snakes1.Length - 1]); // returning the snake with the best score
                }
                
                Console.SetCursorPosition(0, s + 5);
                Console.ForegroundColor = ConsoleColor.White;
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine((i+1) + ": " + snakes1[snakes1.Length - i - 1].snksScore + "                                       ");
                }
                Console.SetCursorPosition(0, s + 3);



                
                for (int x = 0; x < snakes1.Length; x++)
                {
                    if (x < snakes1.Length - a)// create new random snakes
                    {
                        snakes1[x] = new snake(0, s);
                        snakes1[x].AIPrep(snakes);
                    }

                    else if (x < (snakes1.Length) - c) // create snakes childs
                    {
                        snakes1[x] = new snake(0, s);
                        snakes1[x].AIPrep(snakes);
                        snakes1[x].Load(snakes1[snakes1.Length - x % b - 1].nn); // duplicating a snake into this snake
                        snakes1[x].Change(rnd.NextDouble() * 8 + 4); // add mutations
                    }

                    snakes1[x].l = 1;
                    snakes1[x].alive = true;
                    snakes1[x].runTime = 0;
                    snakes1[0].newCord(snakes, s);
                    snakes1[x].snksScore = 0;
                }
            }
            return (snakes1[snakes1.Length - 1]); // returning the snake with the best score
        }

        static void PrintBoard() //printing the board
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int row = 0; row < s; row++)
            {
                for (int col = 0; col < s; col++)
                {
                    if (col == s - 1)
                    {
                        if (row == s - 1)
                        {
                            Console.Write('╝');
                        }
                        else if (row == 0)
                        {
                            Console.Write('╗');
                        }
                        else
                        {
                            Console.Write('║');
                        }
                    }
                    else if (col == 0)
                    {
                        if (row == s - 1)
                        {
                            Console.Write('╚'); 
                        }
                        else if (row == 0)
                        {
                            Console.Write('╔');
                        }
                        else
                        {
                            Console.Write('║');
                        }
                    }

                    else if (row == 0 || row == s - 1)
                    {
                        Console.Write("══");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
                Console.WriteLine("");
            }
        }


        static void Main(string[] args)
        {
            int snknum;
            char dir = 's';
            char dir2 = ' ';
            char temp = ' ';
            bool alive = true;
            bool othersAlive = true;
            int gameNum = 9999;
            int delay;

            int sum = 0;

            Random rnd = new Random();
            Console.WriteLine("");
            Console.Write("Silver Studios presents: ");
            Thread.Sleep(1000);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write('S');
            Thread.Sleep(200);
            Console.Write('n');
            Thread.Sleep(200);
            Console.Write('a');
            Thread.Sleep(200);
            Console.Write('k');
            Thread.Sleep(200);
            Console.Write('e');
            Thread.Sleep(1000);
        firstMenu:
            Console.Clear();
            Console.WriteLine("");
           
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("1. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Start");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("2. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Options");
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("3. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Credits");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("4. ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Quit game");

            string menu = Console.ReadLine();


            while (!(menu == "1" || menu == "2" || menu == "3" || menu == "4"))
            {
                Console.Beep(293, 500);
                Console.Clear();
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("1. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Start");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("2. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Options");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("3. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Credits");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("4. ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Quit game");

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("please try again, write '1', '2', '3' or '4'.");
                menu = Console.ReadLine();

            }

            Console.Beep(261, 100);
            //Console.Beep(330, 50);

            if (menu == "4")
            {
                System.Environment.Exit(1);
            }

            if (menu == "2")
            {
            secondMenu:
                Console.Clear();
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("1. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Size (=" + (s - 2) + ")");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("2. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Snake body (arrows) (=" + arrows + ")");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("3. ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Back");

                string menu2 = Console.ReadLine();


                while (!(menu2 == "1" || menu2 == "2" || menu2 == "3"))
                {
                    Console.Beep(293, 500);
                    Console.Clear();
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("1. ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Size (=" + (s - 2) + ")");

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("2. ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Snake body (arrows) (=" + arrows + ")");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("3. ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Back");
                    Console.WriteLine("Please try again, write '1', '2' or '3'.");
                    menu2 = Console.ReadLine();
                }
                if (menu2 == "1")
                {
                    Console.Beep(261, 50);
                    //Console.Beep(330, 50);
                    Console.Clear();
                    Console.WriteLine("Please write a new size for the board. (current size is: " + (s-2) + ")");
                    s = int.Parse(Console.ReadLine()) + 2;
                    Console.Beep(261, 50);
                    //Console.Beep(330, 50);

                    goto secondMenu;
                }
                if (menu2 == "2")
                {
                    Console.Beep(261, 50);
                    //Console.Beep(330, 50);
                    Console.Clear();
                    arrows = !arrows;
                    goto secondMenu;
                }
                if(menu2 == "3")
                {
                    Console.Beep(261, 50);
                    Console.Beep(330, 50);
                    goto firstMenu;
                }
            }
            if(menu == "3")
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Gray;

                Console.WriteLine();
                Console.WriteLine("              ___");
                Console.WriteLine("            /" + (char)(92) + "   " + (char)(92));
                Console.WriteLine("           /  " + (char)(92) + "   " + (char)(92));
                Console.WriteLine("          /    " + (char)(92) + "   " + (char)(92));
                Console.WriteLine("         /      " + (char)(92) + "   " + (char)(92));
                Console.WriteLine("        /   /" + (char)(92) + "   " + (char)(92) + "   " + (char)(92));
                Console.WriteLine("       /   /  " + (char)(92) + "   " + (char)(92) + "   " + (char)(92));
                Console.WriteLine("      /   /  / " + (char)(92) + "   " + (char)(92) + "   " + (char)(92));
                Console.WriteLine("     /   /  /___" + (char)(92) + "___" + (char)(92) + "   " + (char)(92));
                Console.WriteLine("    /   /  /             " + (char)(92));
                Console.WriteLine("   /   /  /_______________" + (char)(92));
                Console.WriteLine("   " + (char)(92) + "  /                   /");
                Console.WriteLine("    " + (char)(92) + "/___________________/");
                Console.WriteLine("");
                
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("    Made by: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("DANIEL SILVER");

                /*int time = 3;

                Console.Beep(523, time * 240);
                Console.Beep(523, time * 240);
                Console.Beep(523, time * 240);
                Console.Beep(392, time * 80);
                Console.Beep(440, time * 160);
                Console.Beep(392, time * 80);
                Console.Beep(622, time * 160);
                Console.Beep(622, time * 160);
                Console.Beep(587, time * 240);
                Console.Beep(392, time * 80);
                Console.Beep(440, time * 160);
                Console.Beep(392, time * 80);
                
                Console.Beep(523, time * 240);
                Console.Beep(523, time * 240);
                Console.Beep(523, time * 240);
                Console.Beep(392, time * 80);
                Console.Beep(440, time * 160);
                Console.Beep(392, time * 80);
                Console.Beep(311, time * 160);
                Console.Beep(392, time * 80);
                Console.Beep(311, time * 160);
                Console.Beep(294, time * 240);
                Console.Beep(196, time * 80);
                Console.Beep(220, time * 160);
                Console.Beep(196, time * 80);

                Console.Beep(262, time * 240);
                Console.Beep(262, time * 240);
                Console.Beep(262, time * 240);
                Console.Beep(196, time * 80);
                Console.Beep(220, time * 160);
                Console.Beep(196, time * 80);
                Console.Beep(622, time * 160);
                Console.Beep(523, time * 80);
                Console.Beep(622, time * 160);
                Console.Beep(587, time * 240);
                Console.Beep(523, time * 80);
                Console.Beep(440, time * 160);
                Console.Beep(392, time * 80);
                Console.Beep(523, time * 160);

                Console.Beep(311, time * 80);
                Console.Beep(330, time * 160);
                Console.Beep(440, time * 80);
                Console.Beep(392, time * 160);
                Console.Beep(622, time * 80);
                Console.Beep(659, time * 160);
                Console.Beep(880, time * 80);
                Console.Beep(784, time * 160);
                Console.Beep(880, time * 80);
                Console.Beep(988, time * 160);
                Console.Beep(1047, time * 240);
                Console.Beep(523, time * 240);*/





                Thread.Sleep(5000);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("");
                Console.WriteLine("  (Press enter to return to menu)");
                Console.ReadLine();
                Console.Beep(261, 50);
                Console.Beep(330, 50);
                goto firstMenu;

            }
            //Console.Beep(261, 500);
            //Console.Beep(330, 500);
            /*Console.Beep(293, 500);
            Console.Beep(261, 500);
            Console.Beep(293, 500);
            Console.Beep(330, 500);
            Console.Beep(330, 500);

            Console.Beep(261, 50);
            Console.Beep(261, 50);
            Console.Beep(261, 50);
            Console.Beep(261, 50);*/

        thirdMenu:
            Console.Clear();
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("1. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Machine learning");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("2. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Fixed algorithm");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("3. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("I want to play!");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("4. ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Back");
            
            string gameMod1= Console.ReadLine();


            while (!(gameMod1 == "1" || gameMod1 == "2" || gameMod1 == "3" || gameMod1 == "4" || gameMod1 == "123" || gameMod1 == "111"))
            {
                Console.Beep(293, 500);
                Console.Clear();
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("1. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Machine learning");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("2. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Fixed algorithm");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("3. ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("I want to play!");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("4. ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Back");
                Console.WriteLine("Please try again, write '1', '2', '3' or '4'.");

                gameMod1 = Console.ReadLine();
            }

            Console.ForegroundColor = ConsoleColor.White;

            Console.Beep(261, 50);
            //Console.Beep(330, 50);

            if (gameMod1 == "4")
            {
                Console.Beep(330, 50);
                goto firstMenu;
            }
            int gameMod = int.Parse(gameMod1);
        fourthMenu:
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("How many mili-seconds do you want between each step? (100 recomended) ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("b. ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Back");
            string str = Console.ReadLine();
            Console.Beep(261, 50);

            if (str == "b")
            {
                Console.Beep(330, 50);
                goto thirdMenu;
            }
            //Console.Beep(330, 50);

            delay = int.Parse(str);

            if (gameMod == 123)
            {
                gameMod = 1;
                snknum = 10;
            }
            else if (gameMod == 111)
            {
                gameMod = 2;
                snknum = 1;
            }
            else
            {
                gameMod -= 1;
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("How many snakes do you want?(recommended no more then 10) ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("b. ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Back");
                str = Console.ReadLine();
                Console.Beep(261, 50);
                //Console.Beep(330, 50);

                if (str == "b")
                {
                    Console.Beep(330, 50);
                    goto fourthMenu;
                }
                snknum = int.Parse(str);
                gameNum = 1000000000;
            }
            snake[] snakes = new snake[snknum];

            for (int I = 0; I < gameNum; I++)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                PrintBoard();

                Console.WriteLine("games left: " + (gameNum - I));

                for (int i = 0; i < snakes.Length; i++)
                {
                    if (gameMod == 0 && i == 0)
                    {
                        if (I == 0)
                        {
                            snakes[0] = Evolution(snknum);
                            if (snakes[0].IsAI)
                            {
                                Console.Clear();
                                Console.WriteLine();
                                Console.WriteLine("(Befor saving, make sure you have a folder named 'snake_final' under 'c:').");
                                Console.Write("Do you want to save your snake to a file? (");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("IMPORTANT:");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine(" you WON'T have another chance to save your snake!)");
                                Console.Write("If you do, ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("press enter");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(", if you dont, please ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("write 'no'");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine(".");


                                str = Console.ReadLine();
                                if (str == "no" || str == "No" || str == "NO")
                                {
                                    Console.WriteLine("...");
                                    Console.Write("your snake was ");
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write("not saved");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine(".");

                                }
                                else
                                {
                                    snakes[0].SaveSnk();
                                    Console.WriteLine("...");
                                    Console.Write("your snake was ");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write("saved");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine(".");
                                }
                            }
                            Console.ReadLine();
                            Console.Beep(261, 50);
                            Console.Clear();
                            PrintBoard();

                        }
                        snakes[0].l = 1;
                        snakes[0].runTime = 0;
                        snakes[0].alive = true;
                    }
                    else
                    {
                        snakes[i] = new snake(i, s);
                    }
                }
                for (int i = 0; i < snakes.Length; i++)
                {
                    snakes[i].newCord(snakes, s);
                    snakes[i].write(arrows);
                }

                apple apl = new apple(snakes, s);

                apl.print();
                for (int h = 0; h < snakes.Length; h++)
                {
                    snakes[h].write(arrows);
                    Console.SetCursorPosition(2 * s, 0 + 3 * h);
                    Console.Write('╔');
                    for (int z = 0; z < 6; z++)
                    {
                        Console.Write('═');
                    }
                    Console.Write('╗');
                    Console.SetCursorPosition(2 * s, 1 + 3 * h);
                    Console.Write("║ " + snakes[h].l);
                    Console.SetCursorPosition(2 * s + 7, 1 + 3 * h);
                    Console.Write("║");
                    Console.SetCursorPosition(2 * s, 2 + 3 * h);
                    Console.Write('╚');
                    for (int z = 0; z < 6; z++)
                    {
                        Console.Write('═');
                    }
                    Console.Write('╝');
                }
                while (alive && othersAlive)
                {
                    for (int h = 0; h < snakes.Length; h++)
                    {
                        snakes[h].write(arrows);

                        Console.SetCursorPosition(2 * s + 2, 1 + 3 * h);
                        Console.Write(snakes[h].l);

                        Console.SetCursorPosition(2 * s + 7 + 2, 3 * h);
                        if (snakes[h].l > 1)
                        {
                            if (snakes[h].l > (188 - 2 * s))
                            {
                                Console.SetCursorPosition(2 * s + 9 + snakes[h].l - ((snakes[h].l / (188 - 2 * s))) * (188 - 2 * s) - 1, 3 * h + snakes[h].l / (188 - 2 * s));
                                Console.Write('0');
                            }
                            else
                            {
                                Console.SetCursorPosition(2 * s + 9 + snakes[h].l - 2, 3 * h);
                                Console.Write('0');
                            }

                        }
                        Console.Write('#');
                    }


                    if (gameMod == 2)
                    {
                        while (Console.KeyAvailable)
                        {
                            Console.SetCursorPosition(0, 0);
                            temp = dir;
                            dir = Console.ReadKey().KeyChar;
                            if (dir == 'b') //going back to main menu
                            {
                                goto firstMenu;
                            }
                            if (!((dir == 'a') || (dir == 's') || (dir == 'd') || (dir == 'w')))
                            {
                                dir = temp;
                            }
                        }
                        snakes[0].advance(dir, apl, snakes, true);
                        snakes[0].delete();
                        snakes[0].write(arrows);

                        alive = snakes[0].isAlive(snakes);

                        if (!alive)
                        {
                            othersAlive = true;
                            break;
                        }

                        if (snakes.Length > 1)
                        {
                            othersAlive = false;
                            for (int i = 1; i < snakes.Length; i++)
                            {
                                dir2 = snakes[i].snkalg(snakes, apl);
                                snakes[i].advance(dir2, apl, snakes, true);
                                snakes[i].delete();
                                if (snakes[i].isAlive(snakes))
                                {
                                    snakes[i].write(arrows);
                                }
                                if (!othersAlive)
                                {
                                    othersAlive = snakes[i].isAlive(snakes);
                                }
                            }
                        }

                        Console.SetCursorPosition(0, 0);

                        Thread.Sleep(delay);
                    }

                    else if (gameMod != 0)
                    {
                        if (Console.KeyAvailable)
                        {
                            char key = Console.ReadKey().KeyChar;
                            {
                                if (key == 'k') //kills the current snake
                                {
                                    alive = false;
                                    othersAlive = false;
                                }
                                if (key == 'b') //going back to main menu
                                {
                                    goto firstMenu;
                                }
                            }
                        }
                        othersAlive = false;
                        for (int i = 0; i < snakes.Length; i++)
                        {
                            Console.SetCursorPosition(0, 0);
                            dir = snakes[i].snkalg(snakes, apl);
                            snakes[i].advance(dir, apl, snakes, true);
                            snakes[i].delete();
                            Console.SetCursorPosition(0, 0);
                            if (snakes[i].isAlive(snakes))
                            {
                                snakes[i].write(arrows);
                            }
                            Console.SetCursorPosition(0, 0);

                            if (!othersAlive)
                            {
                                othersAlive = snakes[i].isAlive(snakes);
                            }
                        }
                        Thread.Sleep(delay);
                    }

                    if (gameMod == 0)
                    {

                        for (int i = 0; i < snakes.Length; i++)
                        {
                            Console.SetCursorPosition(0, 0);
                            if (Console.KeyAvailable)
                            {
                                char key = Console.ReadKey().KeyChar;
                                {
                                    if (key == 'k') //kills the current snake
                                    {
                                        alive = false;
                                        othersAlive = false;
                                    }
                                    if (key == 'b') //going back to main menu
                                    {
                                        goto firstMenu;
                                    }
                                }
                            }
                            while (Console.KeyAvailable)
                            {
                                char key = Console.ReadKey().KeyChar;
                            }
                            if (snakes[i].IsAI)
                            {
                                dir = snakes[i].AIsnkalg(snakes, apl);
                                snakes[i].Save = dir;
                                alive = snakes[i].isAlive(snakes);

                            }
                            else
                            {
                                dir = snakes[i].snkalg(snakes, apl);
                                snakes[i].Save = dir;
                                if (!snakes[i].isAlive(snakes))
                                {
                                    snakes[i] = new snake(i, s);
                                    snakes[i].newCord(snakes, s);
                                }
                            }

                            snakes[i].advance(dir, apl, snakes, true);
                            snakes[i].delete();
                            Console.SetCursorPosition(0, 0);
                            snakes[i].write(arrows);
                            Console.SetCursorPosition(0, 0);
                        }
                        Thread.Sleep(delay);
                    }
                    Console.SetCursorPosition(0, s + 1);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Press ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" 'b'");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" to go back to main menu") ;
                }

                if (Console.KeyAvailable)
                {
                    char key = Console.ReadKey().KeyChar;
                    {
                        if (key == 'b') //going back to main menu
                        {
                            goto firstMenu;
                        }
                    }
                }
                sum += snakes[0].l;
                Console.Clear();
                PrintBoard();

                if (othersAlive)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine("you lost!!!");
                        Thread.Sleep(100);
                    }
                    Thread.Sleep(250);
                }
                else if (gameMod == 2 && snknum != 1)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine("you Won!!!");
                        Thread.Sleep(100);
                    }
                    Thread.Sleep(550);
                }

                Thread.Sleep(2000);


                othersAlive = true;
                alive = true;
                snakes[0].ttl = snakes[0].ttlConst;
            }


            Console.SetCursorPosition(40, 40);
            Console.WriteLine(sum / gameNum);
        }
    }
}
