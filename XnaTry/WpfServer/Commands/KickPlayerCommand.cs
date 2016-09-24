using System.Diagnostics;
using System.Linq;
using WpfServer.Models;
using WpfServer.ViewModels;
using WpfServer.Windows;

namespace WpfServer.Commands
{
    public class KickPlayerCommand : AsyncCommandBase
    {
        private readonly ServerViewModel viewModel;

        public KickPlayerCommand(ServerViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return parameter != null;
        }

        protected override void OnExecute(object parameter)
        {
            Debug.Assert(parameter is PlayerInformation);
            var player = (PlayerInformation) parameter;
            var id = player.Id;
            var gameClient = viewModel.Server.GameClients.FirstOrDefault(c => c.GameObject.Entity.Id.Equals(id));

            gameClient?.StopClient();
        }
    }
}
