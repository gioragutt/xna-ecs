using System;

namespace EMS
{
    public interface IEmsClient
    {
        void Subscribe(string messageName, Action<EventMessageData> callback);
        void SubscribeToAll(Action<EventMessageData> callback);
        void Unsubscribe(string messageName);
        void Broadcast(EventMessageData message);
    }
}