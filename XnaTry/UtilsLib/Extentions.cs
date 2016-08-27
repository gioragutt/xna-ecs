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
        public static Guid? GetGuid(this JObject jObject, string propName)
        {
            Utils.AssertArgumentNotNull(jObject, "jObject");
            Utils.AssertStringArgumentNotNull(propName, "propName");

            var guidAsString = jObject.Value<string>(propName);
            if (string.IsNullOrEmpty(guidAsString))
                return null;
            return Guid.Parse(guidAsString);
        }

        /// <summary>
        /// Checks if a JObject has a property by name
        /// </summary>
        /// <param name="jObject">The checked JObject</param>
        /// <param name="propName">Name of the property to check</param>
        /// <returns>true if jObject has prop propName; otherwise false</returns>
        public static bool HasProp(this JObject jObject, string propName)
        {
            Utils.AssertArgumentNotNull(jObject, "jObject");
            Utils.AssertStringArgumentNotNull(propName, "propName");

            JToken dummyToken;
            return jObject.TryGetValue(propName, out dummyToken);
        }

        /// <summary>
        /// Gets the value of a property
        /// </summary>
        /// <param name="jObject">A JObject containing the guid</param>
        /// <param name="propName">The name of the property</param>
        /// <param name="defaultValue">Default value if field is not found</param>
        /// <returns>A read property if exists; otherwise defaultValue</returns>
        /// <exception cref="System.ArgumentNullException">if jObject is null</exception>
        /// <exception cref="System.ArgumentNullException">if propName is null</exception>
        public static T GetProp<T>(this JObject jObject, string propName, T defaultValue)
        {
            Utils.AssertArgumentNotNull(jObject, "jObject");
            Utils.AssertStringArgumentNotNull(propName, "propName");

            JToken token;
            return jObject.TryGetValue(propName, out token) ? token.Value<T>() : defaultValue;
        }
    }
}
