using System;
using ECS.Interfaces;

namespace ECS.BaseTypes
{
    public class ComponentContainer : TypedContainer<IComponent>, IComponentContainer
    {
        public override void Add<TDerived>(TDerived instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            instance.Container = this;
            base.Add(instance);
        }
    }
}
