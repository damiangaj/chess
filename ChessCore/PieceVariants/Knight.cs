using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public class Knight : Piece
    {
        public override PieceVariant Variant => PieceVariant.Knight;
        public override Player Colour { get; }
        public Knight(Player colour)
        {
            Colour = colour;
        }

        public override Piece Copy()
        {
            Knight copy = new Knight(Colour);
            copy.Moved = Moved;
            return copy;
        }

        private static IEnumerable<Position> Potential(Position from)
        {
            foreach (PieceMovement vmov in new PieceMovement[] {PieceMovement.up, PieceMovement.down}) 
            {
                foreach(PieceMovement hmov in new PieceMovement[] {PieceMovement.left, PieceMovement.right, })
                {
                    yield return from + 2 * vmov + hmov;
                    yield return from + 2 * hmov + vmov;
                }
            }
        }

        private IEnumerable<Position> NewPosition(Position from, Board board) 
        {
            return Potential(from).Where(pos => Board.OnBoard(pos) && (board.IsFree(pos) || board[pos].Colour !=Colour));
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return NewPosition(from, board).Select(to => new Default(from,to));
        }
    }
}