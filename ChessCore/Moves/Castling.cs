using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChessCore
{
    public class Castling : Move
    {
         
       

    
        public override Moves Type { get; }
        public override Position From { get; }
        public override Position To { get; }

        private readonly PieceMovement kingMovement;
        private readonly Position rookFrom;
        private readonly Position rookTo;

        public Castling(Moves type, Position kingPosition)
        {
            Type = type;
            From = kingPosition;

            if (type == Moves.CastleK)
            {
                kingMovement = PieceMovement.right;
                To = new Position(kingPosition.X, 6);
                rookFrom = new Position(kingPosition.X, 7);
                rookTo = new Position(kingPosition.X, 5);
            }
            else if(type == Moves.CastleQ)
            {
                kingMovement = PieceMovement.left;
                To = new Position(kingPosition.X, 2);
                rookFrom = new Position(kingPosition.X, 0);
                rookTo = new Position(kingPosition.X, 3);
            }
        }

        public override bool Execute(Board board)
        {
            new Default(From, To).Execute(board);
            new Default(rookFrom, rookTo).Execute(board);
            return false;
        }

        public override bool IsPossible(Board board)
        {
            Player player = board[From].Colour;

            if (board.IsChecked(player))
            {
                return false;
            }

            Board copy = board.Copy();
            Position kingPositionCopy = From;

            for(int i = 0; i < 2; i++)
            {
                new Default(kingPositionCopy, kingPositionCopy + kingMovement).Execute(copy);
                kingPositionCopy += kingMovement;

                if (copy.IsChecked(player))
                {
                    return false;
                }
            }
            return true;
        }
        
    }
}
