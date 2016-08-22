using System;
using System.Collections.Generic;
using ECS.Interfaces;
using XnaClientLib.ECS.Compnents;

namespace XnaClientLib.ECS.Systems
{
    public class AnimationSystem : XnaCommonLib.ECS.Systems.System
    {
        public override void Update(IList<IComponentContainer> entities, long delta)
        {
            for (var index = 0; index < entities.Count; index++)
                HandleAnimation(entities[index], delta);
        }

        public void HandleAnimation(IComponentContainer entity, long delta)
        {
            entity.Get<Animation>().Update(delta);
        }

        public override Predicate<IComponentContainer> RelevantEntities()
        {
            return c => c.Has<Animation>(); 
        }
    }
}
