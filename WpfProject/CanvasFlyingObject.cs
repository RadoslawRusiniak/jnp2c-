using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace WpfProject
{
    [Serializable()]
    public class CanvasFlyingObject
    {
        [XmlIgnore]
        public Shape shape { get; set; }
        public Point position { get; set; }

        public CanvasFlyingObject()
        {
        }

        public CanvasFlyingObject(Point p, SolidColorBrush color, int height = 20, int width = 20)
        {
            position = p;
            paintShape(color, height, width);
        }

        public void paintShape(SolidColorBrush color, int height = 20, int width = 20)
        {
            shape = new Ellipse();
            shape.Fill = color;
            shape.Height = height;
            shape.Width = width;
        }
    }
}
