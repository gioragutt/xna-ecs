using System;
using Microsoft.Xna.Framework;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Compnents
{
    public class DebugPrintText : Component
    {
        public object PrintValue { get; set; }
        public Func<string> PrintFunc { get; set; } 
        public Color Color { get; set; } = Color.Blue;
    }
}
