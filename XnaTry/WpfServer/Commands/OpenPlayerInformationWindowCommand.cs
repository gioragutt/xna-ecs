using System;
using System.Diagnostics;
using System.Windows.Threading;
using WpfServer.Models;
using WpfServer.Views;
using WpfServer.Windows;

namespace WpfServer.Commands
{
    public class OpenPlayerInformationWindowCommand : AsyncCommandBase
    {
        private Dispatcher dispatcher;

        public OpenPlayerInformationWindowCommand(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public override bool CanExecute(object parameter)
        {
            return parameter != null;
        }
        
        protected override void OnExecute(object parameter)
        {
            dispatcher.BeginInvoke(new Action(() =>
            {
                Debug.Assert(parameter is PlayerInformation);
                var player = (PlayerInformation) parameter;

                var playerInfoWindow = new PlayerInformationWindow
                {
                    DataContext = player,
                };

                playerInfoWindow.ShowDialog();
            }));
        }
    }
}
