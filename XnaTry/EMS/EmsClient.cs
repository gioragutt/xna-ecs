using System;

namespace EMS
{
    public class EmsClient : IEmsClient
    {
        private readonly EmsServer server;

        public EmsClient(EmsServer emsServer = null)
        {
            server = emsServer ?? EmsServer.Instance;
        }

        public void Subscribe(string messageName, Action<EventMessageData> callback)
        {
            server.Subscribe(this, messageName, callback);
        }

        public void SubscribeToAll(Action<EventMessageData> callback)
        {
            server.SubscribeToAll(this, callback);
        }

        public void Unsubscribe(string messageName)
        {
            server.Unsubscribe(this, messageName);
        }

        public void Broadcast(string messageName)
        {
            server.Broadcast(new EventMessageData(messageName));
        }

        public void Broadcast(EventMessageData message)
        {
            server.Broadcast(message);
        }
    }
}
