namespace ChessCore
{
    public class King : Piece
    {
        public override PieceVariant Variant => PieceVariant.King;
        public override Player Colour { get; }

        public King(Player colour)
        {
            Colour = colour;
        }

        private static bool RookNotMoved(Position position, Board board)
        {
            if (board.IsFree(position))
            {
                return false;
            }
            Piece piece = board[position];
            return piece.Variant == PieceVariant.Rook && !piece.Moved;
        }

        private static bool IsEmpty(IEnumerable<Position> positions, Board board)
        {
            return positions.All(position => board.IsFree(position));
        }

        private bool CanCastleK(Position position, Board board)
        {
            if (Moved)
            {
                return false;
            }

            Position rookPosition = new Position(position.X, 7);
            Position[] middlePositions = new Position[]
            {
                new(position.X, 5), new(position.X, 6) 
            }; 

            return RookNotMoved(rookPosition, board) && IsEmpty(middlePositions, board);
        }


        private bool CanCastleQ(Position position, Board board)
        {
            if(Moved)
            {
                return false;
            }

            Position rookPosition = new Position(position.X, 0);
            Position[] middlePositions = new Position[]
            {
                new(position.X, 1), new(position.X, 2), new(position.X, 3)
            };

            return RookNotMoved(rookPosition, board) && IsEmpty(middlePositions, board);
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
            King copy = new King(Colour);
            copy.Moved = Moved;
            return copy;
        }

        private IEnumerable<Position> MovePosition(Position from, Board board)
        {
           foreach(PieceMovement mov in movs)
            {
                Position to = from + mov;
                if (!Board.OnBoard(to))
                {
                    continue;
                }

                if(board.IsFree(to) || board[to].Colour != Colour)
                {
                    yield return to;
                }
            }
          
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            foreach(Position to in MovePosition(from,board))
            {
                yield return new Default(from, to);
            }

            if (CanCastleK(from, board))
            {
                yield return new Castling(Moves.CastleK, from);
            }

            if (CanCastleQ(from, board))
            {
                yield return new Castling(Moves.CastleQ, from);
            }
        }

        public override bool CanBeatKing(Position from, Board board)
        {
            return MovePosition(from, board).Any(to =>
            {
                Piece piece = board[to];
                return piece != null && piece.Variant == PieceVariant.King;
            });
        }
    }
}