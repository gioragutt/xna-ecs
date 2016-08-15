using System.IO;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Compnents
{
    public class RemotePlayer : NetworkPlayer
    {
        private Transform Transform { get; }
        private PlayerAttributes Attributes { get; }
        private DirectionalInput Input { get; }

        public RemotePlayer(GameObject gameObject) : base(gameObject)
        {
            Transform = UpdatedObject.Transform;
            Attributes = UpdatedObject.Components.Get<PlayerAttributes>();
            Input = UpdatedObject.Components.Get<DirectionalInput>();
        }

        public override void Update(BinaryReader reader)
        {
            Transform.Read(reader);
            Attributes.Read(reader);
            Input.Read(reader);
        }
    }
}
