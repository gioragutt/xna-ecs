using System.IO;

namespace XnaCommonLib.ECS.Components
{
    public interface ISharedComponent
    {
        void Write(BinaryWriter writer);
        void Read(BinaryReader reader);
    }
}
