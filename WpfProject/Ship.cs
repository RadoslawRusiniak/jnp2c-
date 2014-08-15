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
    class Ship
    {
        public Ellipse shipShape {get; set;}
        //private int armour = 1;

        public Ship()
        {
            shipShape = new Ellipse();
            shipShape.Fill = Brushes.Aqua;
            shipShape.Width = 20;
            shipShape.Height = 20;
        }
    }
}
