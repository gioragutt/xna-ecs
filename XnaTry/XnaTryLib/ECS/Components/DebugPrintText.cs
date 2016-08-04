using System;
using Microsoft.Xna.Framework;

namespace XnaTryLib.ECS.Components
{
    public class DebugPrintText : BaseComponent
    {
        public Func<string> ValueGetter { get; set; }
        public Color Color { get; set; } = Color.Blue;
    }
}
