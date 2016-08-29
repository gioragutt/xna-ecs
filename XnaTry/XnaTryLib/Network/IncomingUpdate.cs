using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace XnaCommonLib.Network
{
    public class IncomingUpdate
    {
        public IList<JObject> Broadcasts { get; set; }
        public IList<PlayerUpdate> PlayerUpdates { get; set; }
    }
}