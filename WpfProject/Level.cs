using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfProject
{
    [Serializable()]
    public class Level
    {
        public int levelNr { get; set; }
        public int opponentsArmour { get; set; }
        public int opponentsSpeed { get; set; }
        private Random rand;
        public Level()
        {
            levelNr = 1;
            rand = new Random();
            setLevelDifficulty();
        }

        public Level(int level)
        {
            levelNr = level;
            setLevelDifficulty();
        }

        public void nextLevel()
        {
            levelNr += 1;
            setLevelDifficulty();
        }

        public Enemy generateEnemy(Canvas board)
        {
            int startingPositionX = rand.Next((int)WpfProject.Game.BOUNDS.LEFT, (int)WpfProject.Game.BOUNDS.RIGHT);
            if (rand.Next(0, 1000) < 10 + levelNr)
            {
                Enemy enemy = new Enemy();
                enemy.position = new Point(startingPositionX, 0);
                enemy.armour = opponentsArmour;
                enemy.speed = opponentsSpeed;
                enemy.shape.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)(255 - ((levelNr * 50) % 255)), 0, 0));
                enemy.setOnBoard(board);
                return enemy;
            }
            return null;
        }

        private void setLevelDifficulty()
        {
            opponentsArmour = levelNr;
            opponentsSpeed = levelNr / 10 + 1;
        }

    }
}
