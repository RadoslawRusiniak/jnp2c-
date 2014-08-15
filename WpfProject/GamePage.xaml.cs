using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//DispacherTimer
using System.Windows.Threading;

namespace WpfProject
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        //GameBoard gameBoard = new GameBoard();
        enum MOVINGDIRECTION { LEFT, RIGHT, DOWN, UP, NONE };
        private Point startingPoint = new Point(30, 30);
        private Point currentPosition;
        private MOVINGDIRECTION direction = MOVINGDIRECTION.NONE;
        //private int shipNr = 12345;
        private GameBoard gameBoard;
        private Player player;

        public GamePage()
        {
            InitializeComponent();
            player = new Player();
            gameBoard = new GameBoard(LayoutRoot);
            gameBoard.initGameBoard(player.ship.shipShape);
            /*
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);

            /* Here user can change the speed of the snake.
             * Possible speeds are FAST, MODERATE, SLOW and DAMNSLOW */
            /*
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();

            Application.Current.MainWindow.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            paintShip(startingPoint);
            */
            currentPosition = startingPoint;
        }

        private void paintShip(Point currentposition)
        {
            /* This method is used to paint a frame of the snake´s body
             * each time it is called. */
            /*
            if (GameCanvas.Children.Count >= shipNr)
            {
                GameCanvas.Children.RemoveAt(shipNr);
            }

            Ellipse newEllipse = new Ellipse();
            newEllipse.Fill = Brushes.Aqua;
            newEllipse.Width = 20;
            newEllipse.Height = 20;

            Canvas.SetTop(newEllipse, currentposition.Y);
            Canvas.SetLeft(newEllipse, currentposition.X);

            shipNr = GameCanvas.Children.Add(newEllipse);
             */
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    direction = MOVINGDIRECTION.LEFT;
                    break;
                case Key.Right:
                    direction = MOVINGDIRECTION.RIGHT;
                    break;
                case Key.Up:
                    direction = MOVINGDIRECTION.UP;
                    break;
                case Key.Down:
                    direction = MOVINGDIRECTION.DOWN;
                    break;
            }

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            // Expand the body of the snake to the direction of movement
            switch (direction)
            {
                case MOVINGDIRECTION.LEFT:
                    currentPosition.X -= 5;
                    paintShip(currentPosition);
                    break;
                case MOVINGDIRECTION.RIGHT:
                    currentPosition.X += 5;
                    paintShip(currentPosition);
                    break;
                case MOVINGDIRECTION.UP:
                    currentPosition.Y -= 5;
                    paintShip(currentPosition);
                    break;
                case MOVINGDIRECTION.DOWN:
                    currentPosition.Y += 5;
                    paintShip(currentPosition);
                    break;
            }
            direction = MOVINGDIRECTION.NONE;

            // Restrict to boundaries of the Canvas
            if ((currentPosition.X < 0) || (currentPosition.X > 400) ||
                (currentPosition.Y < 0) || (currentPosition.Y > 500))
                GameOver();
        }

        private void DirectionShow()
        {
            MessageBox.Show("Direction " + direction, "", MessageBoxButton.OK, MessageBoxImage.Hand);
        }

        private void GameOver()
        {
            MessageBox.Show("You Lose! Game Over", "",MessageBoxButton.OK, MessageBoxImage.Hand);
            ProjectHome home = new ProjectHome();
            this.NavigationService.Navigate(home);
        }
    }
}
