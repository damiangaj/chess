using ChessCore;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ChessGUI
{
    /// <summary>
    /// Interaction logic for GameEndingMenu.xaml
    /// </summary>
    public partial class GameEndingMenu : UserControl
    {
        public Action<Option> ChoosedOption;

        public GameEndingMenu(Tour tour)
        {
            InitializeComponent();
            Ending ending = tour.Ending;
            WinnerText.Text = WinnerMessage(ending.Winner);
            PossibleEndingText.Text = PossibleEndingMessage(ending.PossibleEndings, tour.CurrentPlayer);
        }

        private static string WinnerMessage(Player player)
        {
            string message = string.Empty;
            if (player == Player.White) { message = "White won"; }
            else if (player == Player.Black) { message = "Black won"; }
            else { message = "Draw"; }
            return message;
        }

       /* private static string PlayerColour(Player player)
        {
            string colour = string.Empty;
            if (player == Player.White) { colour = "White"; }
            else if (player == Player.Black) { colour = "Black"; }
            else { colour = string.Empty; }
            return colour;
        }*/

        private static string PossibleEndingMessage(PossibleEndings possibleEndings, Player player)
        {
            string message = string.Empty;
            if(possibleEndings == PossibleEndings.Checkmate) { message = "Checkmate!"; }
            else if (possibleEndings == PossibleEndings.Stalemate) { message = "Stalemate!"; }
            else if (possibleEndings == PossibleEndings.InsufficientMaterial) { message = "Insufficient Material!"; }
            else if (possibleEndings == PossibleEndings.FiftyMoveRule) { message = "50  moves by each player without a pawn move or a piece capture"; }
            else if (possibleEndings == PossibleEndings.ThreefoldRepetition) { message = "The same position occured three times"; }
            else { message = string.Empty; }
            return message;
        }
        

        

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            ChoosedOption?.Invoke(Option.Restart);
        }
    }
}
