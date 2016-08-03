using ECS.Interfaces;

namespace ECS.BaseTypes
{
    public class ComponentContainer : TypedContainer<IComponent>, IComponentContainer
    {
        public override void Add<TDerived>(TDerived instance)
        {
            instance.Container = this;
            base.Add(instance);
        }
    }
}
