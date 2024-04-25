using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public class Default : Move
    {
        public override Moves Type => Moves.Default;
        public override Position From { get; }
        public override Position To { get; }

        public Default(Position from, Position to)
        {
            From = from;
            To = to;
        }
        public override bool Execute(Board board)
        {
            Piece piece = board[From];
            bool cap = !board.IsFree(To);
            board[To] = piece;
            board[From] = null;
            piece.Moved = true;

            return cap || piece.Variant == PieceVariant.Pawn;
        }
    }
}
