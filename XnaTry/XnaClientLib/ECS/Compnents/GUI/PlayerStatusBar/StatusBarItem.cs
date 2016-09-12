using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UtilsLib.Utility;

namespace XnaClientLib.ECS.Compnents.GUI.PlayerStatusBar
{
    public class StatusBarItem : IContentRequester
    {
        #region Fields

        private string asset;

        #endregion Fields

        #region Properties

        public Texture2D Texture
        {
            get; private set;
        }
        public string Asset
        {
            get
            {
                return asset;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    asset = value;
            }
        }

        public Func<float> FillPercentage
        {
            get; set;
        }

        #endregion Properties

        #region Constructor

        public StatusBarItem(string asset)
        {
            Utils.AssertStringArgumentNotNull(asset, "asset");
            Asset = asset;
        }

        #endregion

        #region IContentRequester Methods

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>(Asset);
        }

        #endregion IContentRequester Methods
    }
}