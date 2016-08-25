using Newtonsoft.Json.Linq;
using UtilsLib.Consts;
using UtilsLib.Utility;

namespace UtilsLib
{
    public class MessageBuilder
    {
        private JObject ConstructedObject { get; }

        private MessageBuilder(JObject constructedObject)
        {
            ConstructedObject = constructedObject;
        }
        
        public static MessageBuilder Create(string messsageName)
        {
            return new MessageBuilder(new JObject()).Add(Constants.Fields.MessageName, messsageName);
        }

        public static MessageBuilder Create(JObject jObject)
        {
            return new MessageBuilder(jObject);
        }

        public MessageBuilder Add<T>(string propName, T value, bool replace = false)
        {
            Utils.AssertStringArgumentNotNull(propName, "propName");
            Utils.AssertArgumentNotNull(value, "value");

            if (replace)
                RemoveProp(propName);

            ConstructedObject.Add(propName, new JValue(value));
            return this;
        }

        private void RemoveProp(string propName)
        {
            if (ConstructedObject.GetValue(propName) != null)
                ConstructedObject.Remove(propName);
        }

        public JObject Get()
        {
            return ConstructedObject;
        }
    }
}
