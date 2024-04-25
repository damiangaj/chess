using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for StartMenu.xaml
    /// </summary>
    public partial class StartMenu : UserControl
    {

        public Action<Option> ChoosedOption;
        public StartMenu()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            ChoosedOption?.Invoke(Option.Start);
        }

        private void JoinGame_Click(object sender, RoutedEventArgs e)
        {
            ChoosedOption?.Invoke(Option.JoinMenu);
        }

        private void HostGame_Click(object sender, RoutedEventArgs e)
        {
            ChoosedOption?.Invoke(Option.HostMenu);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            ChoosedOption?.Invoke(Option.Exit);
        }
    }
}
