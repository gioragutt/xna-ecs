using System;
using System.Linq;
using WpfServer.Models;
using WpfServer.Windows;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;

namespace WpfServer
{
    public class PlayerInformationService
    {
        public ObservableItemCollection<PlayerInformation> PlayerData { get; }

        public PlayerInformationService()
        {
            PlayerData = new ObservableItemCollection<PlayerInformation>();
        }

        public void AddPlayer(GameObject gameObject)
        {
            var playerInformation = new PlayerInformation
            {
                Id = gameObject.Entity.Id,
                Transform = gameObject.Transform,
                Attributes = gameObject.Components.Get<PlayerAttributes>()
            };

            PlayerData.Add(playerInformation);
        }

        public void RemovePlayer(Guid playerGuid)
        {
            var item = PlayerData.FirstOrDefault(p => p.Id == playerGuid);
            if (item != null)
                PlayerData.Remove(item);
        }

        public void UpdatePlayer(GameObject gameObject)
        {
            PlayerData.FirstOrDefault(p => p.Id == gameObject.Entity.Id)?.Update(gameObject);
        }
    }
}
