using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AttemptedSnake
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }
        static void Menu()//The menu where the title is announced and the choices are made
        {
            Int32 Answer;
            Boolean Valid=true;
            Int32 WindowX = 45, WindowY = 20;
            
            do
            {
                Console.SetWindowSize(WindowX, WindowY);
                //Title
                //Got this title for test to ascii art converter I found online
                Console.WriteLine("   _____             _          ___    ___  ");
                Console.WriteLine("  / ____|           | |        |__    / _   ");
                Console.WriteLine(" | (___  _ __   __ _| | _____     ) || | | |");
                Console.WriteLine("   ___  | '_   / _` | |/ / _     / / | | | |");
                Console.WriteLine("  ____) | | | | (_| |   <  __/  / /_ | |_| |");
                Console.WriteLine(" |_____/|_| |_| __,_|_| _ ___| |____(_)___/ ");
                //Menu
                Console.WriteLine(" ");
                Console.WriteLine("");
                Console.WriteLine(" Please choose one of the following options");
                Console.WriteLine("                 1. Play");
                Console.WriteLine("                 2. Instructions ");
                Console.WriteLine("                     (Read me!)");
                Console.WriteLine("                 3. Exit");
                Console.Write("                 Choice: ");
                //Answer intake
                Valid = Int32.TryParse(Console.ReadLine(), out Answer);
                if (!Valid || Answer > 3 || Answer < 1)
                {
                    Console.Write("        Please select a valid option");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.Clear();
                }
                switch (Answer)
                {
                    case 1:
                        TheSnake();
                        break;
                    case 2:
                        Instructions();
                        break;
                    case 3:
                        break;
                }
            } while (Answer != 3 || !Valid || Answer > 3 || Answer < 1);
        } 
        static void Instructions()//Game instructions
        {
            Console.WriteLine("");
            Console.WriteLine("  Welcome to the brand new version of snake!");
            Console.WriteLine("");
            Console.WriteLine(" Just like the original, you will be playing");
            Console.WriteLine("    a starving snake that glides around in ");
            Console.WriteLine(" search for some delicious apples to eat and ");
            Console.WriteLine(" grow. As you glide faster, you will need to ");
            Console.WriteLine("  watch out for the walls and your own tail.");
            Console.WriteLine("");
            Console.WriteLine("                There's a twist! ");
            Console.WriteLine("    There will be multiple apples(@), only ");
            Console.WriteLine("            eat the RED ones. Good luck!");
            Console.WriteLine(" ");
            Console.ReadKey();
            Console.Clear();
        }
        static void TheSnake()
        {
            Console.CursorVisible = false;
            Int32 WindowX = 100, WindowY = 40;
            Console.SetWindowSize(WindowX, WindowY);
            ConsoleKey Move = ConsoleKey.NoName;
            Int32 CursorX = Console.WindowWidth / 2;
            Int32 CursorY = Console.WindowHeight / 2;
            Console.SetCursorPosition(CursorX, CursorY);
            String Direction = "none";
            char Character = 'o';
            bool GameOver = false;
            Int32 Score = 0;
            Int32 SnakeLength = 1;
            Int32 AppleX=0, AppleY=0;
            Int32 PoisonX = 0, PoisonY = 0;
            List<int> snakeX = new List<int>();
            List<int> snakeY = new List<int>();
            snakeX.Add(CursorX);
            snakeY.Add(CursorY);

            Poison(ref PoisonX, ref PoisonY);
            GoodApple(ref AppleX, ref AppleY);
            Limits();

            do
            {
                CheckApple(CursorX,CursorY, ref AppleX, ref AppleY, ref Score,ref PoisonX,ref PoisonY, ref SnakeLength);
                Console.ForegroundColor = ConsoleColor.White;

                Buffers(ref CursorX, ref CursorY);
                GameOver = Collisions(CursorX, CursorY, ref PoisonX, ref PoisonY);
                if (GameOver)
                {
                    break;
                }
                Console.SetCursorPosition(CursorX, CursorY);
                
                Moving(Move, ref Direction, SnakeLength);
                Gliding(Direction, ref CursorX, ref CursorY, Character, SnakeLength, ref snakeX, ref snakeY);
                GameOver=TouchTail(GameOver, snakeX, snakeY, CursorX, CursorY);
                if (GameOver)
                {
                    break;
                }
                PlayerPosition(CursorX,CursorY, ref snakeX, ref snakeY );
                SnakeSize(Character, SnakeLength, CursorX, CursorY, ref snakeX, ref snakeY);
                

            } while ( Move != ConsoleKey.Escape);
            GameOverScreen(Score);
        }
        static void Moving(ConsoleKey Move, ref string Direction, Int32 SnakeLength)
        {
            Wait(SnakeLength);
            if (Console.KeyAvailable)
            { 
                Move = Console.ReadKey(true).Key;
                switch (Move)
                {
                    case ConsoleKey.RightArrow:
                        Direction = "Right";
                        break;
                    case ConsoleKey.LeftArrow:
                        Direction = "Left";
                        break;
                    case ConsoleKey.UpArrow:
                        Direction = "Up";
                        break;
                    case ConsoleKey.DownArrow:
                        Direction = "Down";
                        break;
                }
            }
        }
        static void Wait(Int32 SnakeLength)
        {
            Int32 TimeAsleep;
            if (SnakeLength >= 20)
            {
                TimeAsleep = 170;
            }
            TimeAsleep = 200 - (SnakeLength*10 );
            Thread.Sleep(TimeAsleep);
        }
        static void Gliding (String Direction, ref Int32 CursorX, ref Int32 CursorY, char Character, Int32 SnakeLength, ref List<int> snakeX, ref List<int> snakeY)
        {

            switch (Direction)
            {
                case "Right":
                    CursorX += 1;
                    break;
                case "Left":
                    CursorX -= 1;                   
                    break;
                case "Up":
                    CursorY -= 1;                   
                    break;
                case "Down":
                    CursorY += 1;                    
                    break;
            }
        }
        static void PlayerPosition(Int32 CursorX, Int32 CursorY, ref List<int> snakeX, ref List<int> snakeY)
        {
            snakeX.Add(CursorX);
            snakeY.Add(CursorY);
        }
        static void GoodApple(ref Int32 AppleX, ref Int32 AppleY)//This function generate edible apples
        {
            Console.ForegroundColor = ConsoleColor.Red;//to make the apples red and disinguish them from the poison apples
            Random r = new Random();
            AppleX = r.Next(1, Console.WindowWidth - 2);
            AppleY = r.Next(1, Console.WindowHeight - 2);
            Console.SetCursorPosition(AppleX, AppleY);
            Console.Write("@");
        }
        static void Poison(ref Int32 PoisonX, ref Int32 PoisonY)
        {
            Console.ForegroundColor = ConsoleColor.Green;//to make the poison disinct
            Random r = new Random();
            PoisonX = r.Next(10, Console.WindowWidth - 10);
            PoisonY = r.Next(10, Console.WindowHeight - 10);
            Console.SetCursorPosition(PoisonX, PoisonY);
            Console.Write("@");
        }
        static void CheckApple(Int32 CursorX, Int32 CursorY, ref Int32 AppleX, ref Int32 AppleY, ref Int32 Score, ref Int32 PoisonX, ref Int32 PoisonY,ref Int32 SnakeLength)
        { 
            if (CursorX == AppleX && CursorY == AppleY)
            {
                Score += 100;
                SnakeLength += 1;
                Console.SetCursorPosition(PoisonX, PoisonY);
                Console.Write(" ");
                Poison(ref PoisonX,ref PoisonY);
                GoodApple(ref AppleX, ref AppleY);
            }
        }
        static void Limits()//This function draw the limits of the game and keeps the snake within them
        {
            //printing out the box of the limit
            Console.ForegroundColor = ConsoleColor.White;
            for (int x = 0; x < Console.WindowHeight; x++)//Lefthand & Righthand wall
            {
                Console.SetCursorPosition(0, x);
                Console.Write("X");
                Console.SetCursorPosition(Console.WindowWidth - 1, x);
                Console.Write("X");
            }
            for (int x = 0; x < Console.WindowWidth; x++)//Lower &Top wall
            {
                Console.SetCursorPosition(x, Console.WindowHeight - 1);
                Console.Write("X");
                Console.SetCursorPosition(x, 0);
                Console.Write("X");
            }
            Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
        }
        static void Buffers(ref Int32 CursorX, ref Int32 CursorY)//stoping the cursor from going out of range and making the whole thing crash
        {
            
            if (CursorX < 0)//Lefthand buffer
            {
                CursorX = 0;
            }
            if (CursorX >= Console.WindowWidth)//righthand buffer
            {
                CursorX = Console.WindowWidth - 1;
            }
            if (CursorY >= Console.WindowHeight)//top buffer
            {
                CursorY = Console.WindowHeight - 1;
            }
            if (CursorY < 0)//lower buffer
            {
                CursorY = 0;
            }
            Console.SetCursorPosition(CursorX, CursorY);
        }
        static bool Collisions(Int32 CursorX, Int32 CursorY, ref Int32 PoisonX, ref Int32 PoisonY)//Makes sure you die when you bump into something you're not suppoes to
        {
            bool GameOver = false;
            if (CursorX<=0||CursorX>=Console.WindowWidth-1)//If X touches the wall
            {
                GameOver = true;
            }
            if (CursorY <= 0 || CursorY >= Console.WindowHeight - 1)//If Y touches the wall
            {
                GameOver = true;
            }
            if (CursorX == PoisonX && CursorY == PoisonY)
            {
                GameOver = true;
            }
            
            return GameOver;
        }
        static void SnakeSize(char Character,Int32 SnakeLength, Int32 CursorX, Int32 CursorY, ref List<int> snakeX, ref List<int> snakeY)
        {
            while (snakeX.Count > SnakeLength)
            {
                Console.SetCursorPosition(snakeX[0], snakeY[0]);
                Console.Write(" ");
                snakeX.RemoveAt(0);
                snakeY.RemoveAt(0);
            }
            Console.SetCursorPosition(CursorX, CursorY);
            Console.Write(Character);
            
        }
        static Boolean TouchTail(Boolean GameOver, List<int> snakeX, List<int> snakeY, Int32 CursorX, Int32 CursorY)
        {
            for (int z=0; z < snakeX.Count; z++)//snakeX and snakeY are always the same length so you only need to check one
            {
                if (snakeX[z] == CursorX && snakeY[z] == CursorY && snakeX.Count>1)
                {
                    GameOver = true;
                }
            }
            return GameOver;
        }
        static void GameOverScreen (Int32 Score)//Final screen announces that the games is over and annouces the points
        {
            Console.Clear();
            Console.SetCursorPosition(45, 15);
            Console.WriteLine("Game Over");
            Console.SetCursorPosition(39, 16);
            Console.Write("You scored " + Score + " point!");
            WaitAgain();
            Console.ReadKey();
            Console.Clear();
        }
        static void WaitAgain()
        {
            Thread.Sleep(500);
        }
    }
}
