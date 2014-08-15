using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfProject
{
    class GameBoard
    {
        private Canvas canvas;
        private Shape heroShip { get; set; }
        private List<Shape> heroBullets;
        private List<Shape> enemies;

        public GameBoard(Grid grid)
        {
            canvas = new Canvas();
            grid.Children.Add(canvas);
        }

        public void initGameBoard(Shape shipShape) {

            heroShip = shipShape;
            Canvas.SetTop(heroShip, 0);
            Canvas.SetLeft(heroShip, 30);

            canvas.Children.Add(heroShip);
        }
    }
}
