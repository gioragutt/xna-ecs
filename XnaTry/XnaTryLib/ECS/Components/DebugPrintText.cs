using System;
using Microsoft.Xna.Framework;

namespace XnaTryLib.ECS.Components
{
    public class DebugPrintText : BaseComponent
    {
        public object PrintValue { get; set; }
        public Func<string> PrintFunc { get; set; } 
        public Color Color { get; set; } = Color.Blue;
    }
}
