using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using UtilsLib.Utility;

namespace UtilsLib
{
    // ReSharper disable once InconsistentNaming
    public static class IOExtentions
    {
        #region Vector2

        /// <summary>
        /// Writes a Vector2 to a stream
        /// </summary>
        /// <param name="writer">Writer to write to</param>
        /// <param name="vec">Vector2 to write</param>
        public static void WriteVector2(this BinaryWriter writer, Vector2 vec)
        {
            Utils.AssertArgumentNotNull(writer, "writer");

            writer.Write(vec.X);
            writer.Write(vec.Y);
        }

        /// <summary>
        /// Reads a Vector2 from a stream
        /// </summary>
        /// <param name="reader">Reader to read from</param>
        /// <returns>The read Vector2</returns>
        public static Vector2 ReadVector2(this BinaryReader reader)
        {
            Utils.AssertArgumentNotNull(reader, "reader");

            var x = reader.ReadSingle();
            var y = reader.ReadSingle();
            return new Vector2(x, y);
        }

        #endregion Vector2

        #region Guid

        /// <summary>
        /// Wrutes a GUID to a stream
        /// </summary>
        /// <param name="writer">Writer to write to</param>
        /// <param name="guid">Guid to write</param>
        public static void WriterGuid(this BinaryWriter writer, Guid guid)
        {
            Utils.AssertArgumentNotNull(writer, "writer");
            Utils.AssertArgumentNotNull(guid, "guid");

            writer.Write(guid.ToByteArray());
        }

        /// <summary>
        /// Reads a GUID from a binary reader
        /// </summary>
        /// <param name="reader">Reader to read from</param>
        /// <returns>The GUID read from the stream</returns>
        public static Guid ReadGuid(this BinaryReader reader)
        {
            Utils.AssertArgumentNotNull(reader, "reader");

            return new Guid(reader.ReadBytes(16));
        }

        #endregion

        #region JObject

        /// <summary>
        /// Writes a JObject to a stream
        /// </summary>
        /// <param name="writer">Writer to write to</param>
        /// <param name="jObject">The JObject to write</param>
        public static void WriteJObject(this BinaryWriter writer, JObject jObject)
        {
            Utils.AssertArgumentNotNull(writer, "writer");
            Utils.AssertArgumentNotNull(jObject, "jObject");

            writer.Write(jObject.ToString(Formatting.None));
        }

        /// <summary>
        /// Reads a JObject from the stream
        /// </summary>
        /// <param name="reader">Reader to read from</param>
        /// <returns>The read JObject</returns>
        public static JObject ReadJObject(this BinaryReader reader)
        {
            Utils.AssertArgumentNotNull(reader, "reader");

            return JObject.Parse(reader.ReadString());
        }

        #endregion
    }
}
