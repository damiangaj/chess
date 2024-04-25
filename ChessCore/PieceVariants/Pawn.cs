using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ChessCore
{
    public class Pawn : Piece
    {
        public override PieceVariant Variant => PieceVariant.Pawn;
        public override Player Colour { get; }

        PieceMovement ahead;




        public Pawn(Player colour) 
        {
            Colour = colour;

            if(colour == Player.White)
            {
                ahead = PieceMovement.up;
            }
            else if (colour == Player.Black) 
            {
                ahead = PieceMovement.down;
            }
        }

        public override Piece Copy()
        {
            Pawn copy = new Pawn(Colour);
            copy.Moved = Moved;
            return copy;
        }

        private static bool PossibleMov(Position pos, Board board)
        {
            return Board.OnBoard(pos) && board.IsFree(pos);
        }

        private bool Capture(Position pos, Board board)
        {
            if(!Board.OnBoard(pos) || board.IsFree(pos))
            {
                return false;
            }
            
            return board[pos].Colour != Colour;
        }

        private static IEnumerable<Move> ReplacingMoves(Position from, Position to)
        {
            yield return new PawnReplacement(from, to, PieceVariant.Knight);
            yield return new PawnReplacement(from, to, PieceVariant.Bishop);
            yield return new PawnReplacement(from, to, PieceVariant.Rook);
            yield return new PawnReplacement(from, to, PieceVariant.Queen);
        }

        private IEnumerable<Move> AheadMoves (Position from ,Board board)
        {
            Position singleMov = from + ahead;

            if (PossibleMov(singleMov, board))
            {
                if(singleMov.X == 0 || singleMov.X == 7)
                {
                    foreach (Move replacementMove in ReplacingMoves(from, singleMov))
                    {
                        yield return replacementMove;
                    }
                }
                else
                {
                    yield return new Default(from, singleMov);
                }

                Position doubleMov = singleMov + ahead;

                if (!Moved && PossibleMov(doubleMov, board))
                {
                    yield return new Double(from, doubleMov);

                }
            }
        }

        private IEnumerable<Move> Diagonal(Position from, Board board)
        {
          
            foreach(PieceMovement mov in new PieceMovement[] { PieceMovement.left, PieceMovement.right })
            {
                Position cap = from + ahead + mov;
                if (cap == board.GetPawnSkip(Colour.Enemy()))
                {
                    yield return new EnPasant(from, cap);
                }
                else if(Capture(cap, board))
                {
                    if (cap.X == 0 || cap.X == 7)
                    {
                        foreach (Move replacementMove in ReplacingMoves(from, cap))
                        {
                            yield return replacementMove;
                        }
                    }
                    else
                    {
                        yield return new Default(from, cap);
                    }
                }
            }  
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return AheadMoves(from, board).Concat(Diagonal(from,board));
        }

        public override bool CanBeatKing(Position from, Board board)
        {
            return Diagonal(from, board).Any(move =>
            {
                Piece piece = board[move.To];
                return piece != null && piece.Variant == PieceVariant.King;
            });
        }
    }
}
