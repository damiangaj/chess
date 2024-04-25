using ChessCore;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChessGUI
{
    /// <summary>
    /// Interaction logic for ChangingPawnMenu.xaml
    /// </summary>
    public partial class ChangingPawnMenu : UserControl
    {
        public event Action<PieceVariant> PieceChoosed;

        public ChangingPawnMenu(Player player)
        {
            InitializeComponent();

            rookPNG.Source = Pngs.TakePng(player, PieceVariant.Rook);
            knightPNG.Source = Pngs.TakePng(player, PieceVariant.Knight);
            queenPNG.Source = Pngs.TakePng(player, PieceVariant.Queen);
            bishopPNG.Source = Pngs.TakePng(player, PieceVariant.Bishop);

        }

        private void rookPNG_mouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceChoosed?.Invoke(PieceVariant.Rook);
        }

        private void knightPNG_mouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceChoosed?.Invoke(PieceVariant.Knight);
        }

        private void queenPNG_mouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceChoosed?.Invoke(PieceVariant.Queen);
        }

        private void bishopPNG_mouseDown(object sender, MouseButtonEventArgs e)
        {
            PieceChoosed?.Invoke(PieceVariant.Bishop);
        }
    }
}
