using System.Linq;
using ECS.BaseTypes;
using ECS.Interfaces;

namespace ECS.Managers
{
    public class SystemManager
    {
        public ISystemContainer Systems { get; }
        public IEntityPool Entities { get; }

        public SystemManager(IEntityPool entities)
        {
            Systems = new SystemContainer();
            Entities = entities;
        }

        public void AddSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            Systems.Add(system);
        }

        public void Update(long delta)
        {
            foreach (var system in Systems.All.Where(s => s.Enabled))
            {
                system.Update(Entities, delta);
            }
        }
    }
}
