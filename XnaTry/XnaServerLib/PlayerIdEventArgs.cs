using System;

namespace XnaServerLib
{
    public class PlayerIdEventArgs : EventArgs
    {
        public Guid Id { get; set; }
    }
}