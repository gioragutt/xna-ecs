using ECS.Interfaces;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;
using XnaTryLib.ECS.Components;

namespace XnaTryLib.ECS.Systems
{
    public class AnimationSystem : BaseSystem
    {
        public ContentManager Content { get; set; }

        public override void Update(ICollection<IComponentContainer> entities, long delta)
        {
            foreach (var entity in entities)
            {
                HandleAnimation(entity, delta);
            }
        }

        private void HandleAnimation(IComponentContainer entity, long delta)
        {
            var animation = entity.Get<Animation>();
            animation.LoadContent(Content);
            animation.Update(delta);
        }

        public override ICollection<IComponentContainer> GetRelevant(IEntityPool pool)
        {
            return pool.AllThat(c => c.Has<Animation>() && c.Has<Sprite>()).ToList();
        }
    }
}
