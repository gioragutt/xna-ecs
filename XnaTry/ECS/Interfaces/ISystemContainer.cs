using System.Collections.Generic;

namespace ECS.Interfaces
{
    public interface ISystemContainer : ITypedContainer<ISystem>
    {
        IEnumerable<ISystem> All { get; }
    }
}