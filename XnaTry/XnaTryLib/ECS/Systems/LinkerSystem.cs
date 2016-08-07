using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;
using XnaTryLib.ECS.Components;

namespace XnaTryLib.ECS.Systems
{
    public class LinkerSystem : BaseSystem
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
