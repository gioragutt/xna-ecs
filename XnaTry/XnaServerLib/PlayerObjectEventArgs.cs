using System;
using XnaCommonLib.ECS;

namespace XnaServerLib
{
    public class PlayerObjectEventArgs : EventArgs
    {
        public GameObject GameObject { get; set; }
    }
}