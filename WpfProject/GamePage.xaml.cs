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
        private Label armourLabel;
        private Label levelLabel;
        public Game game { get; set; }
        public int nextCheckpointScore { get; set; }
        private DispatcherTimer timer;

        public GamePage()
        {
            InitializeComponent();
            this.ShowsNavigationUI = false;
            game = new Game();

            scoreBoard = new Label();
            armourLabel = new Label();
            levelLabel = new Label();

            setGrid();

            nextCheckpointScore = 1;
            
            Application.Current.MainWindow.KeyDown += new KeyEventHandler(OnButtonKeyDown);
        }

        private void setGrid()
        {
            this.playGrid.ColumnDefinitions.Add(new ColumnDefinition());
            this.playGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            this.playGrid.RowDefinitions.Add(new RowDefinition());

            Grid infoPanel = new Grid();
            infoPanel.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < 3; ++i)
            {
                infoPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });
            }
            this.playGrid.Children.Add(infoPanel);
            Grid.SetRow(infoPanel, 0);
            Grid.SetColumn(infoPanel, 0);

            infoPanel.Children.Add(scoreBoard);
            Grid.SetRow(scoreBoard, 0);
            Grid.SetColumn(scoreBoard, 0);

            infoPanel.Children.Add(armourLabel);
            Grid.SetRow(armourLabel, 0);
            Grid.SetColumn(armourLabel, 1);

            infoPanel.Children.Add(levelLabel);
            Grid.SetRow(levelLabel, 0);
            Grid.SetColumn(levelLabel, 2);

            this.playGrid.Children.Add(game.board);
            Grid.SetRow(game.board, 1);
            Grid.SetColumn(game.board, 0);
            //Grid.SetZIndex(gameBoard.canvas, 99);
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
            updateInfoPanel();
            if (game.isGameOver())
            {
                GameOver();
            }
            else 
            {
                if (game.player.score >= nextCheckpointScore)
                {
                    nextCheckpointScore += 100;
                    saveGame();
                }
            }
        }

        private void updateInfoPanel()
        {
            scoreBoard.Content = "Score: " + game.player.score;
            armourLabel.Content = "Armour: " + game.heroShip.armour;
            levelLabel.Content = "Level: " + game.level.levelNr;
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
            timer.Stop();
            MessageBox.Show("Ship crashed! Game Over!", "", MessageBoxButton.OK, MessageBoxImage.Hand);
            
            HighScoresPage highScores = new HighScoresPage();
            highScores.updateResults("TODO Nick", game.player.score);

            ProjectHome projectHome = new ProjectHome();
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(projectHome);
        }

        internal void LoadGame()
        {
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(Game));
            using (System.IO.StreamReader file = new System.IO.StreamReader(
                WpfProject.ProjectHome.SAVE_FILE_NAME))
            {
                game = (Game)reader.Deserialize(file);
                game.load();
            }

            nextCheckpointScore += 100;

            runTimer();
        }
    }
}
