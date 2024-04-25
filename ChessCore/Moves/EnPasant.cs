using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public class EnPasant : Move
    {
        public override Moves Type => Moves.Passante;
        public override Position From { get; }
        public override Position To { get; }

        public readonly Position pawnCaptured;


        public EnPasant(Position from, Position to)
        {
            From = from;
            To = to;
            pawnCaptured = new Position(from.X, to.Y);

        }
        public override bool Execute(Board board)
        {


            new Default(From, To).Execute(board);
            board[pawnCaptured] = null;
            return true;
        }

    }
}
