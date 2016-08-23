using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;
using XnaClientLib.ECS.Linkers;
using System;

namespace XnaClientLib.ECS.Systems
{
    public class LinkerSystem : XnaCommonLib.ECS.Systems.System
    {
        public override void Update(IList<IComponentContainer> entities, long delta)
        {
            if (entities.Count == 0)
                return;

            var linkers = entities.SelectMany(c => c.GetAllOf<Linker>()).ToList();
            linkers.ForEach(linker => linker.Link());
        }

        public override Predicate<IComponentContainer> RelevantEntities()
        {
            return c => c.Has<Linker>(); 
        }
    }
}
