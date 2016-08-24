using System;
using System.Linq;
using ECS.BaseTypes;
using Newtonsoft.Json.Linq;
using XnaCommonLib;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;
using EmsConsts = EMS.Constants;
using MyConsts = XnaCommonLib.Constants;

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

        private void Callback_CllientDisconnected(JObject message)
        {
            var guid = message.Value<Guid>(MyConsts.MessageFields.GuidField);
            EntityPool.Remove(new Entity(guid));
        }

        public void Update(TimeSpan delta)
        {
            Update((long)delta.TotalMilliseconds);
        }

        public string GetAvailablePlayerName(string loginName)
        {
            if (IsNameAvailable(loginName))
                return loginName;

            var count = 1;
            string name;
            do
            {
                name = string.Format("{0}({1})", loginName, count++);
            }
            while (!IsNameAvailable(name));

            return name;
        }

        private bool IsNameAvailable(string name)
        {
            var allEntities = EntityPool.GetAllOf<PlayerAttributes>();
            return allEntities.All(entity => entity.Name != name);
        }
    }
}