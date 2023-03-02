using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    class Food
    {
        private int Width;
        private int Height;
        private Brush Color;
        public int FoodPosY;
        public int FoodPosX;
        private Rectangle body = new Rectangle();
        
        public Food(int width, int height, Brush color,int posY, int posX)
        {
            
            
            Width = width;
            Height = height;
            Color = color;
            body.Width = Width;
            body.Height = Height;
            body.Fill = Color;
            FoodPosY = posY;
            FoodPosX = posX;
            
        }
        public int GetFoodW(Rectangle body)
        {
            return Width;
        }
        public int GetFoodH(Rectangle body)
        {
            return Height;
        }
        public int FPosY(Rectangle body)
        {
            return FoodPosY;
        }
        public int FPosX(Rectangle body)
        {
            return FoodPosX;
        }
        public void Update()
        {
            


        }
        public void Destroy()
        {

        }
        public void Draw()
        {
            Canvas.SetLeft(body, FoodPosX);
            Canvas.SetTop(body, FoodPosY);


        }
        public void SetCanvas(Canvas canvas)
        {
            canvas.Children.Add(body);


        }
        public void DrawFirstTime()
        {
            Canvas.SetLeft(body, FoodPosX);
            Canvas.SetTop(body, FoodPosY);
        }
        public Rectangle CreateNewFood()
        {
            Rectangle food = new Rectangle();

            food.Width = this.Width;
            food.Height = this.Height;
            food.Fill = Color;
            return food;
            
        }
        public void GreenFoodChecker(Canvas canvas,int posX, int posY )
        {
            List<Rectangle> foodList = new List<Rectangle>();
            foreach (Rectangle item in canvas.Children.OfType<Rectangle>())
            {
                if(item.Fill == Brushes.Green)
                {
                    foodList.Add(item);
                }
            }
            if(foodList.Count == 0) 
            {
                Rectangle food = CreateNewFood();
                Canvas.SetLeft(food, posX);
                Canvas.SetTop(food, posY);
                canvas.Children.Add(food);
            }
        }


    }
}
