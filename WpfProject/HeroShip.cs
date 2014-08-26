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
    [Serializable()]
    public class HeroShip : FlyingObject
    {
        public HeroShip()
        {
            shape = new Ellipse();
            shape.Width = 20;
            shape.Height = 20;
            shape.Fill = Brushes.Aqua;

            speed = 5;
        }

        public void placeOnStartingPosition()
        {
            position = new Point(150, 400); 
        }
    }
}
