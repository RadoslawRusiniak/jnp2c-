using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace WpfProject
{
    [Serializable()]
    public class Board
    {
        public enum BOUNDS { UP = 0, RIGHT = 300, DOWN = 420, LEFT = 0 };

        [XmlIgnore]
        public Canvas plane { get; set; }

        public Board()
        {
            plane = new Canvas();
        }

        internal void setOnBoard(Shape shape, Point position)
        {
            removeFromBoard(shape);
            Canvas.SetLeft(shape, position.X);
            Canvas.SetTop(shape, position.Y);
            plane.Children.Add(shape);
        }

        internal void removeFromBoard(Shape shape)
        {
            if (plane.Children.Contains(shape))
            {
                plane.Children.Remove(shape);
            }
        }

        internal void move(FlyingObject fo, WpfProject.Game.DIRECTION direction)
        {
            removeFromBoard(fo.shape);
            switch (direction)
            {
                case Game.DIRECTION.UP:
                    fo.position = new Point(fo.position.X, fo.position.Y - fo.speed);
                    break;
                case Game.DIRECTION.RIGHT:
                    fo.position = new Point(fo.position.X + fo.speed, fo.position.Y);
                    break;
                case Game.DIRECTION.DOWN:
                    fo.position = new Point(fo.position.X, fo.position.Y + fo.speed);
                    break;
                case Game.DIRECTION.LEFT:
                    fo.position = new Point(fo.position.X - fo.speed, fo.position.Y);
                    break;
            }
            setOnBoard(fo.shape, fo.position);
        }

        public bool isWithinBounds(Point position)
        {
            if ((position.X >= (int)BOUNDS.LEFT) && (position.X <= (int)BOUNDS.RIGHT) &&
                (position.Y >= (int)BOUNDS.UP) && (position.Y <= (int)BOUNDS.DOWN))
            {
                return true;
            }
            return false;
        }

        public bool areColliding(Shape shape1, Point position1, Shape shape2, Point position2)
        {
            if (Math.Pow(position1.X - position2.X, 2) + Math.Pow(position1.Y - position2.Y, 2)
                <= Math.Pow((shape1.Width + shape2.Width) / 2, 2))
            {
                return true;
            }
            return false;
        }
    }
}
