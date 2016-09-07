using System;
using Microsoft.Xna.Framework;

namespace XnaServerLib
{
    public class SpawningArea
    {
        private Rectangle bounds;
        private readonly Random random;

        public SpawningArea(Rectangle rectangle)
        {
            bounds = rectangle;
            random = new Random(bounds.X + bounds.Y + bounds.Width * bounds.Height);
        }

        public Vector2 RandomPositionInArea()
        {
            var x = random.Next(bounds.Left, bounds.Right);
            var y = random.Next(bounds.Top, bounds.Bottom);
            return new Vector2(x, y);
        }
    }
}