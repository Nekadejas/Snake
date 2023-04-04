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
        private int _width;
        private int _height;
        private Brush _color;
        public int FoodPosY;
        public int FoodPosX;
        private Rectangle _body = new Rectangle();
        
        public Food(int width, int height, Brush color,int posY, int posX)
        {
            _width = width;
            _height = height;
            _color = color;
            _body.Width = _width;
            _body.Height = _height;
            _body.Fill = _color;
            FoodPosY = posY;
            FoodPosX = posX;
        }
        public int GetFoodW(Rectangle body)
        {
            return _width;
        }
        public int GetFoodH(Rectangle body)
        {
            return _height;
        }
        public int FPosY(Rectangle body)
        {
            return FoodPosY;
        }
        public int FPosX(Rectangle body)
        {
            return FoodPosX;
        }
       
        public void Draw()
        {
            Canvas.SetLeft(_body, FoodPosX);
            Canvas.SetTop(_body, FoodPosY);
        }
        public void SetCanvas(Canvas canvas)
        {
            canvas.Children.Add(_body);
        }
        public void DrawFirstTime()
        {
            Canvas.SetLeft(_body, FoodPosX);
            Canvas.SetTop(_body, FoodPosY);
        }
        public Rectangle CreateNewFood()
        {
            Rectangle food = new Rectangle();

            food.Width = this._width;
            food.Height = this._height;
            food.Fill = _color;
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
