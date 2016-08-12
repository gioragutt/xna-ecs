using System;
using Microsoft.Xna.Framework;

namespace XnaTryLib
{
    public struct TeamData : IEquatable<TeamData>
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public string TeamFrameTextureAsset { get; set; }

        public bool Equals(TeamData other) { return Name.Equals(other.Name); }
    }
}
