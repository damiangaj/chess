using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public class Position
    {
        public int X { get;  }
        public int Y { get;  }

        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Player PositionColour()
        {
            if((X+Y)%2 == 0)
            {
                return Player.White;
            }
            else { return Player.Black;}
        }

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   X == position.X &&
                   Y == position.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Position left, Position right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        public static Position operator +(Position pos, PieceMovement mov)
        {
            return new Position(pos.X + mov.Xchange, pos.Y + mov.Ychange);
        }
    }
}
