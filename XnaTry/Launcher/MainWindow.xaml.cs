using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Application = System.Windows.Forms.Application;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string IpRegistry = "IP";
        private const string NameRegistry = "Name";
        public string IpAddress { get; set; }
        public string PlayerName { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            var appData = Application.UserAppDataRegistry;

            object nameRegistry = null;
            object ipRegistry = null;

            if (appData != null)
            {
                nameRegistry = appData.GetValue(NameRegistry);
                ipRegistry = appData.GetValue(IpRegistry);
            }

            PlayerName = nameRegistry?.ToString() ?? "Player Name";
            IpAddress = ipRegistry?.ToString() ?? "localhost";

            NameBox.Text = PlayerName;
            AddressBox.Text = IpAddress;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private string InputValid()
        {
            var nameInserted = !string.IsNullOrEmpty(NameBox.Text.Trim());
            var ipInserted = !string.IsNullOrEmpty(AddressBox.Text.Trim());
            IPAddress dummy;
            var isIpLegit = ipInserted && (IPAddress.TryParse(AddressBox.Text, out dummy) || AddressBox.Text.Trim().Equals("localhost"));
            var teamSelected = (GoodTeamRadioButton.IsChecked ?? false) || (BadTeamRadioButton.IsChecked ?? false);
            var error = new StringBuilder();
            if (!nameInserted)
                error.AppendLine("A name must be entered");
            if (!isIpLegit)
                error.AppendLine("A legit IP Address must be entered");
            if (!teamSelected)
                error.AppendLine("A team must be chosen");

            return error.ToString();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            NameBox.Text = PlayerName;
            AddressBox.Text = IpAddress;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var error = InputValid();
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Invalid input");
                return;
            }

            var teamName = string.Empty;
            if (GoodTeamRadioButton.IsChecked ?? false)
                teamName = "Good";
            else if (BadTeamRadioButton.IsChecked ?? false)
                teamName = "Bad";

            PlayerName = NameBox.Text.Trim();
            IpAddress = AddressBox.Text.Trim();

            // Getting current process
            var currPros = Process.GetCurrentProcess();

            // Creating game process
            var newPros = new Process
            {
                StartInfo =
                {
                    FileName = "XnaTry.exe",
                    Arguments = string.Format("\"{0}\" {1} {2}", PlayerName, IpAddress, teamName),
                    UseShellExecute = false
                }
            };

            var appData = Application.UserAppDataRegistry;
            if (appData != null)
            {
                appData.SetValue(NameRegistry, PlayerName);
                appData.SetValue(IpRegistry, IpAddress);
            }

            // Starting game process
            newPros.Start();

            // Killing this process
            currPros.Kill();
        }
    }
}
