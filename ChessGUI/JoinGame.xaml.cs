using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessGUI
{
    /// <summary>
    /// Interaction logic for JoinGame.xaml
    /// </summary>
    public partial class JoinGame : UserControl
    {
        public Action<Option> ChoosedOption;


        public JoinGame()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ChoosedOption?.Invoke(Option.Back);
        }

        private void JoinGame_Click(object sender, RoutedEventArgs e)
        {
            ChoosedOption?.Invoke(Option.Start);
        }
    }
}
