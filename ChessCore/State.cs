using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public class State
    {
        private readonly StringBuilder sb = new StringBuilder();

        public State(Player currentPlayer,Board board)
        {
            AddPieceP(board);
            sb.Append(' ');
            AddCurrentPlayer(currentPlayer);
            sb.Append(' ');
            AddCasteling(board);
            sb.Append(' ');
            AddEnPassante(board, currentPlayer);

        }

        public override string ToString()
        {
           return sb.ToString();
        }

        private static char PieceChar(Piece piece)
        {
            char c = piece.Variant switch
            {
                PieceVariant.Pawn => 'p',
                PieceVariant.Queen => 'q',
                PieceVariant.Knight => 'n',
                PieceVariant.Bishop => 'b',
                PieceVariant.Rook => 'r',
                PieceVariant.King => 'k',
                _=>' '
    
            };

            if(piece.Colour == Player.White)
            {
                return char.ToUpper(c);
            }
            
                return c;
            
        }
        private void AddRowData(Board board, int x) 
        {
            int empty = 0;

            for(int i = 0;i < 8; i++)
            {
                if (board[x, i] == null)
                {
                    empty++;
                    continue;
                }

                if(empty > 0)
                {
                    sb.Append(empty);
                    empty = 0;
                }
                sb.Append(PieceChar(board[x,i]));
            }

            if (empty > 0)
            {
                sb.Append(empty);
                empty = 0;
            }
        }

        private void AddPieceP(Board board)
        {
            for(int i = 0; i < 8; i++) 
            {   
                if(i != 0)
                {
                    sb.Append('/');
                }
                AddRowData(board, i);
            }

        }

        private void AddCurrentPlayer(Player player)
        {
            if(player == Player.White)
            {
                sb.Append('w');
            }
            else
            {
                sb.Append('b');
            }
        }

        private void AddCasteling(Board board)
        {
            bool castleWKS = board.CastleRightKS(Player.White);
            bool castleWQS = board.CastleRightQS(Player.White);

            bool castleBKS = board.CastleRightKS(Player.Black);
            bool castleBQS = board.CastleRightQS(Player.Black);

            if(!(castleWKS || castleWQS || castleBKS || castleBQS))
            {
                sb.Append('-');
            }

            if (castleWKS)
            {
                sb.Append('K');
            }

            if (castleWKS)
            {
                sb.Append('Q');
            }


            if (castleBKS)
            {
                sb.Append('k');
            }

            if (castleBKS)
            {
                sb.Append('q');
            }

        }

        private void AddEnPassante(Board board, Player player)
        {
            if (!board.CanEnPassant(player))
            {
                sb.Append('-');
                return;
            }

            Position position = board.GetPawnSkip(player.Enemy());
            char file = (char)('a' + position.Y);
            int rank = 8 - position.X;
            sb.Append(file);
            sb.Append(rank);
        }
    }

}
