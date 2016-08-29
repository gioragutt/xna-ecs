using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;
using XnaCommonLib.Network;

namespace XnaClientLib.ECS.Compnents.Network
{
    public abstract class NetworkPlayer : Component
    {
        public GameObject UpdatedObject { get; }

        protected NetworkPlayer(GameObject gameObject)
        {
            UpdatedObject = gameObject;
        }

        public abstract void Update(PlayerUpdate update);
    }
}