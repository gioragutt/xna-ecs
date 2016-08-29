using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace XnaCommonLib.Network
{
    public class OutgoingMessage
    {
        public IList<JObject> Broadcasts { get; set; }
        public PlayerUpdate PlayerUpdate { get; set; }
    }
}