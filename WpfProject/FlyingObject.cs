using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfProject
{
    [Serializable()]
    public class FlyingObject
    {
        [XMLIgnore]
        public Shape shape { get; set; }
        public Point position { get; set; }
        public int speed { get; set; }
        public int armour { get; set; }

        public FlyingObject()
        {
            speed = 1;
            armour = 1;
        }

        public void setOnBoard(Canvas canvas)
        {
            Canvas.SetLeft(shape, position.X);
            Canvas.SetTop(shape, position.Y);
            canvas.Children.Add(shape);
        }

        public void removeFromBoard(Canvas canvas)
        {
            if (canvas.Children.Contains(shape))
            {
                canvas.Children.Remove(shape);
            }
        }

        public void move(Canvas canvas, WpfProject.Game.DIRECTION direction)
        {
            removeFromBoard(canvas);
            switch (direction)
            {
                case Game.DIRECTION.UP:
                    position = new Point(position.X, position.Y - speed);
                    break;
                case Game.DIRECTION.RIGHT:
                    position = new Point(position.X + speed, position.Y);
                    break;
                case Game.DIRECTION.DOWN:
                    position = new Point(position.X, position.Y + speed);
                    break;
                case Game.DIRECTION.LEFT:
                    position = new Point(position.X - speed, position.Y);
                    break;
            }
            setOnBoard(canvas);
        }

        public bool isCollidingWith(FlyingObject obj)
        {
            if (Math.Pow(position.X - obj.position.X, 2) + Math.Pow(position.Y - obj.position.Y, 2) 
                <= Math.Pow(shape.Width + obj.shape.Width, 2))
            {
                armour -= 1;
                obj.armour -= 1;
                return true;
            }
            return false;
        }
    }
}
