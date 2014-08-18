using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfProject
{
    class CanvasFlyingObject
    {
        public Shape shape { get; set; }
        public Point position { get; set; }

        public CanvasFlyingObject(Point p)
        {
            position = p;
        }

        public CanvasFlyingObject(Point p, SolidColorBrush color, int height, int width)
        {
            position = p;
            shape = new Ellipse();
            shape.Fill = color;
            shape.Height = height;
            shape.Width = width;
        }
    }
}
