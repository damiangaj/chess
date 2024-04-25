using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public abstract class Piece
    {
        public abstract PieceVariant Variant { get; }
        public abstract Player Colour { get; }
        public bool Moved { get; set; } = false;
        public abstract Piece Copy();



        public abstract IEnumerable<Move> GetMoves(Position from, Board board);

        protected  IEnumerable<Position> MovePositions(Position from,Board board,PieceMovement mov)
        {
            for(Position pos = from + mov; Board.OnBoard(pos);pos += mov)
            {
                if(board.IsFree(pos))
                {
                    yield return pos;
                    continue;
                   
                }
                Piece piece = board[pos];
                if(piece.Colour != Colour)
                {
                    yield return pos;
                }
                yield break;
            }
        }

        protected IEnumerable<Position> MovePositionIn(Position from, Board board, PieceMovement[] movs)
        {
            return movs.SelectMany(mov  => MovePositions(from, board, mov));
        }

        public virtual bool CanBeatKing(Position from, Board board)
        {
            return GetMoves(from, board).Any(move =>
            {
                Piece piece = board[move.To];
                return piece != null && piece.Variant == PieceVariant.King;
            });
        }

    }
}
