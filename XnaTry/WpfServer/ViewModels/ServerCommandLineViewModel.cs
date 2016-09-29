using System.Windows.Input;
using WpfServer.Commands;
using WpfServer.Windows;
using XnaServerLib.Commands;

namespace WpfServer.ViewModels
{
    public class ServerCommandLineViewModel : ViewModelBase
    {
        public ICommand ExecuteServerCommandCommand { get; set; }

        public ServerCommandLineViewModel(ServerCommandsService commandsService)
        {
            ExecuteServerCommandCommand = new ExecuteServerCommandCommand(commandsService);
        }
    }
}
