using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UtilsLib.Consts;
using UtilsLib.Utility;

namespace XnaClientLib.ECS.Compnents
{
    public class Label : GuiComponent
    {
        #region Fields

        private readonly Vector2 position;
        private readonly string fontAsset;
        private readonly Func<string> text;
        private SpriteFont font;

        #endregion Fields

        #region Properties

        public Color Color { get; set; }

        #endregion

        #region Constructor

        public Label(Func<string> textFunc, Color color, string fontAssetName, Vector2 labelPosition)
        {
            Utils.AssertStringArgumentNotNull(fontAssetName, "fontAssetName");

            text = textFunc;
            fontAsset = fontAssetName;
            Color = color;
            position = labelPosition;
        }

        #endregion

        #region GuiComponent Methods

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text(), position, Color);
        }

        public override void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>(fontAsset);
        }

        public override int DrawOrder => Constants.GUI.DrawOrder.Minimap;
        public override bool IsHud => true;

        #endregion
    }
}
