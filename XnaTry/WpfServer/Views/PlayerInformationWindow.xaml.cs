using System;
using System.Windows;

namespace WpfServer.Views
{
    /// <summary>
    /// Interaction logic for PlayerInformationWindow.xaml
    /// </summary>
    public partial class PlayerInformationWindow : Window
    {
        public PlayerInformationWindow() { InitializeComponent(); }

        private void CloseWindowOnKick(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(Close));
        }
    }
}
