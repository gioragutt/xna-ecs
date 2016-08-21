using System;
using System.Collections.Generic;
using System.IO;

namespace EMS
{
    public class EmsServerEndpoint : EmsClient
    {
        public List<EventMessageData> OutgoingMessagesBuffer { get; set; }

        public EmsServerEndpoint()
        {
            OutgoingMessagesBuffer = new List<EventMessageData>();
            Console.WriteLine("Initializing EmsServerEndpoint");
            EmsServer.Instance.SubscribeToAll(this, InsertMessageToBuffer);
        }

        private void InsertMessageToBuffer(EventMessageData message)
        {
            if (message.Transmitted)
                return;

            OutgoingMessagesBuffer.Add(message);
        }

        public void Flush(BinaryWriter writer)
        {
            var initialCount = OutgoingMessagesBuffer.Count;
            writer.Write(initialCount);
            for (var i = 0; i < initialCount; ++i)
            {
                var message = OutgoingMessagesBuffer[0];
                message.Write(writer);
                OutgoingMessagesBuffer.RemoveAt(0);
            }
        }

        public void BroadcastIncomingEvents(BinaryReader reader)
        {
            var count = reader.ReadInt32();

            for (var i = 0; i < count; ++i)
            {
                var msg = new EventMessageData(reader);
                Broadcast(msg);
                Console.WriteLine("Recieved {0}", msg.Name);
            }
        }
    }
}