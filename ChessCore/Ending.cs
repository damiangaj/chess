using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public class Ending
    {
        public Player Winner { get; }
        public PossibleEndings PossibleEndings { get; }

        public Ending(Player winner, PossibleEndings possibleEndings)
        {
            Winner = winner;
            PossibleEndings = possibleEndings;
        }

        public static Ending Win(Player player)
        {
            return new Ending(player, PossibleEndings.Checkmate);
        }

        public static Ending Draw(PossibleEndings possibleEndings)
        {
            return new Ending(Player.None, possibleEndings);
        }

    }
}
