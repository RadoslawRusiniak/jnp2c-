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
    /// Interaction logic for HighScoresPage.xaml
    /// </summary>

    struct Result
    {
        public String playerNick { get; set; }
        public int score { get; set; }

    }

    public partial class HighScoresPage : Page
    {
        private const string HIGHSCORES_FILE_NAME = "HighScores.txt";
        private const int SCORES_TO_SHOW = 10;
        private char SPECIAL_CHAR = ';';

        private Result[] bestResults = new Result[SCORES_TO_SHOW + 5];

        public HighScoresPage()
        {
            InitializeComponent();
            initHighScoresOnStart();
            renderScores();
        }

        public void updateResults(String nick, int score)
        {
            int position_to_put = SCORES_TO_SHOW;
            for (int i = 0; i < SCORES_TO_SHOW; ++i)
            {
                if (score > bestResults[i].score)
                {
                    position_to_put = i;
                    break;
                }
            }

            if (position_to_put < SCORES_TO_SHOW)
            {
                for (int i = position_to_put + 1; i < SCORES_TO_SHOW; ++i)
                {
                    bestResults[i] = bestResults[i - 1];
                }
                bestResults[position_to_put].playerNick = nick;
                bestResults[position_to_put].score = score;

                writeDataToFile();
                renderScores();
            }

        }

        private void initHighScoresOnStart()
        {
            if (File.Exists(HIGHSCORES_FILE_NAME))
            {
                loadDataFromFile();
            }
            else
            {
                FileStream fs = File.Create(HIGHSCORES_FILE_NAME);
                fs.Close();
                initBestResults();
                writeDataToFile();
            }
        }

        private void initBestResults()
        {
            for (int i = 0, j = 10 * SCORES_TO_SHOW; i < SCORES_TO_SHOW; ++i, j -= 10)
            {
                bestResults[i].playerNick = "User" + Convert.ToString(i + 1);
                bestResults[i].score = j;
            }
        }

        private void writeDataToFile()
        {
            using (StreamWriter outfile = new StreamWriter(HIGHSCORES_FILE_NAME))
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < SCORES_TO_SHOW; ++i)
                {
                    sb.AppendLine(String.Format("{0}{1}{2}", bestResults[i].playerNick, SPECIAL_CHAR, bestResults[i].score));
                }
                outfile.Write(sb.ToString());
            }
        }

        private void loadDataFromFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(HIGHSCORES_FILE_NAME))
                {
                    String line;
                    int i = 0;
                    while (i < SCORES_TO_SHOW && (line = sr.ReadLine()) != null)
                    {
                        bestResults[i].playerNick = line.Split(SPECIAL_CHAR)[0];
                        bestResults[i].score = Convert.ToInt32(line.Split(SPECIAL_CHAR)[1]);
                        ++i;
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                initBestResults();
            }
        }

        private void renderScores()
        {
            this.ScoresGrid.ColumnDefinitions.Clear();
            this.ScoresGrid.RowDefinitions.Clear();

            this.ScoresGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            Label highScores = new Label();
            highScores.Content = "Highest scores"; 
            highScores.HorizontalAlignment = HorizontalAlignment.Center;
            this.ScoresGrid.Children.Add(highScores);
            Grid.SetRow(highScores, 0);

            this.ScoresGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            Grid resultsGrid = new Grid();
            for (int i = 0; i <= 1; ++i)
            {
                resultsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < SCORES_TO_SHOW; ++i)
            {
                resultsGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                Label nameLabel = new Label(); nameLabel.Content = bestResults[i].playerNick;
                Label dataLabel = new Label(); dataLabel.Content = bestResults[i].score.ToString();
                nameLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                dataLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                Grid.SetRow(nameLabel, i);
                Grid.SetRow(dataLabel, i);
                Grid.SetColumn(nameLabel, 0);
                Grid.SetColumn(dataLabel, 1);

                resultsGrid.Children.Add(nameLabel);
                resultsGrid.Children.Add(dataLabel);
            }
            this.ScoresGrid.Children.Add(resultsGrid);
            Grid.SetRow(resultsGrid, 1);
            
            this.ScoresGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
            Button btn = new Button();
            btn.Width = 120;
            btn.Content = "Back to main page";
            btn.Click += toMain_Click;

            this.ScoresGrid.Children.Add(btn);
            Grid.SetRow(btn, 2);
        }

        private void toMain_Click(object sender, RoutedEventArgs e)
        {
            ProjectHome projectHome = new ProjectHome();
            this.NavigationService.Navigate(projectHome);
        }
        
    }
}
