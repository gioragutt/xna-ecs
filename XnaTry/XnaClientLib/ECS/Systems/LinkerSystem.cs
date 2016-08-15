using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;
using XnaClientLib.ECS.Linkers;

namespace XnaClientLib.ECS.Systems
{
    public class LinkerSystem : XnaCommonLib.ECS.Systems.System
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
