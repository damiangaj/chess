using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public enum Player
    {
        None,
        White,
        Black
    }

    public static class PlayerAdditions
    {
        public static Player Enemy(this Player player)
        {
            switch (player)
            {
                case Player.White:return Player.Black;
                case Player.Black: return Player.White;
                default: return Player.None;
            }
        }
    }
}
