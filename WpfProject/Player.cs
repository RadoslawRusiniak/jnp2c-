using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfProject
{
    [Serializable()]
    public class Player
    {
        public int score { get; set; }

        public Player()
        {
            score = 0;
        }
    }
}
