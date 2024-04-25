using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public class Rook : Piece
    {
        public override PieceVariant Variant => PieceVariant.Rook;
        public override Player Colour { get; }


        private static readonly PieceMovement[] movs = new PieceMovement[]
    {
            PieceMovement.down,
            PieceMovement.up,
            PieceMovement.left,
            PieceMovement.right

    };
        public Rook(Player colour)
        {
            Colour = colour;
        }

        public override Piece Copy()
        {
            Rook copy = new Rook(Colour);
            copy.Moved = Moved;
            return copy;
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionIn(from, board, movs).Select(to => new Default(from, to));
        }

    }
}