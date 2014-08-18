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
    /// Interaction logic for HighScoresPage.xaml
    /// </summary>
    public partial class HighScoresPage : Page
    {
        private const string HIGHSCORES_FILE_NAME = "HighScores.txt";

        public HighScoresPage()
        {
            InitializeComponent();
            if (!File.Exists(FILE_NAME))
            {
                File.Create(FILE_NAME);
            }
        }
    }
}
