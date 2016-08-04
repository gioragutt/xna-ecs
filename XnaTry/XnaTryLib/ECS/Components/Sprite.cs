using Microsoft.Xna.Framework.Graphics;

namespace XnaTryLib.ECS.Components
{
    /// <summary>
    /// The base sprite component of entities in the game
    /// </summary>
    public class Sprite : BaseComponent
    {
        /// <summary>
        /// Name of the asset you want to load
        /// </summary>
        /// <remarks>
        /// If the Texture property is not null, the asset specified will not be loaded
        /// </remarks>
        public string AssetName { get; set; }

        /// <summary>
        /// The texture to be Rendered
        /// </summary>
        public Texture2D Texture { get; set; }

        private void InitializeProperties(Texture2D texture, string assetName)
        {
            Texture = texture;
            AssetName = assetName;
        }

        /// <summary>
        /// Initializes a Sprite component with an asset name to load
        /// </summary>
        /// <param name="assetName">Name of the asset to load</param>
        /// <exception cref="System.ArgumentNullException">if assetName is empty or null</exception>
        public Sprite(string assetName)
        {
            Util.AssertStringArgumentNotNull(assetName, "assetName");
            InitializeProperties(null, assetName);
        }

        /// <summary>
        /// Initializes a Sprite component with a loaded texture
        /// </summary>
        /// <param name="texture">The texture of the sprite</param>
        /// <exception cref="System.ArgumentNullException">if texture is null</exception>
        public Sprite(Texture2D texture)
        {
            Util.AssertArgumentNotNull(texture, "texture");
            InitializeProperties(texture, null);
        }
    }
}
