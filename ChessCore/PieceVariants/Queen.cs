using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public class Queen : Piece
    {
        public override PieceVariant Variant => PieceVariant.Queen;
        public override Player Colour { get; }
        public Queen(Player colour)
        {
            Colour = colour;
        }
        private static readonly PieceMovement[] movs = new PieceMovement[]
        {
            PieceMovement.down,
            PieceMovement.up,
            PieceMovement.left,
            PieceMovement.right,
            PieceMovement.downLeft,
            PieceMovement.downRight,
            PieceMovement.upLeft,
            PieceMovement.upRight

        };
        public override Piece Copy()
        {
            Queen copy = new Queen(Colour);
            copy.Moved = Moved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionIn(from, board, movs).Select(to => new Default(from, to));
        }

    }
}