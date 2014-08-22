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
    public class Ship
    {
        //public Ellipse shipShape {get; set;}
        public int armour { get; set; }

        public Ship()
        {
            armour = 1;
        }
    }
}
