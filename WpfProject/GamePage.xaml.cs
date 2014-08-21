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
    [Serializable()]
    public partial class GamePage : Page
    {
        private Random rand;
        private GameBoard gameBoard;
        private Player player;
        private Point startingPointHeroShip = new Point(150, 400);
        private int nextSavingScore = 1;

        public GamePage()
        {
            InitializeComponent();
            this.ShowsNavigationUI = false;
            rand = new Random();
            player = new Player();
            gameBoard = new GameBoard(LayoutRoot, startingPointHeroShip);
            ScoreBoard.Content = player.score;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += new EventHandler(timerTick);
            timer.Start();

            Application.Current.MainWindow.KeyDown += new KeyEventHandler(OnButtonKeyDown);
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            gameBoard.OnButtonKeyDown(sender, e);
        }

        private void timerTick(object sender, EventArgs e)
        {
            if (rand.Next(0, 1000) < 10)
            {
                gameBoard.generateEnemy(rand.Next(0, 300));
            }
            gameBoard.moveFlyingObjects();
            int hits = gameBoard.checkBulletsHits();
            player.score += hits;
            ScoreBoard.Content = player.score;
            if (gameBoard.checkCrashes())
            {
                GameOver();
            }
            else if (player.score >= nextSavingScore)
            {
                nextSavingScore += 100; 
                saveGame();
            }
        }

        private void saveGame()
        {
            /*
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(GamePage));

            System.IO.StreamWriter file = new System.IO.StreamWriter(
                "save.xml");
            writer.Serialize(file, gameBoard);
            file.Close();
            MessageBox.Show("Game saved.");
            */
        }

        private void GameOver()
        {
            MessageBox.Show("Ship crashed!", "", MessageBoxButton.OK, MessageBoxImage.Hand);
            //foreach (Window w in 
            //ProjectHome projectHome = new ProjectHome();
            HighScoresPage highScores = new HighScoresPage();
            highScores.updateResults("TODO Nick", player.score); 
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(highScores);
        }
    }
}
