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
        public enum BOUNDS { UP = 0, RIGHT = 300, DOWN = 420, LEFT = 0 };
        public enum DIRECTION { UP, RIGHT, DOWN, LEFT, NONE };

        [XMLIgnore]
        private Random rand;
        [XmlIgnore]
        public Canvas canvas { get; set; }        
        public Player player { get; set; }
        public HeroShip heroShip { get; set; }
        public List<Bullet> heroBullets { get; set; }
        public List<Enemy> enemies { get; set; }

        public GameBoard()
        {
            rand = new Random();
            canvas = new Canvas();
            player = new Player();

            heroShip = new HeroShip();
            heroShip.position = new Point(150, 400);
            heroShip.setOnBoard(canvas);
            heroBullets = new List<Bullet>();
            enemies = new List<Enemy>();
        }

        public void updateGame()
        {
            checkCrashes();
            int hits = checkBulletsHits();
            player.score += hits;
            if (rand.Next(0, 1000) < 10)
            {
                generateEnemy(rand.Next(0, 300));
            }
            foreach (Bullet bullet in heroBullets)
            {
                bullet.move(canvas, DIRECTION.UP);
            }
            foreach (Enemy enemy in enemies)
            {
                enemy.move(canvas, DIRECTION.DOWN);
            }
        }

        public void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    heroShip.move(canvas, DIRECTION.LEFT);
                    break;
                case Key.Right:
                    heroShip.move(canvas, DIRECTION.RIGHT);
                    break;
                case Key.Up:
                    heroShip.move(canvas, DIRECTION.UP);
                    break;
                case Key.Down:
                    heroShip.move(canvas, DIRECTION.DOWN);
                    break;
                case Key.Space:
                    shootBullet();
                    break;
            }
        }

        public void shootBullet()
        {
            Bullet bullet = new Bullet();
            int positionX = (int)heroShip.position.X + (20 - 10) / 2;
            bullet.position = new Point(positionX, heroShip.position.Y - 5);
            bullet.setOnBoard(canvas);
            heroBullets.Add(bullet);
        }

        public void generateEnemy(int startingPositionX)
        {
            Enemy enemy = new Enemy();
            enemy.position = new Point(startingPositionX, 0);
            enemy.setOnBoard(canvas);
            enemies.Add(enemy);
        }

        public bool checkCrashes()
        {
            if ((heroShip.position.X < (int)BOUNDS.LEFT) || (heroShip.position.X > (int)BOUNDS.RIGHT) ||
                (heroShip.position.Y < (int)BOUNDS.UP) || (heroShip.position.Y > (int)BOUNDS.DOWN))
            {
                heroShip.armour -= 1;
                return true;
            }

            foreach (Enemy enemy in enemies)
            {
                if (heroShip.isCollidingWith(enemy))
                {
                    return true;
                }
            }
            return false;
        }

        public int checkBulletsHits()
        {
            List<Bullet> bulletsToDel = new List<Bullet>();
            List<Enemy> enemiesToDel = new List<Enemy>();
            foreach (Bullet bullet in heroBullets)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (bullet.isCollidingWith(enemy))
                    {
                        bulletsToDel.Add(bullet);
                        enemiesToDel.Add(enemy);
                    }
                }
            }
            foreach (Bullet bulletToDel in bulletsToDel)
            {
                bulletToDel.removeFromBoard(canvas);
                heroBullets.Remove(bulletToDel);
            }
            foreach (Enemy enemyToDel in enemiesToDel)
            {
                enemyToDel.removeFromBoard(canvas);
                enemies.Remove(enemyToDel);
            }
            return enemiesToDel.Count;
        }

        internal void save()
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(List<FlyingObject>));

            System.IO.StreamWriter file = new System.IO.StreamWriter(
                "saveObj.xml");
            writer.Serialize(file, heroBullets);

        }

        public bool isGameOver()
        {
            return heroShip.armour == 0;
        }
    }
}
