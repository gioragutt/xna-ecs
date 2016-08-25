using System;
using Newtonsoft.Json.Linq;
using UtilsLib.Consts;
using UtilsLib.Utility;

namespace UtilsLib
{
    public static class Extentions
    {
        /// <summary>
        /// Returns the message name registed in the jobject
        /// </summary>
        /// <param name="jObject">A JObject containing an event message</param>
        /// <returns>Name of the message if contains one; otherwise false</returns>
        /// <exception cref="System.ArgumentNullException">if Jobject is null</exception>
        public static string GetMessageName(this JObject jObject)
        {
            Utils.AssertArgumentNotNull(jObject, "jObject");

            return jObject.GetValue(Constants.Fields.MessageName)?.Value<string>();
        }

        /// <summary>
        /// Returns whether the message was transmitted from another host
        /// </summary>
        /// <param name="jObject">A JObject containing an event message</param>
        /// <returns>true if the transmitted value was found and was true; otherwise false</returns>
        /// <exception cref="System.ArgumentNullException">if jObject is null</exception>
        public static bool GetTransmitted(this JObject jObject)
        {
            Utils.AssertArgumentNotNull(jObject, "jObject");

            return jObject.GetValue(Constants.Fields.Transmitted)?.Value<bool?>() ?? false;
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
            Utils.AssertArgumentNotNull(jObject, "jObject");
            Utils.AssertStringArgumentNotNull(propName, "propName");

            var guidAsString = jObject.Value<string>(propName);
            return string.IsNullOrEmpty(guidAsString) ? Guid.Empty : Guid.Parse(guidAsString);
        }
    }
}
