using System;
using Newtonsoft.Json.Linq;

namespace EMS
{
    /// <summary>
    /// An interface defining behavior of an EventMessage system client
    /// </summary>
    public interface IEmsClient
    {
        /// <summary>
        /// Subscribe to a given message, providing a callback when subscribed message is broadcast
        /// </summary>
        /// <param name="messageName">Name of the message to subscribe to</param>
        /// <param name="callback">Callback for when the message is broadcast</param>
        void Subscribe(string messageName, Action<JObject> callback);

        /// <summary>
        /// Subscribe to all outgoing messages
        /// </summary>
        /// <param name="callback">The callback for when a message is broadcast</param>
        void SubscribeToAll(Action<JObject> callback);

        /// <summary>
        /// Unsubscribe from a certain message
        /// </summary>
        /// <param name="messageName">Message name to unsubscribe from</param>
        void Unsubscribe(string messageName);

        /// <summary>
        /// Unsubscribes from receiving all messages
        /// </summary>
        void UnsubscribeFromAll();

        /// <summary>
        /// Broadcast an event message
        /// </summary>
        /// <param name="message">Message to broadcast</param>
        void Broadcast(JObject message);
    }
}