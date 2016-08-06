using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XnaTryLib
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
    }
}
