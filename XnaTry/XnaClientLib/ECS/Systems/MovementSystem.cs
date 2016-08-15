using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;
using Microsoft.Xna.Framework;
using XnaClientLib.ECS.Compnents;
using XnaCommonLib;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Systems
{
    public class MovementSystem : XnaCommonLib.ECS.Systems.System
    {
        public override void Update(ICollection<IComponentContainer> entities, long delta)
        {
            foreach (var entity in entities)
            {
                UpdateEntity(entity, delta);
            }
        }

        private static void UpdateEntity(IComponentContainer entity, long delta)
        {
            var transform = entity.Get<Transform>();
            var velocity = entity.Get<Velocity>();
            var input = entity.Get<DirectionalInput>();
            var rotateToMouse = entity.Get<RotateToMouse>();
            HandleMovement(delta, input, velocity, transform);
            HandleRotationToMouse(rotateToMouse, transform);
        }

        private static void HandleRotationToMouse(RotateToMouse rotateToMouse, Transform transform)
        {
            if (!Util.ComponentsEnabled(rotateToMouse))
                return;

            var mousePosition = rotateToMouse.MousePosition;
            transform.RotateTo(mousePosition);
        }

        private static void HandleMovement(long delta, DirectionalInput input, Velocity velocity, Transform transform)
        {
            if (!Util.ComponentsEnabled(input, velocity))
                return;

            input.Update(delta);
            var moveVector = GetMoveVector(velocity, input);
            transform.MoveBy(moveVector);
        }

        /// <summary>
        /// This is a debug part of the system
        /// Each entity that's ought to move, should have it's movement vector
        /// Updated by an external source - an input system. CaYn be like below,
        /// using keyboard state, or even through AI or Network updates
        /// </summary>
        /// <param name="velocity">Velocity component </param>
        /// <param name="input">The source of input</param>
        /// <returns>Movement vector for the update</returns>
        private static Vector2 GetMoveVector(Velocity velocity, DirectionalInput input)
        {
            var moveVector = Vector2.Zero;
            moveVector.X = velocity.X * input.Horizontal;
            moveVector.Y = velocity.Y * input.Vertical;
            return moveVector;
        }

        public override ICollection<IComponentContainer> GetRelevant(IEntityPool pool)
        {
            return pool.AllThat(c => c.Has<Transform>() &&
                                     ((c.Has<Velocity>() && c.Has<DirectionalInput>()) ||
                                      c.Has<RotateToMouse>())).ToList();
        }
    }
}
