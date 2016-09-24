using System.ComponentModel;
using WpfServer.ViewModels;

namespace WpfServer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly ServerViewModel server;

        public MainWindow()
        {
            InitializeComponent();
            server = new ServerViewModel(Dispatcher);
            DataContext = server;
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            server?.StopListeningCommand.Execute(null);
        }
    }
}
