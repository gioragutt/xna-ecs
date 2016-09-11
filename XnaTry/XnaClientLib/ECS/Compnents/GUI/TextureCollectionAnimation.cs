using System.Collections.Generic;
using ECS.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UtilsLib.Utility;

namespace XnaClientLib.ECS.Compnents
{
    public class TextureCollectionAnimation : Animation
    {
        private bool texturesLoaded;
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
                if (AnimationTextures == null)
                    return;

                currentTextureIndex = (value + AnimationTextures.Count) % AnimationTextures.Count;
            }
        }

        private long TimePerFrame => (long)(MsPerFrame * AnimationSpeed);

        /// <summary>
        /// List of asset names to load
        /// </summary>
        public IList<string> AssetNames { get; set; }

        /// <summary>
        /// Initializes all variables of TextureCollectionAnimation
        /// </summary>
        /// <param name="entity">The sprite to animate</param>
        /// <param name="assetNames">List of assets to be loaded before animation</param>
        /// <param name="msPerFrame">Rate at which frames of the animation change</param>
        public TextureCollectionAnimation(IComponentContainer entity, IList<string> assetNames, long msPerFrame)
            : base(entity.Get<Sprite>(), msPerFrame)
        {
            Utils.AssertArgumentNotNull(assetNames, "assetNames");
            for (var i = 0; i < assetNames.Count; i++)
                Utils.AssertStringArgumentNotNull(assetNames[i], string.Format("Item {0} of assetNames", i));

            AnimationTextures = null;
            AssetNames = assetNames;
            CurrentTick = 0;
            currentTextureIndex = 0;
            texturesLoaded = false;
        }

        public override void LoadContent(ContentManager content)
        {
            AnimationTextures = new Texture2D[AssetNames.Count];
            for (var i = 0; i < AssetNames.Count; ++i)
                AnimationTextures[i] = content.Load<Texture2D>(AssetNames[i]);

            texturesLoaded = true;
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

            if (AnimationTextures != null)
                Sprite.Texture = AnimationTextures[CurrentTextureIndex];
        }

        public override void Update(long delta)
        {
            if (!texturesLoaded)
                return;

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