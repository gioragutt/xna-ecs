using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SharedGameData;
using XnaCommonLib;
using Application = System.Windows.Forms.Application;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Registry Names

        private const string IpRegistry = "IP";
        private const string NameRegistry = "Name";
        private const string TeamRegistry = "Team";

        #endregion

        #region Login Data

        public string IpAddress { get; set; }
        public string PlayerName { get; set; }
        public string PlayerTeam { get; set; }

        #endregion

        public Dictionary<string, TeamData> Teams { get; }
        public MainWindow()
        {
            InitializeComponent();
            Teams = TeamsData.Teams;
            DataContext = this;

            var appData = Application.UserAppDataRegistry;
            object nameRegistry = null;
            object ipRegistry = null;
            object teamRegistry = null;

            if (appData != null)
            {
                nameRegistry = appData.GetValue(NameRegistry);
                ipRegistry = appData.GetValue(IpRegistry);
                teamRegistry = appData.GetValue(TeamRegistry);
            }

            PlayerName = nameRegistry?.ToString() ?? "Player Name";
            IpAddress = ipRegistry?.ToString() ?? "localhost";
            PlayerTeam = teamRegistry?.ToString() ?? Teams.Keys.First();

            ResetInput();
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
            var teamSelected = TeamsDropdown.SelectedIndex != -1;
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
            ResetInput();
        }

        private void ResetInput()
        {
            NameBox.Text = PlayerName;
            AddressBox.Text = IpAddress;
            TeamsDropdown.SelectedIndex = Teams.Keys.ToList().IndexOf(PlayerTeam);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var error = InputValid();
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Invalid input");
                return;
            }

            var teamName = ((KeyValuePair<string, TeamData>) TeamsDropdown.SelectedItem).Value.Name;

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
                appData.SetValue(TeamRegistry, teamName);
            }

            // Starting game process
            newPros.Start();

            // Killing this process
            currPros.Kill();
        }
    }
}
