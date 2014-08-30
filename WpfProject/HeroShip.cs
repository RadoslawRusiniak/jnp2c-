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
    public class HeroShip : FlyingObject
    {
        public HeroShip()
        {
            setShape(20, Brushes.Aqua);
            speed = 5;
            armor = 3;
        }

        public void placeOnStartingPosition()
        {
            position = new Point((int)WpfProject.Game.BOUNDS.RIGHT / 2, (int)WpfProject.Game.BOUNDS.DOWN - 50); 
        }

        public Bullet shootBullet()
        {
            Bullet bullet = new Bullet();
            int positionX = (int)position.X + (int)(shape.Width - bullet.shape.Width) / 2;
            bullet.position = new Point(positionX, position.Y - 5);
            return bullet;
        }
    }
}
