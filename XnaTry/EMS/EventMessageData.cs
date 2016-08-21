using System.IO;
using System.Text;

namespace EMS
{
    /// <summary>
    /// Contains all data of an event message
    /// </summary>
    public struct EventMessageData
    {
        /// <summary>
        /// The name of the message
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The data transferred on the event message.
        /// </summary>
        /// <remarks>
        /// Can be empty
        /// </remarks>
        public byte[] Data { get; set; }

        /// <summary>
        /// Indicated whether this message came from a remote host
        /// </summary>
        public bool Transmitted { get; private set; }

        #region Constructors

        /// <summary>
        /// Initializes an event message with a name and without data
        /// </summary>
        /// <param name="name">Name of the message</param>
        public EventMessageData(string name) : this(name, new byte[0])
        {
        }

        /// <summary>
        /// Initializes an event message with name and data
        /// </summary>
        /// <param name="name">The name of the message</param>
        /// <param name="data">The data of the mesage</param>
        public EventMessageData(string name, byte[] data)
        {
            Name = name;
            Data = data;
            Transmitted = false;
        }

        /// <summary>
        /// Initializes a new event message that was transmitted from a remote host
        /// </summary>
        /// <param name="reader">Binary reader which contains the message</param>
        public EventMessageData(BinaryReader reader)
        {
            var nameLength = reader.ReadInt32();
            Name = Encoding.ASCII.GetString(reader.ReadBytes(nameLength));
            var dataLength = reader.ReadInt32();
            Data = reader.ReadBytes(dataLength);
            Transmitted = true;
        }

        #endregion Constructors

        /// <summary>
        /// Writes the event message using a binary writer
        /// </summary>
        /// <param name="writer">Writer to the stream which the message will be written to</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(Name.Length);
            writer.Write(Encoding.ASCII.GetBytes(Name));
            writer.Write(Data.Length);
            writer.Write(Data);
        }
    }
}