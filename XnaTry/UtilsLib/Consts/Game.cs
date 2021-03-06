﻿using Microsoft.Xna.Framework;

namespace UtilsLib.Consts
{
    public static partial class Constants
    {
        public static class Game
        {
            public const float DefaultScale = 1f;
            public const float DefaultRotation = 0f;
            public const float MaxRotation = MathHelper.TwoPi;
            public const float FullPositiveInput = 1;
            public const float FullNegativeInput = -1;
            public const float TileScale = 0.5f;

            public const uint LoginMessageHeader = 0xdeadbeaf;
            public const uint LoginMessageFooter = 0xbeafdead;
        }
    }
}
