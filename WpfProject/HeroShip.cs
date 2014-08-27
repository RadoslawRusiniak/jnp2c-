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
            armour = 3;
        }

        public void placeOnStartingPosition()
        {
            position = new Point(150, 400); 
        }

        public Bullet shootBullet(System.Windows.Controls.Canvas board)
        {
            Bullet bullet = new Bullet();
            int positionX = (int)position.X + (int)(shape.Width - bullet.shape.Width) / 2;
            bullet.position = new Point(positionX, position.Y - 5);
            bullet.setOnBoard(board);
            return bullet;
        }
    }
}
