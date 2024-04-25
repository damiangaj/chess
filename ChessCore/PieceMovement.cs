using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public class PieceMovement
    {
        public int Xchange { get; }
        public int Ychange { get; }

        public readonly static PieceMovement up = new PieceMovement(-1, 0);
        public readonly static PieceMovement down = new PieceMovement (1, 0);
        public readonly static PieceMovement left = new PieceMovement(0, -1);
        public readonly static PieceMovement right = new PieceMovement(0, 1);
        
        public readonly static PieceMovement upLeft = new PieceMovement(-1, -1);
        public readonly static PieceMovement upRight = new PieceMovement (-1, 1);

        public readonly static PieceMovement downLeft = new PieceMovement(1, -1);
        public readonly static PieceMovement downRight = new PieceMovement(1, 1);

        // ewentualnie jakby nie dzialalo:
        // upDown = up + down z dzialajacym przeciazeniem

        public PieceMovement(int xChange, int yChange)
        {
            this.Xchange = xChange;
            this.Ychange = yChange;
        }
        /*public static PieceMovement operator +(PieceMovement mov1,  PieceMovement mov2)
        {
            return new PieceMovement(mov1.Xchange + mov2.Xchange, mov1.Ychange + mov2.Ychange);
        }*/

        public static PieceMovement operator *(int multi, PieceMovement mov)
        {
            return new PieceMovement(multi * mov.Xchange, multi * mov.Ychange);
        }
    }
}
