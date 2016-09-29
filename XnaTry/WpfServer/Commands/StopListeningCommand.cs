using System;
using WpfServer.ViewModels;
using WpfServer.Windows;

namespace WpfServer.Commands
{
    public class StopListeningCommand : AsyncCommandBase
    {
        private readonly ServerViewModel viewModel;

        public StopListeningCommand(ServerViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.Server.Listening;
        }

        protected override void OnExecute(object parameter)
        {
            viewModel.Server.StopListen();
            viewModel.ServerStatus = "Stopping Listening";

            base.OnExecute(parameter);
        }

        protected override void AfterExecute(object parameter, Exception error)
        {
            var status = "Not Listening";
            if (error != null)
                status += string.Format(" - {0}", error.Message);
            viewModel.ServerStatus = status;

            base.AfterExecute(parameter, error);
        }
    }
}