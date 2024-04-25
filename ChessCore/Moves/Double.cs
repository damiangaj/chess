using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public class Double : Move
    {
        public override Moves Type => Moves.PawnDouble;
        public override Position From { get; }
        public override Position To { get; }

        private readonly Position skip;

        public Double(Position from, Position to)
        {
            From = from;
            To = to;
            skip = new Position((from.X + to.X) / 2, from.Y);
        }

        public override bool Execute(Board board)
        {
            Player player = board[From].Colour;
            board.SetSkip(player, skip);
            new Default(From, To).Execute(board);
            return true;
        }
    }
}
