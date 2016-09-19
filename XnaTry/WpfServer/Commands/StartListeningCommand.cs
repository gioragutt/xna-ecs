using System;
using WpfServer.ViewModels;
using WpfServer.Windows;

namespace WpfServer.Commands
{
    public class StartListeningCommand : AsyncCommandBase
    {
        private readonly ServerViewModel viewModel;

        public StartListeningCommand(ServerViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return !viewModel.Server.Listening;
        }

        protected override void OnExecute(object parameter)
        {
            viewModel.Server.StartListen();
            viewModel.ServerStatus = "Starting Listening";
        }

        protected override void AfterExecute(object parameter, Exception error)
        {
            var status = "Listening";
            if (error != null)
                status += string.Format(" - {0}", error.Message);
            viewModel.ServerStatus = status;
        }
    }
}