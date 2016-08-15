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
            new Transform().Read(reader);
            new PlayerAttributes().Read(reader);
            new InputData().Read(reader);
        }
    }
}
