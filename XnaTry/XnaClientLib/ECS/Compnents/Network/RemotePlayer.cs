using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;
using XnaCommonLib.Network;

namespace XnaClientLib.ECS.Compnents.Network
{
    public class RemotePlayer : NetworkPlayer
    {
        private Transform Transform { get; }
        private PlayerAttributes Attributes { get; }
        private DirectionalInput Input { get; }
        private Velocity Velocity { get; }

        public RemotePlayer(GameObject gameObject) : base(gameObject)
        {
            Transform = UpdatedObject.Transform;
            Attributes = UpdatedObject.Components.Get<PlayerAttributes>();
            Input = UpdatedObject.Components.Get<DirectionalInput>();
            Velocity = UpdatedObject.Components.Get<Velocity>();
        }

        public override void Update(PlayerUpdate update)
        {
            Transform.Update(update.Transform);
            Attributes.Update(update.Attributes);
            Velocity.Update(update.Velocity);
            Input.Update(update.Input);
        }
    }
}
