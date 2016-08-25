using Newtonsoft.Json.Linq;
using UtilsLib.Consts;

namespace UtilsLib.Utility
{
    public static partial class Utils
    {
        public static class Ems
        {
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

                return MessageBuilder.Create(messageName).Add(Constants.Fields.Transmitted, transmitted, true).Get();
            }
        }
    }
}
