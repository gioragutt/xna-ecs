using System;
using System.Linq;
using ECS.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using UtilsLib.Utility;
using XnaCommonLib.ECS.Components;

namespace XnaCommonLib
{
    public static class Extentions
    {
        public static Vector2 GetPosition(this MouseState state)
        {
            return new Vector2(state.X, state.Y);
        }

        public static Vector2 ToVector(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static float ToRadians(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.X, -vector.Y);
        }

        public static bool NotZero(this float number)
        {
            return Math.Abs(number) > float.Epsilon; 
        }

        public static bool IsMoving(this DirectionalInput input)
        {
            if (!Utils.NotNull(input))
                return false;
            return input.Horizontal.NotZero() || input.Vertical.NotZero(); 
        }

        public static bool KeysPressed(this KeyboardState state, params Keys[] keys)
        {
            return keys.All(state.IsKeyDown);
        }

        public static void Destroy(this IComponentContainer entity, long timeTillDestruction = 0)
        {
            Utils.AssertArgumentNotNull(entity, "entity");

            if (entity.Has<Lifespan>())
                entity.Remove<Lifespan>();

            entity.Add(new Lifespan(timeTillDestruction));
        }
    }
}
