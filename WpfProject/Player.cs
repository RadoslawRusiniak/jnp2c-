using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfProject
{
    class Player
    {
        public Ship ship { get; set; }
        public int score { get; set; }

        public Player()
        {
            ship = new Ship();
            score = 0;
        }
    }
}
