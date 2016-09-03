using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;

namespace ECS.BaseTypes
{
    public class SystemContainer : TypedContainer<ISystem>, ISystemContainer
    {
        public IList<ISystem> All => Values.ToList();
    }
}
