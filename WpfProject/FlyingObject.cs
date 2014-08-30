using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace WpfProject
{
    [Serializable()]
    public class FlyingObject
    {
        [XmlIgnore]
        public Shape shape { get; set; }
        public Point position { get; set; }
        public int speed { get; set; }
        public int armor { get; set; }

        public FlyingObject()
        {
            speed = 1;
            armor = 1;
        }

        public void setShape(int widthHeight, SolidColorBrush color)
        {
            shape = new Ellipse();
            shape.Width = widthHeight;
            shape.Height = widthHeight;
            shape.Fill = color;
        }

        public void reduceArmor()
        {
            armor -= 1;
        }

        internal bool isDestroyed()
        {
            return armor <= 0;
        }
    }
}
