using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UtilsLib;
using UtilsLib.Consts;

namespace EMS
{
    /// <summary>
    /// Responsible for managing the EMS routine with endpoints from other hosts
    /// </summary>
    public class EmsServerEndpoint : EmsClient
    {
        /// <summary>
        /// All messages the were broadcasts before the last time Flush was called
        /// </summary>
        public Queue<JObject> OutgoingMessagesBuffer { get; set; }

        /// <summary>
        /// Initializes an EmsServerEndpoint
        /// </summary>
        public EmsServerEndpoint()
        {
           OutgoingMessagesBuffer = new Queue<JObject>();
           SubscribeToAll(InsertMessageToBuffer);
        }

        /// <summary>
        /// Inserts a broadcasts message to the buffer
        /// </summary>
        /// <param name="message">The broadcasts message</param>
        private void InsertMessageToBuffer(JObject message)
        {
            // Do no send messaged if they were transmitted from another Endpoint
            if (message.GetTransmitted())
                return;

            OutgoingMessagesBuffer.Enqueue(message);
        }

        /// <summary>
        /// Flushes the currently buffered messages to a list
        /// </summary>
        public IList<JObject> Flush()
        {
            var bufferCopy = OutgoingMessagesBuffer.ToList();
            OutgoingMessagesBuffer.Clear();
            return bufferCopy;
        }

        /// <summary>
        /// Broadcasts all incoming messages from a remote Endpoint
        /// </summary>
        /// <param name="broadcasts">Broadcasts recieved</param>
        /// <remarks>
        /// Messages broadcasts by this method are marked as transmitted, so that messages broadcasts by an 
        /// Endpoint wouldn't be recieved by an endpoint and start a message loop.
        /// In case a message should be transfered again, a suitable client should handle the logic of those messages
        /// </remarks>
        public void BroadcastIncomingEvents(IList<JObject> broadcasts)
        {
            foreach (var broadcast in broadcasts)
            {
                var msg = MessageBuilder.Create(new JObject(broadcast)).Add(Constants.Fields.Transmitted, true, true).Get();
                Broadcast(msg);
            }
        }
    }
}