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
        [XMLIgnore]
        private Random rand;
        public GameBoard gameBoard { get; set; }
        public Player player { get; set; }
        public int nextSavingScore = 1;
        [XMLIgnore]
        private DispatcherTimer timer;

        public GamePage()
        {
            InitializeComponent();
            this.ShowsNavigationUI = false;
            rand = new Random();
            player = new Player();
            Point heroShipStartingPoint = new Point(150, 400);
            gameBoard = new GameBoard(LayoutRoot, heroShipStartingPoint);
            ScoreBoard.Content = player.score;

            Application.Current.MainWindow.KeyDown += new KeyEventHandler(OnButtonKeyDown);

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += new EventHandler(timerTick);
            timer.Start();
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
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(GameBoard));

            System.IO.StreamWriter file = new System.IO.StreamWriter(
                "saveBoard.xml");
            writer.Serialize(file, gameBoard);

            writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Player));
            file = new System.IO.StreamWriter(
                "savePlayer.xml");
            writer.Serialize(file, player);
            //gameBoard.save();

            file.Close();
            MessageBox.Show("Game saved.");
            
        }

        private void GameOver()
        {
            //TODO przerobic na mutex, zeby na pewno dzialalo
            timer.Stop();
            MessageBox.Show("Ship crashed!", "", MessageBoxButton.OK, MessageBoxImage.Hand);
            
            HighScoresPage highScores = new HighScoresPage();
            highScores.updateResults("TODO Nick", player.score);

            ProjectHome projectHome = new ProjectHome();
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(projectHome);
        }
    }
}
