using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfProject
{
    /// <summary>
    /// Interaction logic for ProjectHome.xaml
    /// </summary>
    public partial class ProjectHome : Page
    {
        public const String SAVE_FILE_NAME = "save.xml";

        public ProjectHome()
        {
            InitializeComponent();
            //this.NavigationService.RemoveBackEntry();
            this.ShowsNavigationUI = true;
        }

        private void PlayClick(object sender, RoutedEventArgs e)
        {
            GamePage gamePage = new GamePage();
            gamePage.runTimer();
            this.NavigationService.Navigate(gamePage);
        }

        private void HighScoresClick(object sender, RoutedEventArgs e)
        {
            HighScoresPage highScoresPage = new HighScoresPage();
            this.NavigationService.Navigate(highScoresPage);
        }

        private void ContinueGame(object sender, RoutedEventArgs e)
        {
            if (File.Exists(SAVE_FILE_NAME))
            {
                GamePage gamePage = new GamePage();

                System.Xml.Serialization.XmlSerializer reader =
                    new System.Xml.Serialization.XmlSerializer(typeof(Game));
                System.IO.StreamReader file = new System.IO.StreamReader(
                    SAVE_FILE_NAME);
                gamePage.game = (Game)reader.Deserialize(file);
                foreach (FlyingObject enemy in gamePage.game.enemies)
                {
                }
                foreach (FlyingObject bullet in gamePage.game.heroBullets)
                {
                }

                gamePage.runTimer();
                this.NavigationService.Navigate(gamePage);
            }
            else
            {
                MessageBox.Show("Save not available. Click \"Play\" to start new game.");
            }
        }
    }
}
