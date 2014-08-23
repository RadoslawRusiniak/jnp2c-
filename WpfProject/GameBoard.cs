using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace WpfProject
{
    [Serializable()]
    public class GameBoard
    {
        enum BOUNDS {UP = 0, RIGHT = 300, DOWN = 420, LEFT = 0 };

        [XmlIgnore]
        public Canvas canvas { get; set; }
        public CanvasFlyingObject heroShip { get; set; }
        public List<CanvasFlyingObject> heroBullets { get; set; }
        public List<CanvasFlyingObject> enemies { get; set; }

        public GameBoard()
        {
            canvas = new Canvas();

            heroShip = new CanvasFlyingObject(new Point(150, 400), Brushes.Aqua, 20, 20);
            updateHeroShipPainting();
            heroBullets = new List<CanvasFlyingObject>();
            enemies = new List<CanvasFlyingObject>();
        }

        public void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    heroShip.position = new Point(heroShip.position.X - 5, heroShip.position.Y);
                    updateHeroShipPainting();
                    break;
                case Key.Right:
                    heroShip.position = new Point(heroShip.position.X + 5, heroShip.position.Y);
                    updateHeroShipPainting();
                    break;
                case Key.Up:
                    heroShip.position = new Point(heroShip.position.X, heroShip.position.Y - 5);
                    updateHeroShipPainting();
                    break;
                case Key.Down:
                    heroShip.position = new Point(heroShip.position.X, heroShip.position.Y + 5);
                    updateHeroShipPainting();
                    break;
                case Key.Space:
                    shootBullet();
                    break;
            }
        }

        public void shootBullet()
        {
            int positionX = (int)heroShip.position.X + (20 - 10) / 2;
            CanvasFlyingObject bullet = new CanvasFlyingObject(new Point(positionX, heroShip.position.Y - 5), Brushes.Yellow, 10, 10);

            Canvas.SetTop(bullet.shape, bullet.position.Y);
            Canvas.SetLeft(bullet.shape, bullet.position.X);

            canvas.Children.Add(bullet.shape);
            heroBullets.Add(bullet);
        }

        public void generateEnemy(int startingPositionX)
        {
            CanvasFlyingObject enemy = new CanvasFlyingObject(new Point(startingPositionX, 0), Brushes.Red, 20, 20);

            Canvas.SetTop(enemy.shape, enemy.position.Y);
            Canvas.SetLeft(enemy.shape, enemy.position.X);

            canvas.Children.Add(enemy.shape);
            enemies.Add(enemy);
        }

        public void moveFlyingObjects()
        {
            updateBulletsPainting();
            updateEnemiesPainting();
        }

        public bool checkCrashes()
        {
            if ((heroShip.position.X < (int)BOUNDS.LEFT) || (heroShip.position.X > (int)BOUNDS.RIGHT) ||
                (heroShip.position.Y < (int)BOUNDS.UP) || (heroShip.position.Y > (int)BOUNDS.DOWN))
            {
                return true;
            }

            foreach (CanvasFlyingObject enemy in enemies)
            {
                if (CFOsCollide(heroShip, enemy))
                {
                    return true;
                }
            }
            return false;
        }

        public int checkBulletsHits()
        {
            List<CanvasFlyingObject> bulletsToDel = new List<CanvasFlyingObject>();
            List<CanvasFlyingObject> enemiesToDel = new List<CanvasFlyingObject>();
            foreach (CanvasFlyingObject bullet in heroBullets)
            {
                foreach (CanvasFlyingObject enemy in enemies)
                {
                    if (CFOsCollide(bullet, enemy))
                    {
                        bulletsToDel.Add(bullet);
                        enemiesToDel.Add(enemy);
                    }
                }
            }
            foreach(CanvasFlyingObject bulletToDel in bulletsToDel)
            {
                canvas.Children.Remove(bulletToDel.shape);
                heroBullets.Remove(bulletToDel);
            }
            foreach (CanvasFlyingObject enemyToDel in enemiesToDel)
            {
                canvas.Children.Remove(enemyToDel.shape);
                enemies.Remove(enemyToDel);
            }
            return enemiesToDel.Count;
        }

        private bool CFOsCollide(CanvasFlyingObject cfo1, CanvasFlyingObject cfo2)
        {
            if (Math.Pow(cfo1.position.X - cfo2.position.X, 2)
               + Math.Pow(cfo1.position.Y - cfo2.position.Y, 2) <= Math.Pow((cfo1.shape.Width + cfo2.shape.Width) / 2, 2))
            {
                return true;
            }
            return false;
        }

        private void updateHeroShipPainting() {
            canvas.Children.Remove(heroShip.shape);

            Canvas.SetTop(heroShip.shape, heroShip.position.Y);
            Canvas.SetLeft(heroShip.shape, heroShip.position.X);

            canvas.Children.Add(heroShip.shape);
        }

        private void updateEnemiesPainting()
        {
            foreach (CanvasFlyingObject enemy in enemies)
            {
                canvas.Children.Remove(enemy.shape);
                enemy.position = new Point(enemy.position.X, enemy.position.Y + 1);
                if (enemy.position.Y < (int)BOUNDS.DOWN)
                {
                    Canvas.SetTop(enemy.shape, enemy.position.Y);
                    Canvas.SetLeft(enemy.shape, enemy.position.X);
                    canvas.Children.Add(enemy.shape);
                }
            }
        }

        private void updateBulletsPainting()
        {
            foreach (CanvasFlyingObject bullet in heroBullets)
            {
                canvas.Children.Remove(bullet.shape);
                bullet.position = new Point(bullet.position.X, bullet.position.Y - 3);
                if (bullet.position.Y > (int)BOUNDS.UP)
                {
                    Canvas.SetTop(bullet.shape, bullet.position.Y);
                    Canvas.SetLeft(bullet.shape, bullet.position.X);
                    canvas.Children.Add(bullet.shape);
                }
            }
        }

        internal void save()
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(List<CanvasFlyingObject>));

            System.IO.StreamWriter file = new System.IO.StreamWriter(
                "saveObj.xml");
            writer.Serialize(file, heroBullets);

        }
    }
}
