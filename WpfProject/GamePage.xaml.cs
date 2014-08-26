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
        private Label scoreBoard;
        public Game game { get; set; }
        public int nextSavingScore { get; set; }
        private DispatcherTimer timer;

        public GamePage()
        {
            InitializeComponent();
            this.ShowsNavigationUI = false;

            this.playGrid.ColumnDefinitions.Add(new ColumnDefinition());

            scoreBoard = new Label();
            scoreBoard.Content = "Score: 0";
            this.playGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            this.playGrid.Children.Add(scoreBoard);
            Grid.SetRow(scoreBoard, 0);
            Grid.SetColumn(scoreBoard, 0);

            game = new Game();
            this.playGrid.RowDefinitions.Add(new RowDefinition());
            this.playGrid.Children.Add(game.board);
            Grid.SetRow(game.board, 1);
            Grid.SetColumn(game.board, 0);
            //Grid.SetZIndex(gameBoard.canvas, 99);

            nextSavingScore = 1;
            
            Application.Current.MainWindow.KeyDown += new KeyEventHandler(OnButtonKeyDown);
        }

        internal void runTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += new EventHandler(timerTick);
            timer.Start();
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            game.OnButtonKeyDown(sender, e);
        }

        private void timerTick(object sender, EventArgs e)
        {
            game.updateGame();
            if (game.isGameOver())
            {
                GameOver();
            }
            else 
            {
                scoreBoard.Content = "Score: " + game.player.score;
                if (game.player.score >= nextSavingScore)
                {
                    nextSavingScore += 100;
                    saveGame();
                }
            }
        }

        private void saveGame()
        {   
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Game));

            using (System.IO.StreamWriter file 
                = new System.IO.StreamWriter(WpfProject.ProjectHome.SAVE_FILE_NAME))
            {
                writer.Serialize(file, game);
            }
            
            MessageBox.Show("Game saved.");
            
        }

        private void GameOver()
        {
            //TODO przerobic na mutex, zeby na pewno dzialalo
            timer.Stop();
            MessageBox.Show("Ship crashed!", "", MessageBoxButton.OK, MessageBoxImage.Hand);
            
            HighScoresPage highScores = new HighScoresPage();
            highScores.updateResults("TODO Nick", game.player.score);

            ProjectHome projectHome = new ProjectHome();
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(projectHome);
        }
    }
}
