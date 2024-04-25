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
    /// Interaction logic for HostGame.xaml
    /// </summary>
    public partial class HostGame : UserControl
    {
        public Action<Option> ChoosedOption;
        public HostGame()
        {
            InitializeComponent();
            StartGame_Button.IsEnabled = false;
            StopHosting_Button.IsEnabled = false;
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            ChoosedOption?.Invoke(Option.Start);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ChoosedOption?.Invoke(Option.Back);
        }

        private void HostGame_Click(object sender, RoutedEventArgs e)
        {
            ChoosedOption?.Invoke(Option.StartHosting);
        }

        private void StopHosting_Click(object sender, RoutedEventArgs e)
        {
            ChoosedOption?.Invoke(Option.StopHosting);
        }


    }
}
