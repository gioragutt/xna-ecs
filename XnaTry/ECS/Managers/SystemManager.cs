using System.Linq;
using ECS.BaseTypes;
using ECS.Interfaces;

namespace ECS.Managers
{
    public class SystemManager
    {
        public ISystemContainer Systems { get; }
        public EntityPool Entities { get; }

        SystemManager(EntityPool entities)
        {
            Systems = new SystemContainer();
            Entities = entities;
        }

        void AddSystem<TComponent>(ISystem<TComponent> system) where TComponent : class, IComponent
        {
            Systems.Add(system);
        }

        void Update(long delta)
        {
            foreach (var system in Systems.GetAll().Where(s => s.Enabled))
            foreach (var system in Systems.All.Where(s => s.Enabled))
            {
                system.Update(Entities, delta);
            }
        }
    }
}
