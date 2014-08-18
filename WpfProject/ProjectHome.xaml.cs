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

namespace WpfProject
{
    /// <summary>
    /// Interaction logic for ProjectHome.xaml
    /// </summary>
    public partial class ProjectHome : Page
    {
        GamePage gamePage;
        HighScoresPage highScoresPage;

        public ProjectHome()
        {
            InitializeComponent();
        }

        private void PlayClick(object sender, RoutedEventArgs e)
        {
            gamePage = new GamePage();
            this.NavigationService.Navigate(gamePage);
        }

        private void HighScoresClick(object sender, RoutedEventArgs e)
        {
            highScoresPage = new HighScoresPage();
            this.NavigationService.Navigate(highScoresPage);
        }
    }
}
