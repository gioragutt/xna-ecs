using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EMS
{
    public static class EmsUtils
    {
        /// <summary>
        /// Returns the message name registed in the jobject
        /// </summary>
        /// <param name="jObject">A JObject containing an event message</param>
        /// <returns>Name of the message if contains one; otherwise false</returns>
        /// <exception cref="System.ArgumentNullException">if Jobject is null</exception>
        public static string GetMessageName(this JObject jObject)
        {
            AssertArgumentNotNull(jObject, "jObject");

            return jObject.GetValue(Constants.NameField)?.Value<string>(); 
        }

        /// <summary>
        /// Returns whether the message was transmitted from another host
        /// </summary>
        /// <param name="jObject">A JObject containing an event message</param>
        /// <returns>true if the transmitted value was found and was true; otherwise false</returns>
        /// <exception cref="System.ArgumentNullException">if jObject is null</exception>
        public static bool GetTransmitted(this JObject jObject)
        {
            AssertArgumentNotNull(jObject, "jObject");

            return jObject.GetValue(Constants.TransmittedField)?.Value<bool?>() ?? false;
        }

        /// <summary>
        /// Gets the value of a GUID field
        /// </summary>
        /// <param name="jObject">A JObject containing the guid</param>
        /// <param name="propName">The name of the guid property</param>
        /// <returns>A read GUID if exists; otherwise Guid.Empty</returns>
        /// <exception cref="System.ArgumentNullException">if jObject is null</exception>
        /// <exception cref="System.ArgumentNullException">if propName is null</exception>
        public static Guid GetGuid(this JObject jObject, string propName)
        {
            AssertArgumentNotNull(jObject, "jObject");
            AssertStringArgumentNotNull(propName, "propName");

            var guidAsString = jObject.Value<string>(propName);
            return string.IsNullOrEmpty(guidAsString) ? Guid.Empty : Guid.Parse(guidAsString);
        }

        /// <summary>
        /// Creates a message with message name and transmitted values
        /// </summary>
        /// <param name="messageName">Name of the message</param>
        /// <param name="transmitted">Whether the message was transmitted from another host</param>
        /// <returns>The newly created message</returns>
        /// /// <exception cref="System.ArgumentNullException">if the message name was empty or null</exception>
        public static JObject CreateMessage(string messageName, bool transmitted = false)
        {
            AssertStringArgumentNotNull(messageName, "messageName");

            return MessageBuilder.Create(messageName).Add(Constants.TransmittedField, transmitted, true).Get();
        }

        /// <summary>
        /// Writes a JObject to a stream
        /// </summary>
        /// <param name="writer">Writer to write to</param>
        /// <param name="jObject">The JObject to writer</param>
        public static void WriteJObject(BinaryWriter writer, JObject jObject)
        {
            AssertArgumentNotNull(writer, "writer");
            AssertArgumentNotNull(jObject, "jObject");

            writer.Write(jObject.ToString(Formatting.None));
        }

        /// <summary>
        /// Reads a JObject from the stream
        /// </summary>
        /// <param name="reader">Reader to read from</param>
        /// <returns></returns>
        public static JObject ReadJObject(BinaryReader reader)
        {
            return JObject.Parse(reader.ReadString());
        }

        public static void AssertArgumentNotNull(object argument, string parameterName)
        {
            if (ReferenceEquals(argument, null))
                throw new ArgumentNullException(parameterName);
        }
        public static void AssertStringArgumentNotNull(string argument, string parameterName)
        {
            if (string.IsNullOrEmpty(argument))
                throw new ArgumentNullException(parameterName);
        }
    }
}
