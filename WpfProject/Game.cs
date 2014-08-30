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

        public Board board { get; set; }
        public Player player { get; set; }
        public Level level { get; set; }
        public HeroShip heroShip { get; set; }
        public List<Bullet> heroBullets { get; set; }
        public List<Enemy> enemies { get; set; }

        public Game()
        {
            //boardCanvas = new Canvas();
            board = new Board();
            player = new Player();
            level = new Level();

            heroShip = new HeroShip();
            heroShip.placeOnStartingPosition();
            board.setOnBoard(heroShip.shape, heroShip.position);

            heroBullets = new List<Bullet>();
            enemies = new List<Enemy>();
        }

        public void updateGame()
        {
            checkCrashes();
            int hits = checkBulletsHits();
            player.score += hits;
            level.updateLevel(player.score);
            Enemy generatedEnemy = level.generateEnemy();
            if (generatedEnemy != null)
            {
                board.setOnBoard(generatedEnemy.shape, generatedEnemy.position);
                enemies.Add(generatedEnemy);
            }
            foreach (Bullet bullet in heroBullets)
            {
                board.move(bullet, DIRECTION.UP);
            }
            foreach (Enemy enemy in enemies)
            {
                board.move(enemy, DIRECTION.DOWN);
            }
        }

        internal void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    board.move(heroShip, DIRECTION.LEFT);
                    break;
                case Key.Right:
                    board.move(heroShip, DIRECTION.RIGHT);
                    break;
                case Key.Up:
                    board.move(heroShip, DIRECTION.UP);
                    break;
                case Key.Down:
                    board.move(heroShip, DIRECTION.DOWN);
                    break;
                case Key.Space:
                    Bullet bullet = heroShip.shootBullet();
                    board.setOnBoard(bullet.shape, bullet.position);
                    heroBullets.Add(bullet);
                    break;
            }
        }

        private void checkCrashes()
        {
            if (!board.isWithinBounds(heroShip.position))
            {
                heroShip.reduceArmor();
                heroShip.placeOnStartingPosition();
                board.setOnBoard(heroShip.shape, heroShip.position);
            }
            foreach (Enemy enemy in enemies)
            {
                if (board.areColliding(heroShip.shape, heroShip.position, enemy.shape, enemy.position))
                {
                    heroShip.reduceArmor();
                    board.removeFromBoard(enemy.shape);
                    enemies.Remove(enemy);
                    break;
                }
            }
        }

        private int checkBulletsHits()
        {
            List<Bullet> bulletsToDel = new List<Bullet>();
            List<Enemy> enemiesToDel = new List<Enemy>();
            int hits = 0;
            foreach (Bullet bullet in heroBullets)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (board.areColliding(bullet.shape, bullet.position, enemy.shape, enemy.position))
                    {
                        hits += 1;
                        bullet.reduceArmor();
                        enemy.reduceArmor();
                        if (bullet.isDestroyed())
                        {
                            bulletsToDel.Add(bullet);
                        }
                        if (enemy.isDestroyed())
                        {
                            enemiesToDel.Add(enemy);
                        }
                    }
                }
            }
            foreach (Bullet bulletToDel in bulletsToDel)
            {
                board.removeFromBoard(bulletToDel.shape);
                heroBullets.Remove(bulletToDel);
            }
            foreach (Enemy enemyToDel in enemiesToDel)
            {
                board.removeFromBoard(enemyToDel.shape);
                enemies.Remove(enemyToDel);
            }
            return hits;
        }

        internal bool isGameOver()
        {
            return heroShip.isDestroyed();
        }

        internal void load()
        {
            board.setOnBoard(heroShip.shape, heroShip.position);
        }
    }
}
