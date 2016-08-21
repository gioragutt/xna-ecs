using System;
using System.Collections.Generic;
using System.Linq;

namespace EMS
{
    /// <summary>
    /// Responsible for managing the EMS routine among local EmsClients
    /// </summary>
    public class EmsServer
    {
        #region Members

        /// <summary>
        /// The dictionary of all subsciptions to specific messages
        /// </summary>
        private readonly Dictionary<string, List<ClientCallbackPair>> subscriptions;

        /// <summary>
        /// List of clients that are subscribed to all messages
        /// </summary>
        private readonly List<ClientCallbackPair> subscriptionsToAllMessages;

        #endregion

        #region Singleton

        /// <summary>
        /// Lazy-ly create a singleton instance
        /// </summary>
        private static readonly Lazy<EmsServer> Lazy = new Lazy<EmsServer>(() => new EmsServer());

        /// <summary>
        /// Get the local EmsServer instance
        /// </summary>
        public static EmsServer Instance => Lazy.Value;

        /// <summary>
        /// Initialize an EmsServer
        /// </summary>
        private EmsServer()
        {
            subscriptions = new Dictionary<string, List<ClientCallbackPair>>();
            subscriptionsToAllMessages = new List<ClientCallbackPair>();
            Console.WriteLine("Initializing EmsServer");
        }

        #endregion

        #region EMS API

        /// <summary>
        /// Subscribes a client to all broadcast messages (directed to a single callback)
        /// </summary>
        /// <param name="client">Subscribing client</param>
        /// <param name="callback">The registered callback</param>
        /// <exception cref="System.ArgumentNullException">if client is null</exception>
        /// <exception cref="System.ArgumentNullException">if callback is null</exception>
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

        /// <summary>
        /// Subscribes a client to a single message
        /// </summary>
        /// <param name="client">Subscribing client</param>
        /// <param name="callback">The registered callback</param>
        /// <param name="messageName">Name of the message to subscribe to</param>
        /// <exception cref="System.ArgumentNullException">if client is null</exception>
        /// <exception cref="System.ArgumentNullException">if callback is null</exception>
        /// <exception cref="System.ArgumentNullException">if messageName is null</exception>
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

        /// <summary>
        /// Rmoves a client's subscription from a certain message
        /// </summary>
        /// <param name="client">Subscribing client</param>
        /// <param name="messageName">Name of the message to subscribe to</param>
        /// <exception cref="System.ArgumentNullException">if client is null</exception>
        /// <exception cref="System.ArgumentNullException">if messageName is null</exception>
        public void Unsubscribe(IEmsClient client, string messageName)
        {
            #region Argument Null Assertion

            if (client == null)
                throw new ArgumentNullException("client");
            if (messageName == null)
                throw new ArgumentNullException("messageName");

            #endregion

            var existing = FindClient(messageName, client);
            if (existing == null)
                return;

            subscriptions[messageName].Remove(existing);
            if (subscriptions[messageName].Count == 0)
                subscriptions.Remove(messageName);
        }

        public void UnsubscribeFromAll(IEmsClient client)
        {
            var existing = FindClientInAll(client);

            if (existing != null)
                subscriptionsToAllMessages.Remove(existing);
        }

        /// <summary>
        /// Broadcasts an event message
        /// </summary>
        /// <param name="message">Message to broadcast</param>
        /// <remarks>
        /// Broadcast to clients subscribed to all messages, and to clients subscribed to
        /// The broadcast message name, if any exists
        /// </remarks>
        public void Broadcast(EventMessageData message)
        {
            subscriptionsToAllMessages.ForEach(c => c.Callback(message));

            if (!subscriptions.ContainsKey(message.Name))
                return;

            subscriptions[message.Name].ForEach(c => c.Callback(message));
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Retrives a client's subscription in the subscriptions to all messages
        /// </summary>
        /// <param name="client">Client to search for</param>
        /// <returns>
        /// A pair containing the client and the callback, if he's subscribed to all; otherwise false
        /// </returns>
        private ClientCallbackPair FindClientInAll(IEmsClient client)
        {
            return subscriptionsToAllMessages.FirstOrDefault(c => c.Client == client);
        }

        /// <summary>
        /// Retrieves a client's subscription for a certain message
        /// </summary>
        /// <param name="message">Message the client is subscribed to</param>
        /// <param name="client">Client to search for</param>
        /// <returns>
        /// A pair containing the client and the callback, if he's subscribed to the given message; otherwise false
        /// </returns>
        private ClientCallbackPair FindClient(string message, IEmsClient client)
        {
            if (!subscriptions.ContainsKey(message))
                return null;

            return subscriptions[message].FirstOrDefault(c => c.Client == client);
        }

        #endregion
    }
}
