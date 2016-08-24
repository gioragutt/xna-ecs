using System;
using Newtonsoft.Json.Linq;

namespace EMS
{
    public class ClientCallbackPair
    {
        public IEmsClient Client
        {
            get; set;
        }
        public Action<JObject> Callback
        {
            get; set;
        }
    }
}