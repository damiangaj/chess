

namespace ChessCore
{
    public class PawnReplacement : Move
    {
        public override Moves Type => Moves.Replacement;
        public override Position From { get; }
        public override Position To { get; }
        private readonly PieceVariant newReplacement;

        public PawnReplacement(Position from, Position to, PieceVariant newReplacement)
        {
            From = from;
            To = to;
            this.newReplacement = newReplacement;
        }

        private Piece MakeNewPiece(Player player)
        {
            Piece newPiece;
            if(newReplacement == PieceVariant.Knight) { newPiece = new Knight(player); }
            else if(newReplacement == PieceVariant.Rook) { newPiece = new Rook(player); }
            else if (newReplacement == PieceVariant.Bishop) { newPiece = new Bishop(player); }
            else { newPiece = new Queen(player); }
            return newPiece;
        }

        public override bool Execute(Board board)
        {
            Piece pawn = board[From];
            board[From] = null;
            Piece newPiece = MakeNewPiece(pawn.Colour);
            newPiece.Moved = true;
            board[To] = newPiece;
            return true;
        }
    }
}
