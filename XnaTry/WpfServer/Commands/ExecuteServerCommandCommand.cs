using System.Collections.Generic;
using WpfServer.Windows;
using XnaServerLib.Commands;

namespace WpfServer.Commands
{
    public class ExecuteServerCommandCommand : AsyncCommandBase
    {
        private readonly ServerCommandsService commandsService;

        public ExecuteServerCommandCommand(ServerCommandsService commandsService)
        {
            this.commandsService = commandsService;
        }

        public override bool CanExecute(object parameter)
        {
            var commandParameters = parameter as IList<string>;
            return commandParameters != null && commandsService.ParametersValid(commandParameters);
        }

        protected override void OnExecute(object parameter)
        {
            var commandParameters = parameter as IList<string>;
            commandsService.Execute(commandParameters);

            base.OnExecute(parameter);
        }
    }
}
