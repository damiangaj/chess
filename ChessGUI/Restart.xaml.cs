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
    /// Interaction logic for Restart.xaml
    /// </summary>
    public partial class Restart : UserControl
    {
        public Action<Option> ChoosedOption;
        public Restart()
        {
            InitializeComponent();
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            ChoosedOption?.Invoke(Option.Restart);
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            ChoosedOption?.Invoke(Option.Continue);
        }
    }
}

