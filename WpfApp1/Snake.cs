using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WpfApp1
{
    class Snake
    {
        private Rectangle body;
        private List<Rectangle> FullBody;
        private int FoodEaten;
        private int Width;
        private int Height;
        private Brush SnakeColor;
        private int PosY;
        private int PosX;
        public int MoveDirectionHor;
        public int MoveDircetionVer = 0;
        private Key moveDir;

        

        public Snake(int width, int height, Brush snakeColor, int posY, int posX)
        {
            body = new Rectangle();
            FullBody = new List<Rectangle>();
            Width = width;
            Height = height;
            SnakeColor = snakeColor;
            PosY = posY;
            PosX = posX;
            body.Width = width;
            body.Height = height;
            body.Fill = snakeColor;
            MoveDirectionHor = width;
            FoodEaten = 3;
            for (int i = 0; i < FoodEaten-1; i++)
            {

                FullBody.Add(CreateBodyPart());
            }
        }
        
        public void Update(Canvas canvas)
        {
            UpdateKey(moveDir);
            PosY = PosY + MoveDircetionVer;
            PosX = PosX + MoveDirectionHor;
            MoveRestriction(canvas);
        }
        public void UpdateKey(Key keyPressed)
        {

            switch (keyPressed)
            {
                case Key.Up:
                    if (MoveDircetionVer == Width)
                    {
                        return;
                    }
                    MoveDircetionVer = -Width;
                    MoveDirectionHor = 0;

                    break;
                case Key.Down:

                    if (MoveDircetionVer == -Width)
                    {
                        return;
                    }
                    MoveDircetionVer = Width;
                    MoveDirectionHor = 0;

                    break;
                case Key.Left:
                    if (MoveDirectionHor == Height)
                    {
                        return;
                    }
                    MoveDirectionHor = -Height;
                    MoveDircetionVer = 0;
                    break;
                case Key.Right:
                    if (MoveDirectionHor == -Height)
                    {
                        return;
                    }
                    MoveDirectionHor = Height;
                    MoveDircetionVer = 0;
                    break;
                default:
                    break;
            }
        }
        
        public void Draw(Canvas canvas)
        {
            int i = Width;
            foreach (Rectangle body in FullBody)
            {
                PosX = PosX + i;
                Canvas.SetLeft(body, PosX);
                Canvas.SetTop(body, PosY);
                canvas.Children.Add(body);
            }


        }
        public void DrawNext(Canvas canvas,Snake snake)
        {



            FullBody.Add(CreateBodyPart());
            Canvas.SetLeft(FullBody[FullBody.Count - 1], PosX);
            Canvas.SetTop(FullBody[FullBody.Count - 1], PosY);
            canvas.Children.Add(FullBody[FullBody.Count - 1]);
            
            Eat(canvas, snake);
            if (FoodEaten < FullBody.Count)
            {
                canvas.Children.Remove(FullBody[0]);
                FullBody.RemoveAt(0);


            }


        }
       
        public void SetCanvas(Canvas canvas)
        {
            foreach (Rectangle body in FullBody)
            {
                canvas.Children.Add(body);
            }

        }
        public Rectangle CreateBodyPart()
        {
            Rectangle body = new Rectangle();

            body.Width = this.Width;
            body.Height = this.Height;
            body.Fill = SnakeColor;
            return body;
        }
        public void MoveRestriction(Canvas canvas)
        {
            if (PosY >= canvas.Height) // Going Down
            {
                PosY = 0;
            }
            else if (PosY <= -1)// Going Up
            {
                PosY = (int)canvas.Height - Height;
            }
            else if (PosX >= canvas.Width) //Going Right
            {
                PosX = 0;
            }

            else if (PosX <= -1) // Going Left
            {
                PosX = (int)canvas.Width - Width;
            }
        }

        public void UpdateMovement(Key keyInput)
        {
            moveDir = keyInput;
        }
        public List<Rectangle> GetBody()
        {
            return FullBody;

        }
        public void Eat(Canvas canvas,Snake snake)
        {
            List<Rectangle> FoodList= new List<Rectangle>();
            foreach (System.Windows.Shapes.Rectangle rect in canvas.Children.OfType<System.Windows.Shapes.Rectangle>())
            {
                if (rect.Fill == Brushes.Green)
                {
                    FoodList.Add(rect);

                }
                
            }
            foreach (Rectangle food in FoodList)
            {
                double x = Canvas.GetLeft(food);
                double y = Canvas.GetTop(food);
                if(x == PosX && y == PosY)
                {
                    FoodEaten++;
                    canvas.Children.Remove(food);
                    canvas.Resources.Remove("MyScore");
                    string scoreString = "Score: " + (snake.GetFoodEaten()-3).ToString();
                    canvas.Resources.Add("MyScore", scoreString);
                }
            }
        }
        public int GetFoodEaten()
        {
            return FoodEaten;
        }
        public bool IsDead(Canvas canvas)
        {
            for (int i = 0; i < FullBody.Count -1; i++)
            {
                double x = Canvas.GetLeft(FullBody[i]);
                double y = Canvas.GetTop(FullBody[i]);
                if (x == (double)PosX && y == (double)PosY)
                {
                    
                    return true;
                    
                }
            }
            return false;
        }
    }
}
