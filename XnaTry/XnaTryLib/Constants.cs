﻿using Microsoft.Xna.Framework;

namespace XnaCommonLib
{
    public static class Constants
    {
        public const float DefaultScale = 1f;
        public const float DefaultRotation = 0f;

        public const int DebugPrintInitialX = 10;
        public const int DebugPrintInitialY = 10;
        public const int DebugPrintSpacing = 0;

        public const float FullPositiveInput = 1;
        public const float FullNegativeInput = -1;

        public const float MaxRotation = MathHelper.TwoPi;

        public const int UpdateThreadSleepTime = 1000 / 100;


        public static class Assets
        {
            public const string PlayerHealthBarAsset = "Player/GUI/HealthBar";
            public const string PlayerNameFontAsset = "Player/Fonts/NameFont";
        }
    }
}