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
    public class Game
    {
        public enum BOUNDS { UP = 0, RIGHT = 300, DOWN = 420, LEFT = 0 };
        public enum DIRECTION { UP, RIGHT, DOWN, LEFT, NONE };

        [XmlIgnore]
        private Random rand;
        [XmlIgnore]
        public Canvas board { get; set; }      
        public Player player { get; set; }
        public HeroShip heroShip { get; set; }
        public List<Bullet> heroBullets { get; set; }
        public List<Enemy> enemies { get; set; }

        public Game()
        {
            rand = new Random();
            board = new Canvas();
            player = new Player();

            heroShip = new HeroShip();
            heroShip.placeOnStartingPosition();
            heroShip.setOnBoard(board);

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
                bullet.move(board, DIRECTION.UP);
            }
            foreach (Enemy enemy in enemies)
            {
                enemy.move(board, DIRECTION.DOWN);
            }
        }

        public void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    heroShip.move(board, DIRECTION.LEFT);
                    break;
                case Key.Right:
                    heroShip.move(board, DIRECTION.RIGHT);
                    break;
                case Key.Up:
                    heroShip.move(board, DIRECTION.UP);
                    break;
                case Key.Down:
                    heroShip.move(board, DIRECTION.DOWN);
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
            bullet.setOnBoard(board);
            heroBullets.Add(bullet);
        }

        public void generateEnemy(int startingPositionX)
        {
            Enemy enemy = new Enemy();
            enemy.position = new Point(startingPositionX, 0);
            enemy.setOnBoard(board);
            enemies.Add(enemy);
        }

        public bool checkCrashes()
        {
            if ((heroShip.position.X < (int)BOUNDS.LEFT) || (heroShip.position.X > (int)BOUNDS.RIGHT) ||
                (heroShip.position.Y < (int)BOUNDS.UP) || (heroShip.position.Y > (int)BOUNDS.DOWN))
            {
                heroShip.armour -= 1;
                heroShip.placeOnStartingPosition();
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
                bulletToDel.removeFromBoard(board);
                heroBullets.Remove(bulletToDel);
            }
            foreach (Enemy enemyToDel in enemiesToDel)
            {
                enemyToDel.removeFromBoard(board);
                enemies.Remove(enemyToDel);
            }
            return enemiesToDel.Count;
        }

        public bool isGameOver()
        {
            return heroShip.armour == 0;
        }
    }
}
