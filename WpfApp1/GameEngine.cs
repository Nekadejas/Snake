using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;
using Path = System.IO.Path;

namespace WpfApp1
{
    class GameEngine : DispatcherObject
    {

        Snake snake;

        private int cubeSize = 3;
        
        
        public Canvas Board;
        
        public GameEngine(Canvas gameboard)
        {
            this.Board = gameboard;
            
            snake = new Snake(cubeSize, cubeSize, Brushes.White,GenerateRandomPosY(gameboard),GenerateRandomPosX(gameboard));
            string scoreString = "Score: " + (snake.GetFoodEaten() - 3).ToString();
            gameboard.Resources.Add("MyScore", scoreString);
            gameboard.Resources.Add("HighScore","Highscore: " + GetHighScore().ToString());
        }
        
        
        public void GameLoop()
        {
            Food food = new Food(cubeSize,cubeSize,Brushes.Green,GenerateRandomPosY(Board), GenerateRandomPosX(Board));
            snake.Draw(Board);
            Task.Run(() =>
            {
                while (true)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                            snake.Update(Board);
                            snake.DrawNext(Board,snake);
                            GreenFoodManager(food, Board);

                        if (snake.IsDead(Board) == true)
                        {

                            HighScore(snake);
                            GetHighScore();
                            MessageBox.Show("GameOver!");
                            this.Dispatcher.InvokeShutdown();
                        }
                    }));
                    Thread.Sleep(300 - snake.GetFoodEaten()*5);
                }
            });
        }
        public int GenerateRandomPosX(Canvas gameboard)
        {
            Random random = new Random();
            double x = gameboard.Width / cubeSize;

            int randomPosX = random.Next(0, (int)x) * cubeSize;

            return randomPosX;
        }
        public int GenerateRandomPosY(Canvas gameboard)
        {
            Random random = new Random();

            double y = gameboard.Height / cubeSize;
            int randomPosY = random.Next(0, (int)y) * cubeSize;

            return randomPosY;
        }
       
        public void UpdateMovement(Key key)
        {
            snake.UpdateMovement(key);
        }
        public void GreenFoodManager(Food food,Canvas canvas)
        {
            List<Rectangle> snakebody = new List<Rectangle>();
            foreach (Rectangle item in canvas.Children.OfType<Rectangle>())
            {
                if(item.Fill == Brushes.White)
                {
                    snakebody.Add(item);
                }
            }
            
            int x = GenerateRandomPosX(canvas);
            int y = GenerateRandomPosY(canvas);
            for (int i = 0; i < snakebody.Count; i++)
            {
                if (x == Canvas.GetLeft(snakebody[i]) && y == Canvas.GetTop(snakebody[i]))
                {
                    x = GenerateRandomPosX(canvas);
                    y = GenerateRandomPosY(canvas);
                    i= 0;
                }
            }
            food.GreenFoodChecker(canvas,x,y);
        }
        public static int GetHighScore()
        {
            string x = File.ReadAllText("HighScore.txt");
            int highscore = int.Parse(x);
            return highscore;
        }
        public static void HighScore(Snake snake)
        {
            if (GetHighScore() < snake.GetFoodEaten() - 3)
            {
                
                File.WriteAllText("HighScore.txt", (snake.GetFoodEaten() - 3).ToString());
            }
        }
    }
}
