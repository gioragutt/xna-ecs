namespace UtilsLib.Consts
{
    public static partial class Constants
    {
        // ReSharper disable once InconsistentNaming
        public static class GUI
        {
            public const int DebugPrintInitialX = 10;
            public const int DebugPrintInitialY = 10;
            public const int DebugPrintSpacing = 0;

            public const int MinimapRatio = 10;
            public const float MinimapSize = MinimapRatio / 100f;

            public static class DrawOrder
            {
                public const int Map = 0;
                public const int Player = 1;
                public const int Minimap = 2;
            }
        }
    }
}