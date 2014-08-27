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
        public const String SAVE_FILE_NAME = "save2.xml";

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

        private void ContinueGameClick(object sender, RoutedEventArgs e)
        {
            if (File.Exists(SAVE_FILE_NAME))
            {
                GamePage gamePage = new GamePage();
                this.NavigationService.Navigate(gamePage);
                gamePage.LoadGame();
            }
            else
            {
                MessageBox.Show("Save not available. Click \"Play\" to start new game.");
            }
        }
    }
}
