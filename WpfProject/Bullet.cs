using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfProject
{
    public class Bullet : FlyingObject
    {
        public Bullet()
        {
            shape = new Ellipse();
            shape.Width = 10;
            shape.Height = 10;
            shape.Fill = Brushes.Yellow;

            speed = 3;
        }
    }
}
