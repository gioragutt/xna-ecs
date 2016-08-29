using Microsoft.Xna.Framework;
using XnaCommonLib.ECS.Components;

namespace XnaCommonLib
{
    public class TeamData : IUpdatable<TeamData>
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public string Frame { get; set; }

        public void Update(TeamData instance)
        {
            Name = instance.Name;
        }
    }
}
