using Newtonsoft.Json.Linq;
using UtilsLib.Consts;
using UtilsLib.Utility;

namespace UtilsLib
{
    /// <summary>
    /// Helps construct an JObject to broadcast
    /// </summary>
    public class MessageBuilder
    {
        /// <summary>
        /// The object manipulated in the builder
        /// </summary>
        private JObject ConstructedObject { get; }

        /// <summary>
        /// Initializes a new MessageBuilder with a given JObject
        /// </summary>
        /// <param name="constructedObject"></param>
        private MessageBuilder(JObject constructedObject)
        {
            ConstructedObject = constructedObject;
        }

        /// <summary>
        /// Create a new MessageBuilder for an empty message
        /// </summary>
        /// <param name="messsageName">The name of the message</param>
        /// <returns>The created MessageBuilder</returns>
        public static MessageBuilder Create(string messsageName)
        {
            return new MessageBuilder(new JObject()).Add(Constants.Fields.MessageName, messsageName);
        }

        /// <summary>
        /// Create a new MessageBuilder for an empty message
        /// </summary>
        /// <param name="jObject">The message to build</param>
        /// <returns>The created MessageBuilder</returns>
        public static MessageBuilder Create(JObject jObject)
        {
            return new MessageBuilder(jObject);
        }

        /// <summary>
        /// Addes a property to the message
        /// </summary>
        /// <typeparam name="T">Type of the value to add</typeparam>
        /// <param name="propName">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <param name="replace">Indicates if should replace the property if one exsits</param>
        /// <returns>The MessageBuilder object</returns>
        public MessageBuilder Add<T>(string propName, T value, bool replace = false)
        {
            Utils.AssertStringArgumentNotNull(propName, "propName");
            Utils.AssertArgumentNotNull(value, "value");

            if (replace)
                RemoveProp(propName);

            if (!ConstructedObject.HasProp(propName))
                ConstructedObject.Add(propName, new JValue(value));
            return this;
        }

        /// <summary>
        /// Changes the name of the message
        /// </summary>
        /// <param name="name">New name of the message</param>
        /// <returns>The MessageBuilder object</returns>
        public MessageBuilder ChangeName(string name)
        {
            Add(Constants.Fields.MessageName, name, true);

            return this;
        }

        /// <summary>
        /// Removes a property by a given name if exists
        /// </summary>
        /// <param name="propName">Name of the property to remove</param>
        /// <returns>The MessageBuilder object</returns>
        public MessageBuilder RemoveProp(string propName)
        {
            if (ConstructedObject.GetValue(propName) != null)
                ConstructedObject.Remove(propName);

            return this;
        }

        /// <summary>
        /// Gets the Message constructed by the builder
        /// </summary>
        /// <returns>The constructed message</returns>
        public JObject Get()
        {
            return ConstructedObject;
        }
    }
}
