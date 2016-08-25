using System.IO;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Compnents
{
    public class LocalPlayer : NetworkPlayer
    {
        public LocalPlayer(GameObject gameObject) 
            : base(gameObject)
        {
        }

        public override void Update(BinaryReader reader)
        {
            var components = UpdatedObject.Components;

            components.Get<Transform>().Read(reader);
            components.Get<PlayerAttributes>().Read(reader);
            new InputData().Read(reader);
            components.Get<Velocity>().Read(reader);
        }
    }
}
