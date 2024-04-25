using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChessCore
{
    public class Tour
    {
        public Player CurrentPlayer { get; private set; }
        public Board Board { get; }

        public Ending Ending { get; private set; } = null;

        private int noCapOrPro = 0;
        private string tourString;
        public string moveString = "";

        private readonly Dictionary<string, int> tourHistory = new Dictionary<string, int>();

        public Tour(Player player, Board board)
        {
            this.CurrentPlayer = player;
            this.Board = board;

            tourString = new State(CurrentPlayer, board).ToString();
            tourHistory[tourString] = 1;
        }

     

        public IEnumerable<Move> ValidMove(Position pos)
        {
            if(Board.IsFree(pos) || Board[pos].Colour != CurrentPlayer) 
            { 
                return Enumerable.Empty<Move>();
            }

            Piece piece = Board[pos];
            IEnumerable<Move> moveToVerify = piece.GetMoves(pos, Board);
            return moveToVerify.Where(move => move.IsPossible(Board));
        }

        public void MakeMove(Move mov)
        {

            Position from = mov.From;
            Position to = mov.To;

            int fx = from.X;
            int fy = from.Y;
            int tx = to.X;
            int ty = to.Y;

            string type = mov.Type.ToString();

            moveString = type + ":" + fx + ":" + fy + ":" + tx + ":" + ty;
            Debug.WriteLine(moveString);

           
            Board.SetSkip(CurrentPlayer, null);
            bool capOrPaw =mov.Execute(Board);
            if (capOrPaw)
            {
                noCapOrPro = 0;
                tourHistory.Clear();
            }
            else
            {
                noCapOrPro++;
            }

            CurrentPlayer = CurrentPlayer.Enemy();
            UpdateTourString();
            CheckIfGameEnds();
        }

        public IEnumerable<Move> EveryPossibleMove(Player player)
        {
            IEnumerable<Move> possibleMoves = Board.PiecePositionByColour(player).SelectMany(pos =>
            {
                Piece piece = Board[pos];
                return piece.GetMoves(pos, Board);
            });

            return possibleMoves.Where(move => move.IsPossible(Board));
        }

        private void CheckIfGameEnds()
        {
            if (!EveryPossibleMove(CurrentPlayer).Any())
            {
                if (Board.IsChecked(CurrentPlayer))
                {
                    Ending = Ending.Win(CurrentPlayer.Enemy());
                }
                else
                {
                    Ending = Ending.Draw(PossibleEndings.Stalemate);
                }
            }
            else if (Board.InsufficientMaterial())
            {
                Ending = Ending.Draw(PossibleEndings.InsufficientMaterial);
            }
            else if (Moves50())
            {
                Ending = Ending.Draw(PossibleEndings.FiftyMoveRule);
            }
            else if (ThreeRepetition())
            {
                Ending = Ending.Draw(PossibleEndings.ThreefoldRepetition);
            }
        }

        private bool Moves50()
        {
            int allMoves = noCapOrPro;

            return allMoves == 100;
        }

        public bool IsGameEnded()
        {
            return Ending != null;
        }


        private void UpdateTourString()
        {
            tourString = new State(CurrentPlayer, Board).ToString();
            if(!tourHistory.ContainsKey(tourString))
            {
                tourHistory[tourString] = 1;
            }
            else
            {
                tourHistory[tourString]++;
            }
        }

        private bool ThreeRepetition()
        {
            return tourHistory[tourString] == 3;
        }
    }
}
