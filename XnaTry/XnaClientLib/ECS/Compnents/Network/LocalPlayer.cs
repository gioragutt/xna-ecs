using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;
using XnaCommonLib.Network;

namespace XnaClientLib.ECS.Compnents.Network
{
    public class LocalPlayer : NetworkPlayer
    {
        private Transform Transform { get; }
        private PlayerAttributes Attributes { get; }
        private Velocity Velocity { get; }

        public LocalPlayer(GameObject gameObject) 
            : base(gameObject)
        {
            Transform = UpdatedObject.Transform;
            Attributes = UpdatedObject.Components.Get<PlayerAttributes>();
            Velocity = UpdatedObject.Components.Get<Velocity>();
        }

        public override void Update(PlayerUpdate update)
        {
            Transform.Update(update.Transform);
            Attributes.Update(update.Attributes);
            Velocity.Update(update.Velocity);
        }
    }
}
