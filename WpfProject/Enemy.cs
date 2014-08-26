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
    [Serializable()]
    public class Enemy : FlyingObject
    {
        public Enemy()
        {
            shape = new Ellipse();
            shape.Width = 20;
            shape.Height = 20;
            shape.Fill = Brushes.Red;

            speed = 1;
        }
    }
}
