using System.Collections.Generic;
using ECS.Interfaces;

namespace ECS.BaseTypes
{
    public class SystemContainer : TypedContainer<ISystem>, ISystemContainer
    {
        public IEnumerable<ISystem> GetAll() { return Values; }
    }
}
