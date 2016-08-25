using System;
using System.Collections.Generic;
using System.IO;
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
        /// All messages the were broadcast before the last time Flush was called
        /// </summary>
        public Queue<JObject> OutgoingMessagesBuffer { get; set; }

        /// <summary>
        /// Initializes an EmsServerEndpoint
        /// </summary>
        public EmsServerEndpoint()
        {
            OutgoingMessagesBuffer = new Queue<JObject>();
            EmsServer.Instance.SubscribeToAll(this, InsertMessageToBuffer);
        }

        /// <summary>
        /// Inserts a broadcast message to the buffer
        /// </summary>
        /// <param name="message">The broadcast message</param>
        private void InsertMessageToBuffer(JObject message)
        {
            // Do no send messaged if they were transmitted from another Endpoint
            if (message.GetTransmitted())
                return;

            OutgoingMessagesBuffer.Enqueue(message);
        }

        /// <summary>
        /// Flushes the currently buffered messages to a binary writer
        /// </summary>
        /// <param name="writer">BinaryWriter to writer messages to</param>
        public void Flush(BinaryWriter writer)
        {
            // Write current amount of messages
            var initialCount = OutgoingMessagesBuffer.Count;
            writer.Write(initialCount);

            // Send only the messages we declared above to send
            for (var i = 0; i < initialCount; ++i)
            {
                var message = OutgoingMessagesBuffer.Dequeue();
                writer.WriteJObject(message);
            }
        }

        /// <summary>
        /// Reads all messages from a binary reader and broadcasts them
        /// </summary>
        /// <param name="reader">BinaryReader containing the trasmitted messages</param>
        public void BroadcastIncomingEvents(BinaryReader reader)
        {
            var count = reader.ReadInt32();

            for (var i = 0; i < count; ++i)
            {
                var msg = MessageBuilder.Create(reader.ReadJObject()).Add(Constants.Fields.Transmitted, true, true).Get();
                Broadcast(msg);
            }
        }
    }
}