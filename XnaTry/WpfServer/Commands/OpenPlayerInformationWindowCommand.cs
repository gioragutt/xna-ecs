using System;
using System.Diagnostics;
using WpfServer.Models;
using WpfServer.ViewModels;
using WpfServer.Views;
using WpfServer.Windows;

namespace WpfServer.Commands
{
    public class OpenPlayerInformationWindowCommand : AsyncCommandBase
    {
        private readonly ServerViewModel serverViewModel;

        public OpenPlayerInformationWindowCommand(ServerViewModel serverViewModel)
        {
            this.serverViewModel = serverViewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return parameter != null;
        }
        
        protected override void OnExecute(object parameter)
        {
            serverViewModel.Dispatcher.BeginInvoke(new Action(() =>
            {
                Debug.Assert(parameter is PlayerInformation);
                var player = (PlayerInformation) parameter;

                var playerInfoWindow = new PlayerInformationWindow
                {
                    DataContext = new PlayerInformationViewModel(player, serverViewModel),
                };

                playerInfoWindow.ShowDialog();
            }));
        }
    }
}
