using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;
using XnaClientLib.ECS.Compnents;

namespace XnaClientLib.ECS.Systems
{
    public class AnimationSystem : XnaCommonLib.ECS.Systems.System
    {
        public override void Update(ICollection<IComponentContainer> entities, long delta)
        {
            foreach (var entity in entities)
            {
                HandleAnimation(entity, delta);
            }
        }

        public void HandleAnimation(IComponentContainer entity, long delta)
        {
            entity.Get<Animation>().Update(delta);
        }

        public override ICollection<IComponentContainer> GetRelevant(IEntityPool pool)
        {
            return pool.AllThat(c => c.Has<Animation>()).ToList();
        }
    }
}
