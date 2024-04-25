using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];
        private readonly Dictionary<Player, Position> pawnSkip = new Dictionary<Player, Position>() {
            {Player.White,null },
            {Player.Black,null },

        };
        public Piece this[int x, int y]
        {
            get { return pieces[x, y]; }
            set { pieces[x,y] = value; }
        }

        public Piece this[Position pos]
        {
            get { return this[pos.X, pos.Y]; }
            set { this[pos.X, pos.Y] = value; }
        }
        public Position GetPawnSkip(Player player)
        {
            return pawnSkip[player];
        }

        public void SetSkip(Player player, Position skip)
        {
            pawnSkip[player] = skip;
        }

        public static Board Start()
        {
            Board board = new Board();
            board.StartPositions();
            return board;
        }

        private void StartPositions()
        {
            this[0, 0] = new Rook(Player.Black);
            this[0, 1] = new Knight(Player.Black);
            this[0, 2] = new Bishop(Player.Black);
           this[0, 3] = new Queen(Player.Black);
            this[0, 4] = new King(Player.Black);
            this[0, 5] = new Bishop(Player.Black);
            this[0, 6] = new Knight(Player.Black);
            this[0, 7] = new Rook(Player.Black);

            this[1, 0] = new Pawn(Player.Black);
            this[1, 1] = new Pawn(Player.Black);
            this[1, 2] = new Pawn(Player.Black);
            this[1, 3] = new Pawn(Player.Black);
            this[1, 4] = new Pawn(Player.Black);
            this[1, 5] = new Pawn(Player.Black);
            this[1, 6] = new Pawn(Player.Black);
            this[1, 7] = new Pawn(Player.Black);

            this[7, 0] = new Rook(Player.White);
            this[7, 1] = new Knight(Player.White);
            this[7, 2] = new Bishop(Player.White);
            this[7, 3] = new Queen(Player.White);
            this[7, 4] = new King(Player.White);
            this[7, 5] = new Bishop(Player.White);
            this[7, 6] = new Knight(Player.White);
            this[7, 7] = new Rook(Player.White);

            this[6, 0] = new Pawn(Player.White);
            this[6, 1] = new Pawn(Player.White);
            this[6, 2] = new Pawn(Player.White);
            this[6, 3] = new Pawn(Player.White);
            this[6, 4] = new Pawn(Player.White);
            this[6, 5] = new Pawn(Player.White);
            this[6, 6] = new Pawn(Player.White);
            this[6, 7] = new Pawn(Player.White);


        }

        public static bool OnBoard(Position pos)
        {
            return pos.X >= 0 && pos.X <= 7 && pos.Y >= 0 && pos.Y <= 7;
        }

        public bool IsFree(Position pos) 
        {
            return this[pos] == null;
        }

        public IEnumerable<Position> PiecePosition()
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    Position position = new Position(i, j);
                    if(!IsFree(position)) yield return position;
                }
            }
        }

        public IEnumerable<Position> PiecePositionByColour(Player player)
        {
            return PiecePosition().Where(pos => this[pos].Colour == player);
        }

        public bool IsChecked(Player player)
        {
            return PiecePositionByColour(player.Enemy()).Any(position =>
            {
                Piece piece = this[position];
                return piece.CanBeatKing(position, this);
            });
        }

        public Board Copy()
        {
            Board board = new Board();

            foreach(Position pos in PiecePosition()) 
            {
                board[pos] = this[pos].Copy();
            }
            return board;
        }

        public CountPieces Counter()
        {
            CountPieces counter = new CountPieces();
            foreach(Position position in PiecePosition())
            {
                Piece piece = this[position];
                counter.Add(piece.Colour, piece.Variant);
            }
            return counter;
        }

        public bool InsufficientMaterial()
        {
            CountPieces counter = Counter();

            return KingVsKing(counter) || KingAndBishopVsKing(counter) || KingAndKnightVsKing(counter) || KingAndBishopVsKingBishop(counter);
        }

        private static bool KingVsKing(CountPieces counter)
        {
            return counter.Counter == 2;
        }

        private static bool KingAndBishopVsKing(CountPieces counter)
        {
            return counter.Counter == 3 && (counter.NumberOfWhite(PieceVariant.Bishop) == 1 || counter.NumberOfBlack(PieceVariant.Bishop) == 1);
        }

        private static bool KingAndKnightVsKing(CountPieces counter)
        {
            return counter.Counter == 3 && (counter.NumberOfWhite(PieceVariant.Knight) == 1 || counter.NumberOfBlack(PieceVariant.Knight) == 1);
        }

    

        private bool KingAndBishopVsKingBishop(CountPieces counter)
        {
            if (counter.Counter != 4) return false;
            if (counter.NumberOfWhite(PieceVariant.Bishop) != 1 || counter.NumberOfBlack(PieceVariant.Bishop) != 1) return false;
            Position whiteBishopPosition = PiecePosition(Player.White, PieceVariant.Bishop);
            Position blackBishopPosition = PiecePosition(Player.Black, PieceVariant.Bishop);
            return whiteBishopPosition.PositionColour() == blackBishopPosition.PositionColour();
        }

        private Position PiecePosition(Player player, PieceVariant variant)
        {
            return PiecePositionByColour(player).First(position => this[position].Variant == variant);
        }

        private bool IsUnmovedKingAndRook(Position kingP, Position rookP)
        {
            if(IsFree(kingP) || IsFree(rookP))
            {
                return false;
            }
            Piece king = this[kingP];
            Piece rook = this[rookP];

            return king.Variant == PieceVariant.King && rook.Variant == PieceVariant.Rook &&
               !king.Moved && !rook.Moved;
        }

        public bool CastleRightKS(Player player)
        {
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 7)),
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 7)),
                _=> false

            };
        }
        public bool CastleRightQS(Player player)
        {
            return player switch
            {
                Player.White => IsUnmovedKingAndRook(new Position(7, 4), new Position(7, 0)),
                Player.Black => IsUnmovedKingAndRook(new Position(0, 4), new Position(0, 0)),
                _ => false

            };
        }

        private bool HasPawnInPosition(Player player, Position[] pawnPosition, Position pawnSkip)
        {
            foreach (Position pos in pawnPosition.Where(OnBoard))
            {
                Piece piece = this[pos];
                if (piece == null || piece.Colour != player || piece.Variant != PieceVariant.Pawn)
                {
                    continue;
                }
                EnPasant move = new EnPasant(pos, pawnSkip);
                if(move.IsPossible(this))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanEnPassant(Player player)
        {
            Position pawnSkip = GetPawnSkip(player.Enemy());

            if(pawnSkip == null)
            {
                return false;
            }
            Position[] pawnPosition = player switch
            {

                Player.White => new Position[] { pawnSkip + PieceMovement.downLeft, pawnSkip + PieceMovement.downRight },
                Player.Black => new Position[] { pawnSkip + PieceMovement.upLeft, pawnSkip + PieceMovement.upRight },
                _=> Array.Empty<Position>()
            };
            return HasPawnInPosition(player, pawnPosition, pawnSkip);
        }

        


    }
}
