using System.Collections.Generic;

namespace ECS.Interfaces
{
    public interface ISystemContainer : ITypedContainer<ISystem>
    {
        IList<ISystem> All { get; }
    }
}