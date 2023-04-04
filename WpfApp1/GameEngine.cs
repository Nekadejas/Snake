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

        private Snake _snake;
        private int _cubeSize = 3;
        public Canvas Board;
        
        public GameEngine(Canvas gameboard)
        {
            this.Board = gameboard;
            
            _snake = new Snake(_cubeSize, _cubeSize, Brushes.White,GenerateRandomPosY(gameboard),GenerateRandomPosX(gameboard));
            string scoreString = "Score: " + (_snake.FoodEaten - 3).ToString();
            gameboard.Resources.Add("MyScore", scoreString);
            gameboard.Resources.Add("HighScore","Highscore: " + GetHighScore().ToString());
        }
        
        public void GameLoop()
        {
            Food food = new Food(_cubeSize,_cubeSize,Brushes.Green,GenerateRandomPosY(Board), GenerateRandomPosX(Board));
            _snake.Draw(Board);
            Task.Run(() =>
            {
                while (true)
                {
                    this.Dispatcher.Invoke((() =>
                    {
                            _snake.Update(Board);
                            _snake.DrawNext(Board,_snake);
                            GreenFoodManager(food, Board);

                        if (_snake.IsDead(Board) == true)
                        {
                            NewHighScore(_snake);
                            GetHighScore();
                            MessageBox.Show("GameOver!");
                            this.Dispatcher.InvokeShutdown();
                        }
                    }));
                    Thread.Sleep(300 - _snake.FoodEaten*5);
                }
            });
        }
        public int GenerateRandomPosX(Canvas gameboard)
        {
            Random random = new Random();
            double x = gameboard.Width / _cubeSize;

            int randomPosX = random.Next(0, (int)x) * _cubeSize;

            return randomPosX;
        }
        public int GenerateRandomPosY(Canvas gameboard)
        {
            Random random = new Random();

            double y = gameboard.Height / _cubeSize;
            int randomPosY = random.Next(0, (int)y) * _cubeSize;

            return randomPosY;
        }
       
        public void UpdateMovement(Key key)
        {
            _snake.UpdateMovement(key);
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
        public static void NewHighScore(Snake snake)
        {
            if (GetHighScore() < snake.FoodEaten - 3)
            {
                File.WriteAllText("HighScore.txt", (snake.FoodEaten - 3).ToString());
            }
        }
    }
}
