using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public class Bishop : Piece
    {
        public override PieceVariant Variant => PieceVariant.Bishop;
        public override Player Colour { get; }

        private static readonly PieceMovement[] movs = new PieceMovement[]
        {
            PieceMovement.downLeft,
            PieceMovement.downRight,
            PieceMovement.upLeft,
            PieceMovement.upRight

        };

        public Bishop(Player colour)
        {
            Colour = colour;
        }

        public override Piece Copy()
        {
            Bishop copy = new Bishop(Colour);
            copy.Moved = Moved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionIn(from, board, movs).Select(to => new Default(from, to));
        }
    }

    
}
