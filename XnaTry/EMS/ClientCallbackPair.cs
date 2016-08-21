using System;

namespace EMS
{
    public class ClientCallbackPair
    {
        public IEmsClient Client
        {
            get; set;
        }
        public Action<EventMessageData> Callback
        {
            get; set;
        }
    }
}