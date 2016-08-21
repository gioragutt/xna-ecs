using System;
using System.IO;
using Microsoft.Xna.Framework;
using XnaCommonLib.ECS.Components;

namespace XnaCommonLib
{
    public class TeamData : IEquatable<TeamData>, ISharedComponent
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public string Frame { get; set; }

        public bool Equals(TeamData other) { return Name.Equals(other.Name); }

        public void Read(BinaryReader reader)
        {
            Name = Util.ReadString(reader);
        }

        public void Write(BinaryWriter writer)
        {
            Util.WriteString(writer, Name);
        }
    }
}
