using System;
using System.Collections.Generic;
using System.Linq;
using UtilsLib.Exceptions.Server.Commands;
using XnaCommonLib.ECS;

namespace XnaServerLib.Commands
{
    public class ServerCommandsService
    {
        private readonly Server server;
        private readonly IDictionary<string, IServerCommand> commands; 

        public ServerCommandsService(Server server)
        {
            this.server = server;
            commands = new Dictionary<string, IServerCommand>
            {
                ["damage"] = new DamagePlayersCommand()
            };
        }

        public void Execute(string commandName, params string[] args)
        {
            var parameters = new List<string>
            {
                commandName
            };
            parameters.AddRange(args);
            Execute(parameters);
        }

        public bool ParametersValid(IList<string> parameters)
        {
            try
            {
                ValidateCommandParameters(parameters);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public CommandExecutionData ValidateCommandParameters(IList<string> parameters)
        {
            if (parameters.Count == 0)
                throw new NoCommandSuppliedException();

            var commandName = parameters[0];
            if (!commands.ContainsKey(commandName))
                throw new NoSuchCommandException(commandName);

            var command = commands[commandName];
            var arguments = parameters.Skip(1).ToList();

            if (!command.CanExecute(parameters))
                throw new InvalidCommandParametersException(arguments);

            return new CommandExecutionData
            {
                Command = command,
                Parameters = arguments
            };
        }

        public void Execute(IList<string> parameters)
        {
            var commandData = ValidateCommandParameters(parameters);
            if (commands != null)
                commandData.Command.Execute(GameObjects(server.GameClients), commandData.Parameters);
        }

        private static IList<GameObject> GameObjects(IEnumerable<GameClient> gameClients)
        {
            return gameClients.Select(c => c.GameObject).ToList();
        } 
    }
}