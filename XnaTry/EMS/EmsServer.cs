using System;
using System.Collections.Generic;
using System.Linq;

namespace EMS
{
    public class EmsServer
    {
        private readonly Dictionary<string, List<ClientCallbackPair>> subscriptions;
        private readonly List<ClientCallbackPair> subscriptionsToAllMessages;

        #region Singleton

        private static readonly Lazy<EmsServer> Lazy = new Lazy<EmsServer>(() => new EmsServer());
        public static EmsServer Instance => Lazy.Value;

        private EmsServer()
        {
            subscriptions = new Dictionary<string, List<ClientCallbackPair>>();
            subscriptionsToAllMessages = new List<ClientCallbackPair>();
            Console.WriteLine("Initializing EmsServer");
        }

        #endregion

        #region EMS API

        public void SubscribeToAll(IEmsClient client, Action<EventMessageData> callback)
        {
            #region Argument Null Assertion

            if (client == null)
                throw new ArgumentNullException("client");
            if (callback == null)
                throw new ArgumentNullException("callback");

            #endregion

            if (FindClientInAll(client) != null)
                return;

            subscriptionsToAllMessages.Add(new ClientCallbackPair
            {
                Client = client,
                Callback = callback
            });
        }

        public void Subscribe(IEmsClient client, string messageName, Action<EventMessageData> callback)
        {
            #region Argument Null Assertion

            if (client == null)
                throw new ArgumentNullException("client");
            if (messageName == null)
                throw new ArgumentNullException("messageName");
            if (callback == null)
                throw new ArgumentNullException("callback");

            #endregion

            if (FindClient(messageName, client) != null)
                return;

            if (!subscriptions.ContainsKey(messageName))
                subscriptions.Add(messageName, new List<ClientCallbackPair>());

            subscriptions[messageName].Add(new ClientCallbackPair
            {
                Client = client,
                Callback = callback
            });
        }

        public void Unsubscribe(IEmsClient client, string messageName)
        {
            var existing = FindClient(messageName, client);
            if (existing == null)
                return;

            subscriptions[messageName].Remove(existing);
            if (subscriptions[messageName].Count == 0)
                subscriptions.Remove(messageName);
        }

        public void Broadcast(EventMessageData message)
        {
            subscriptionsToAllMessages.ForEach(c => c.Callback(message));

            if (!subscriptions.ContainsKey(message.Name))
                return;

            subscriptions[message.Name].ForEach(c => c.Callback(message));
        }

        #endregion

        #region Helper Methods

        private ClientCallbackPair FindClientInAll(IEmsClient client)
        {
            return subscriptionsToAllMessages.FirstOrDefault(c => c.Client == client);
        }

        private ClientCallbackPair FindClient(string message, IEmsClient client)
        {
            if (!subscriptions.ContainsKey(message))
                return null;

            return subscriptions[message].FirstOrDefault(c => c.Client == client);
        }

        #endregion
    }
}
