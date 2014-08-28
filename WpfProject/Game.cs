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
        public Canvas board { get; set; }      
        public Player player { get; set; }
        public Level level { get; set; }
        public HeroShip heroShip { get; set; }
        public List<Bullet> heroBullets { get; set; }
        public List<Enemy> enemies { get; set; }

        public Game()
        {
            board = new Canvas();
            player = new Player();
            level = new Level();

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
            level.updateLevel(player.score);
            Enemy generatedEnemy = level.generateEnemy(board);
            if (generatedEnemy != null)
            {
                enemies.Add(generatedEnemy);
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

        internal void OnButtonKeyDown(object sender, KeyEventArgs e)
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
                    Bullet bullet = heroShip.shootBullet(board);
                    heroBullets.Add(bullet);
                    break;
            }
        }

        private void checkCrashes()
        {
            if ((heroShip.position.X < (int)BOUNDS.LEFT) || (heroShip.position.X > (int)BOUNDS.RIGHT) ||
                (heroShip.position.Y < (int)BOUNDS.UP) || (heroShip.position.Y > (int)BOUNDS.DOWN))
            {
                heroShip.armour -= 1;
                heroShip.placeOnStartingPosition();
            }

            foreach (Enemy enemy in enemies)
            {
                if (heroShip.checkCollision(enemy))
                {
                    enemy.removeFromBoard(board);
                    enemies.Remove(enemy);
                    return;
                }
            }
        }

        private int checkBulletsHits()
        {
            List<Bullet> bulletsToDel = new List<Bullet>();
            List<Enemy> enemiesToDel = new List<Enemy>();
            foreach (Bullet bullet in heroBullets)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (bullet.checkCollision(enemy))
                    {
                        bulletsToDel.Add(bullet);
                        if (enemy.armour == 0)
                        {
                            enemiesToDel.Add(enemy);
                        }
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

        internal bool isGameOver()
        {
            return heroShip.armour <= 0;
        }

        internal void load()
        {
            heroShip.setOnBoard(board);
        }
    }
}
