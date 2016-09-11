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

        /// <summary>
        /// The color of the team
        /// </summary>
        public Color Color { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of Lable
        /// </summary>
        /// <param name="textFunc">A getter for the text of the label</param>
        /// <param name="color">The color of the label</param>
        /// <param name="fontAssetName">The asset of the font of the label</param>
        /// <param name="labelPosition">The position of the label</param>
        /// <exception cref="System.ArgumentNullException">fontAssetName is null or empty</exception>
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

        public override int DrawOrder => Constants.GUI.DrawOrder.Hud;
        public override bool IsHud => true;

        #endregion
    }
}
