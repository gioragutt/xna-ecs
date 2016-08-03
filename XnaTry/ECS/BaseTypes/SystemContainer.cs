using ECS.Interfaces;

namespace ECS.BaseTypes
{
    public class SystemContainer : TypedContainer<ISystem>, ISystemContainer
    {
    }
}
