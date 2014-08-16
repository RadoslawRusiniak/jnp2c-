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

namespace WpfProject
{
    enum MOVINGDIRECTION { LEFT, RIGHT, DOWN, UP, NONE };

    class GameBoard
    {
        private Canvas canvas;
        private CanvasFlyingObject heroShip;
        private MOVINGDIRECTION heroShipDirection = MOVINGDIRECTION.NONE;
        private List<CanvasFlyingObject> heroBullets;
        private List<CanvasFlyingObject> enemies;

        public GameBoard(Grid grid, Point startingPoint)
        {
            canvas = new Canvas();
            grid.Children.Add(canvas);

            heroShip = new CanvasFlyingObject(startingPoint);
            updateHeroShipPainting();
            heroBullets = new List<CanvasFlyingObject>();
            enemies = new List<CanvasFlyingObject>();
        }

        public void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    heroShipDirection = MOVINGDIRECTION.LEFT;
                    break;
                case Key.Right:
                    heroShipDirection = MOVINGDIRECTION.RIGHT;
                    break;
                case Key.Up:
                    heroShipDirection = MOVINGDIRECTION.UP;
                    break;
                case Key.Down:
                    heroShipDirection = MOVINGDIRECTION.DOWN;
                    break;
                case Key.Space:
                    shootBullet();
                    break;
            }
        }

        public void shootBullet()
        {
            int positionX = (int)heroShip.position.X + (20 - 10) / 2;
            CanvasFlyingObject bullet = new CanvasFlyingObject(new Point(positionX, heroShip.position.Y - 5));
            Ellipse ell = new Ellipse();
            ell.Fill = Brushes.Yellow;
            ell.Width = 10;
            ell.Height = 10;
            bullet.shape = ell;

            Canvas.SetTop(bullet.shape, bullet.position.Y);
            Canvas.SetLeft(bullet.shape, bullet.position.X);

            canvas.Children.Add(bullet.shape);

            heroBullets.Add(bullet);
        }

        public void generateEnemy(int startingPositionX)
        {
            CanvasFlyingObject enemy = new CanvasFlyingObject(new Point(startingPositionX, 0));
            Ellipse ell = new Ellipse();
            ell.Fill = Brushes.Red;
            ell.Width = 20;
            ell.Height = 20;
            enemy.shape = ell;

            Canvas.SetTop(enemy.shape, enemy.position.Y);
            Canvas.SetLeft(enemy.shape, enemy.position.X);

            canvas.Children.Add(enemy.shape);
            enemies.Add(enemy);
        }

        public void moveShip()
        {
            switch (heroShipDirection)
            {
                case MOVINGDIRECTION.LEFT:
                    heroShip.position = new Point(heroShip.position.X - 5, heroShip.position.Y);
                    updateHeroShipPainting();
                    break;
                case MOVINGDIRECTION.RIGHT:
                    heroShip.position = new Point(heroShip.position.X + 5, heroShip.position.Y);
                    updateHeroShipPainting();
                    break;
                case MOVINGDIRECTION.UP:
                    heroShip.position = new Point(heroShip.position.X, heroShip.position.Y - 5);
                    updateHeroShipPainting();
                    break;
                case MOVINGDIRECTION.DOWN:
                    heroShip.position = new Point(heroShip.position.X, heroShip.position.Y + 5);
                    updateHeroShipPainting();
                    break;
            }
            heroShipDirection = MOVINGDIRECTION.NONE;
        }

        public bool checkCrashes()
        {
            if ((heroShip.position.X < 0) || (heroShip.position.X > 300) ||
                (heroShip.position.Y < 0) || (heroShip.position.Y > 420))
            {
                return true;
            }
            foreach (CanvasFlyingObject enemy in enemies)
            {
                if (Math.Pow(enemy.position.X - heroShip.position.X, 2) 
                    + Math.Pow(enemy.position.Y - heroShip.position.Y, 2) <= heroShip.shape.Width * enemy.shape.Width)
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
                    if (Math.Pow(bullet.position.X - enemy.position.X, 2)
                        + Math.Pow(bullet.position.Y - enemy.position.Y, 2) <= bullet.shape.Width * enemy.shape.Width)
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

        private void updateHeroShipPainting() {

            if (heroShip.shape == null)
            {
                Ellipse newEllipse = new Ellipse();
                newEllipse.Fill = Brushes.Aqua;
                newEllipse.Width = 20;
                newEllipse.Height = 20;

                heroShip.shape = newEllipse;
            }
            else
            {
                canvas.Children.Remove(heroShip.shape);
            }

            Canvas.SetTop(heroShip.shape, heroShip.position.Y);
            Canvas.SetLeft(heroShip.shape, heroShip.position.X);

            canvas.Children.Add(heroShip.shape);
        }

        public void updateEnemiesPainting()
        {
            foreach (CanvasFlyingObject enemy in enemies)
            {
                canvas.Children.Remove(enemy.shape);
                enemy.position = new Point(enemy.position.X, enemy.position.Y + 1);
                if (enemy.position.Y < 400)
                {
                    Canvas.SetTop(enemy.shape, enemy.position.Y);
                    canvas.Children.Add(enemy.shape);
                }
            }
        }

        public void updateBulletsPainting()
        {
            foreach (CanvasFlyingObject bullet in heroBullets)
            {
                canvas.Children.Remove(bullet.shape);
                bullet.position = new Point(bullet.position.X, bullet.position.Y - 1);
                if (bullet.position.Y > 0)
                {
                    Canvas.SetTop(bullet.shape, bullet.position.Y);
                    canvas.Children.Add(bullet.shape);
                }
            }
        }
    }
}
