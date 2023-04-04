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
        private Rectangle _body;
        private List<Rectangle> _fullBody;
        public int FoodEaten { get; private set; }
        private int _width;
        private int _height;
        private Brush _snakeColor;
        private int _posY;
        private int _posX;
        public int MoveDirectionHor;
        public int MoveDircetionVer;
        private Key _moveDir;

        public Snake(int width, int height, Brush snakeColor, int posY, int posX)
        {
            _body = new Rectangle();
            _fullBody = new List<Rectangle>();
            _width = width;
            _height = height;
            _snakeColor = snakeColor;
            _posY = posY;
            _posX = posX;
            _body.Width = width;
            _body.Height = height;
            _body.Fill = snakeColor;
            MoveDirectionHor = width;
            FoodEaten = 3;
            for (int i = 0; i < FoodEaten-1; i++)
            {

                _fullBody.Add(CreateBodyPart());
            }
        }
        public void Update(Canvas canvas)
        {
            UpdateKey(_moveDir);
            _posY = _posY + MoveDircetionVer;
            _posX = _posX + MoveDirectionHor;
            MoveRestriction(canvas);
        }
        public void UpdateKey(Key keyPressed)
        {

            switch (keyPressed)
            {
                case Key.Up:
                    if (MoveDircetionVer == _width)
                    {
                        return;
                    }
                    MoveDircetionVer = -_width;
                    MoveDirectionHor = 0;

                    break;
                case Key.Down:

                    if (MoveDircetionVer == -_width)
                    {
                        return;
                    }
                    MoveDircetionVer = _width;
                    MoveDirectionHor = 0;

                    break;
                case Key.Left:
                    if (MoveDirectionHor == _height)
                    {
                        return;
                    }
                    MoveDirectionHor = -_height;
                    MoveDircetionVer = 0;
                    break;
                case Key.Right:
                    if (MoveDirectionHor == -_height)
                    {
                        return;
                    }
                    MoveDirectionHor = _height;
                    MoveDircetionVer = 0;
                    break;
                default:
                    break;
            }
        }
        
        public void Draw(Canvas canvas)
        {
            int i = _width;
            foreach (Rectangle body in _fullBody)
            {
                _posX = _posX + i;
                Canvas.SetLeft(body, _posX);
                Canvas.SetTop(body, _posY);
                canvas.Children.Add(body);
            }
        }
        public void DrawNext(Canvas canvas,Snake snake)
        {
            _fullBody.Add(CreateBodyPart());
            Canvas.SetLeft(_fullBody[_fullBody.Count - 1], _posX);
            Canvas.SetTop(_fullBody[_fullBody.Count - 1], _posY);
            canvas.Children.Add(_fullBody[_fullBody.Count - 1]);
            
            Eat(canvas, snake);
            if (FoodEaten < _fullBody.Count)
            {
                canvas.Children.Remove(_fullBody[0]);
                _fullBody.RemoveAt(0);


            }
        }
       
        public void SetCanvas(Canvas canvas)
        {
            foreach (Rectangle body in _fullBody)
            {
                canvas.Children.Add(body);
            }
        }
        public Rectangle CreateBodyPart()
        {
            Rectangle body = new Rectangle();

            body.Width = this._width;
            body.Height = this._height;
            body.Fill = _snakeColor;
            return body;
        }
        public void MoveRestriction(Canvas canvas)
        {
            if (_posY >= canvas.Height) // Going Down
            {
                _posY = 0;
            }
            else if (_posY <= -1)// Going Up
            {
                _posY = (int)canvas.Height - _height;
            }
            else if (_posX >= canvas.Width) //Going Right
            {
                _posX = 0;
            }

            else if (_posX <= -1) // Going Left
            {
                _posX = (int)canvas.Width - _width;
            }
        }

        public void UpdateMovement(Key keyInput)
        {
            _moveDir = keyInput;
        }
        public List<Rectangle> GetBody()
        {
            return _fullBody;

        }
        public void Eat(Canvas canvas,Snake snake)
        {
            List<Rectangle> FoodList= new List<Rectangle>();
            foreach (System.Windows.Shapes.Rectangle rect in canvas.Children.OfType<Rectangle>())
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
                if(x == _posX && y == _posY)
                {
                    FoodEaten++;
                    canvas.Children.Remove(food);
                    canvas.Resources.Remove("MyScore");
                    string scoreString = "Score: " + (snake.FoodEaten -3).ToString();
                    canvas.Resources.Add("MyScore", scoreString);
                }
            }
        }
        
        public bool IsDead(Canvas canvas)
        {
            for (int i = 0; i < _fullBody.Count -1; i++)
            {
                double x = Canvas.GetLeft(_fullBody[i]);
                double y = Canvas.GetTop(_fullBody[i]);
                if (x == (double)_posX && y == (double)_posY)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
