using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaCommonLib.ECS;

namespace XnaClientLib
{
    /// <summary>
    /// Represents the camera in the game
    /// </summary>
    public class Camera
    {
        private Vector3 cameraPosition;

        /// <summary>
        /// Gets the camera matrix of the camera.
        /// </summary>
        public Matrix CameraMatrix => Matrix.CreateTranslation(-cameraPosition);

        /// <summary>
        /// The bounds of the camera;
        /// </summary>
        public Rectangle Bounds { get; set; } = new Rectangle();

        /// <summary>
        /// Updates the camera vector
        /// </summary>
        /// <param name="go">Player to set camera on</param>
        /// <param name="viewport">The screen viewport</param>
        public void UpdateCamera(GameObject go, Viewport viewport)
        {
            var shouldClamp = Bounds.Width >= viewport.Width && Bounds.Height >= viewport.Height;
            ClampToBounds(go.Transform.Position, viewport.Width, viewport.Height, shouldClamp);
        }

        private void ClampToBounds(Vector2 position, int viewportWidth, int viewportHeight, bool shouldClamp)
        {
            cameraPosition.X = position.X - viewportWidth / 2f;
            cameraPosition.Y = position.Y - viewportHeight / 2f;

            if (!shouldClamp)
                return;

            cameraPosition.X = MathHelper.Clamp(cameraPosition.X, Bounds.Left, Bounds.Right - viewportWidth);
            cameraPosition.Y = MathHelper.Clamp(cameraPosition.Y, Bounds.Top, Bounds.Bottom - viewportHeight);
        }
    }
}
 