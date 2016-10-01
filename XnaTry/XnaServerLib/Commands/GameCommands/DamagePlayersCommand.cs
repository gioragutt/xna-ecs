using System.Collections.Generic;
using System.Linq;
using UtilsLib;
using UtilsLib.Consts;
using UtilsLib.Exceptions.Server.Commands;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;
using XnaServerLib.Commands.BaseClasses;

namespace XnaServerLib.Commands.GameCommands
{
    public class DamagePlayersCommand : BaseServerCommand
    {
        public override bool CanExecute(IList<string> parameters)
        {
            float damage;
            return parameters != null && parameters.Count > 1 && float.TryParse(parameters[1], out damage) && damage > 0;
        }

        protected override void DoExecute(IList<GameObject> gameObjects, IList<string> parameters)
        {
            float damage;
            if (!float.TryParse(parameters[0], out damage))
                throw new CommandExecutionException("Damage parameter is not a number");

            if (damage < 0)
                throw new CommandExecutionException("Damage paremeter must be bigger then 0");

            var playersList = parameters.Skip(1).ToList();

            if (playersList.Count == 0)
                DealDamageToPlayers(gameObjects, damage);
            else
                DealDamageToPlayerInList(gameObjects, playersList, damage);

            base.Execute(gameObjects, parameters);
        }

        private void DealDamageToPlayerInList(IList<GameObject> gameObjects, IEnumerable<string> playersList, float damage)
        {
            foreach (var playerName in playersList)
            {
                var player = gameObjects.FirstOrDefault(p => p.Components.Get<PlayerAttributes>().Name == playerName);
                if (player == null)
                    AddExecutionException(new CommandExecutionException(string.Format("Player {0} does not exist", playerName)));
                else
                    DealDamageToPlayer(player, damage);
            }
        }

        private void DealDamageToPlayers(IEnumerable<GameObject> players, float damage)
        {
            players.ToList().ForEach(player =>
            {
                DealDamageToPlayer(player, damage);
            });
        }

        private void DealDamageToPlayer(GameObject player, float damage)
        {
            var attributes = player.Components.Get<PlayerAttributes>();
            attributes.Health -= damage;
            if (attributes.JustDied)
            {
                Broadcast(
                    MessageBuilder.Create(Constants.Messages.AddMessageToBox)
                    .Add(Constants.Fields.Content, string.Format("{0} died!", attributes.Name))
                    .Get());
            }
        }
    }
}