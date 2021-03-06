﻿using System;
using System.Collections.Generic;
using ECS.Interfaces;
using Microsoft.Xna.Framework;
using UtilsLib.Consts;
using UtilsLib.Utility;
using XnaCommonLib.ECS.Components;

namespace XnaServerLib.ECS.Systems
{
    public class MovementSystem : XnaCommonLib.ECS.Systems.System
    {
        public override void Update(IList<IComponentContainer> entities, long delta)
        {
            for (var i = 0; i < entities.Count; i++)
                UpdateEntity(entities[i], delta);
        }

        private static void UpdateEntity(IComponentContainer entity, long delta)
        {
            var transform = entity.Get<Transform>();
            var velocity = entity.Get<Velocity>();
            var input = entity.Get<DirectionalInput>();
            HandleMovement(delta, input, velocity, transform);
        }


        private static void HandleMovement(long delta, DirectionalInput input, Velocity velocity, Transform transform)
        {
            if (!Utils.Ecs.ComponentsEnabled(input, velocity))
                return;

            input.Update(delta);
            var moveVector = GetMoveVector(velocity, input, delta);
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
        private static Vector2 GetMoveVector(Velocity velocity, DirectionalInput input, long delta)
        {
            return velocity * input * (delta / Constants.Time.MillisecondsInSecond);
        }

        public override Predicate<IComponentContainer> RelevantEntities()
        {
            return c => c.Has<Transform>() && c.Has<Velocity>() && c.Has<DirectionalInput>();
        }
    }
}
