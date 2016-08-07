﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XnaTryLib.ECS.Components;

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

        public static bool IsMoving(this DirectionalInput input)
        {
            return input.Horizontal != 0 || input.Vertical != 0; 
        }

        public static bool KeysPressed(this KeyboardState state, params Keys[] keys)
        {
            return keys.All(state.IsKeyDown);
        }
    }
}
