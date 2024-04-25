using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public abstract class Move
    {
        public abstract Moves Type { get; }
        public abstract Position From { get; }
        public abstract Position To { get; }
        public abstract bool Execute(Board board);

        public virtual bool IsPossible(Board board)
        {
            Player player = board[From].Colour;
            Board checkingBoard = board.Copy();
            Execute(checkingBoard);
            return !checkingBoard.IsChecked(player);
        }
    }
}
