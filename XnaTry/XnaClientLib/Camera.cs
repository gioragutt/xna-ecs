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
        public Matrix CameraMatrix
        {
            get
            {

                return Matrix.CreateTranslation(-cameraPosition);
            }
        }

        /// <summary>
        /// Updates the camera vector
        /// </summary>
        /// <param name="go">Player to set camera on</param>
        /// <param name="viewport">The screen viewport</param>
        public void UpdateCamera(GameObject go, Viewport viewport)
        {
            var transform = go.Transform;

            cameraPosition.X = transform.Position.X - viewport.Width / 2f;
            cameraPosition.Y = transform.Position.Y - viewport.Height / 2f;
        }
    }
}
 