using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace WpfProject
{
    [Serializable()]
    public class Level
    {
        public int levelNr { get; set; }
        public int nextLevelPoints { get; set; }
        public int opponentsArmour { get; set; }
        public int opponentsSpeed { get; set; }
        [XmlIgnore]
        private Random rand;
        public Level()
        {
            levelNr = 1;
            rand = new Random();
            setLevelDetails();
        }

        public Level(int level)
        {
            levelNr = level;
            setLevelDetails();
        }

        internal void updateLevel(int score)
        {
            if (score >= nextLevelPoints)
            {
                nextLevel();
            }
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
        
        private void nextLevel()
        {
            levelNr += 1;
            setLevelDetails();
        }

        private void setLevelDetails()
        {
            nextLevelPoints = levelNr * 10;
            opponentsArmour = levelNr;
            opponentsSpeed = levelNr/10 + 1;
        }
    }
}
