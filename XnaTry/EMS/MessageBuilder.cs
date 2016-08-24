using Newtonsoft.Json.Linq;

namespace EMS
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
            return new MessageBuilder(new JObject()).Add(Constants.NameField, messsageName);
        }

        public static MessageBuilder Create(JObject jObject)
        {
            return new MessageBuilder(jObject);
        }

        public MessageBuilder Add<T>(string propName, T value, bool replace = false)
        {
            EmsUtils.AssertStringArgumentNotNull(propName, "propName");
            EmsUtils.AssertArgumentNotNull(value, "value");

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
