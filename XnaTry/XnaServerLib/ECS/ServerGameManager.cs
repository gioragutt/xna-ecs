using System;
using ECS.BaseTypes;
using EMS;
using XnaCommonLib;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;

namespace XnaServerLib.ECS
{
    public class ServerGameManager : GameManagerBase
    {
        private Server Server { get; }
        public ServerGameManager(Server server)
        {
            Server = server;

            Subscribe(EventMessageNames.ClientDisconnected, Callback_CllientDisconnected);       
        }

        public void DisposeOfClient(GameClient client)
        {
            Server.GameClients.Remove(client);

            client.GameObject.Entity.Dispose();
        }

        private void Callback_CllientDisconnected(EventMessageData eventMessageData)
        {
            var guid = new Guid(eventMessageData.Data);
            EntityPool.Remove(new Entity(guid));
        }

        public void Update(TimeSpan delta)
        {
            Update((long)delta.TotalMilliseconds);
        }
    }
}