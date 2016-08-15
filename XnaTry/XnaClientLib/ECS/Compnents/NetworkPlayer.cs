using System.IO;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Compnents
{
    public abstract class NetworkPlayer : Component
    {
        public GameObject UpdatedObject { get; }

        protected NetworkPlayer(GameObject gameObject)
        {
            UpdatedObject = gameObject;
        }

        public abstract void Update(BinaryReader reader);
    }
}