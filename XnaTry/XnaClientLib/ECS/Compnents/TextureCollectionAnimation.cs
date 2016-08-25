using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UtilsLib.Utility;

namespace XnaClientLib.ECS.Compnents
{
    public class TextureCollectionAnimation : Animation
    {
        private int currentTextureIndex;

        /// <summary>
        /// The Texture the animation is going to animate over
        /// </summary>
        public IList<Texture2D> AnimationTextures { get; set; }

        /// <summary>
        /// Used to know when to iterate to the next texture
        /// </summary>
        private long CurrentTick { get; set; }

        /// <summary>
        /// The index of the currently shown texture
        /// </summary>
        private int CurrentTextureIndex
        {
            get
            {
                return currentTextureIndex;
            }
            set
            {
                currentTextureIndex = (value + AnimationTextures.Count) % AnimationTextures.Count;
            }
        }

        private long TimePerFrame => (long)(MsPerFrame * AnimationSpeed);

        /// <summary>
        /// List of asset names to load
        /// </summary>
        public IList<string> AssetNames { get; set; }

        /// <summary>
        /// Initializes the Animation with a predifined set of textures
        /// </summary>
        /// <param name="sprite">The sprite to animate</param>
        /// <param name="animationTextures">List of textures to animate over</param>
        /// <param name="fpsRate">Rate at which frames of the animation change</param>
        /// <exception cref="System.ArgumentNullException">animationTextures, or any of it's items, are null</exception>
        public TextureCollectionAnimation(Sprite sprite, IList<Texture2D> animationTextures, long fpsRate)
            : this(sprite, animationTextures, null, fpsRate)
        {
            Utils.AssertArgumentNotNull(animationTextures, "animationTextures");
            for (var index = 0; index < animationTextures.Count; index++)
                Utils.AssertArgumentNotNull(animationTextures[index], string.Format("Item {0} of animationTextures", index));
        }

        /// <summary>
        /// Initializes the Animation with a list of asset names
        /// </summary>
        /// <param name="sprite">The sprite to animate</param>
        /// <param name="assetNames">List of assets to be loaded before animation</param>
        /// <param name="fpsRate">Rate at which frames of the animation change</param>
        /// <exception cref="System.ArgumentNullException">animationTextures, or any of it's items, are null</exception>
        public TextureCollectionAnimation(Sprite sprite, IList<string> assetNames, long fpsRate)
            : this(sprite, null, assetNames, fpsRate)
        {
            Utils.AssertArgumentNotNull(assetNames, "assetNames");
            for (var index = 0; index < assetNames.Count; index++)
                Utils.AssertStringArgumentNotNull(assetNames[index], string.Format("Item {0} of assetNames", index));
        }

        /// <summary>
        /// Initializes all variables of TextureCollectionAnimation
        /// </summary>
        /// <param name="sprite">The sprite to animate</param>
        /// <param name="animationTextures">List of textures to animate over</param>
        /// <param name="assetNames">List of assets to be loaded before animation</param>
        /// <param name="msPerFrame">Rate at which frames of the animation change</param>
        protected TextureCollectionAnimation(Sprite sprite, IList<Texture2D> animationTextures, IList<string> assetNames, long msPerFrame)
            : base(sprite, msPerFrame)
        {
            AnimationTextures = animationTextures;
            AssetNames = assetNames;
            CurrentTick = 0;
            currentTextureIndex = 0;
        }

        private static bool AreAllTexturesLoaded(IList<Texture2D> textures)
        {
            return textures != null && textures.All(t => t != null);
        }

        public override void LoadContent(ContentManager content)
        {
            if (AreAllTexturesLoaded(AnimationTextures))
                return;
            AnimationTextures = new Texture2D[AssetNames.Count];
            for (var i = 0; i < AssetNames.Count; ++i)
            {
                if (AnimationTextures[i] != null)
                    continue;

                AnimationTextures[i] = content.Load<Texture2D>(AssetNames[i]);
            }
        }

        public override void Disable()
        {
            ResetToInitialFrame();
        }

        private void ResetToInitialFrame()
        {
            Enabled = false;
            CurrentTextureIndex = 0;
            CurrentTick = 0;
            Sprite.Texture = AnimationTextures[CurrentTextureIndex];
        }

        public override void Update(long delta)
        {
            if (!Enabled)
            {
                ResetToInitialFrame();
                return;
            }

            CurrentTick += delta;
            var tpr = TimePerFrame;
            if (CurrentTick >= tpr)
            {
                CurrentTextureIndex++;
                CurrentTick -= tpr;
            }
            else
                return;

            var textureToShow = AnimationTextures[CurrentTextureIndex];
            if (Sprite.Texture == textureToShow)
                return;

            Sprite.Texture = textureToShow;
        }
    }
}