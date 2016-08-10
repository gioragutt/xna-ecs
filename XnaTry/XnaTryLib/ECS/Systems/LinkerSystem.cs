using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;
using XnaTryLib.ECS.Linkers;

namespace XnaTryLib.ECS.Systems
{
    public class LinkerSystem : System
    {
        public override void Update(ICollection<IComponentContainer> entities, long delta)
        {
            entities.Select(c => c.Get<Linker>()).ToList().ForEach(linker => linker.Link());
        }

        public override ICollection<IComponentContainer> GetRelevant(IEntityPool pool)
        {
            return pool.AllThat(c => c.Has<Linker>()).ToList();
        }
    }
}
