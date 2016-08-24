using System;
using Newtonsoft.Json.Linq;

namespace EMS
{
    /// <summary>
    /// Implements the EmsClient interface, interacting with the local EmsServer by default
    /// </summary>
    public class EmsClient : IEmsClient
    {
        #region Members

        private readonly EmsServer server;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new EmsClient
        /// </summary>
        /// <param name="emsServer">Can specify server to connect to. Connects to local server by default</param>
        public EmsClient(EmsServer emsServer = null)
        {
            server = emsServer ?? EmsServer.Instance;
        }

        #endregion

        #region Subscriptions

        /// <summary>
        /// Subscribe to a given message, providing a callback when subscribed message is broadcast
        /// </summary>
        /// <param name="messageName">Name of the message to subscribe to</param>
        /// <param name="callback">Callback for when the message is broadcast</param>
        public void Subscribe(string messageName, Action<JObject> callback)
        {
            server.Subscribe(this, messageName, callback);
        }

        /// <summary>
        /// Subscribe to all outgoing messages
        /// </summary>
        /// <param name="callback">The callback for when a message is broadcast</param>
        public void SubscribeToAll(Action<JObject> callback)
        {
            server.SubscribeToAll(this, callback);
        }

        #endregion Subscriptions

        #region Unsubcriptions

        /// <summary>
        /// Unsubscribe from a certain message
        /// </summary>
        /// <param name="messageName">Message name to unsubscribe from</param>
        public void Unsubscribe(string messageName)
        {
            server.Unsubscribe(this, messageName);
        }

        /// <summary>
        /// Unsubscribes from receiving all messages
        /// </summary>
        public void UnsubscribeFromAll()
        {
            server.UnsubscribeFromAll(this);
        }

        #endregion Unsubcriptions

        #region Broadcasts

        /// <summary>
        /// Broadcast an event message without data
        /// </summary>
        /// <param name="messageName">Name of the message to broadcast</param>
        public void Broadcast(string messageName)
        {
            server.Broadcast(EmsUtils.CreateMessage(messageName));
        }

        /// <summary>
        /// Broadcast an event message
        /// </summary>
        /// <param name="message">Message to broadcast</param>
        public void Broadcast(JObject message)
        {
            server.Broadcast(message);
        }

        #endregion Broadcasts
    }
}
