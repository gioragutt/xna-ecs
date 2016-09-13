using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UtilsLib.Consts;
using UtilsLib.Utility;

namespace XnaClientLib.ECS.Compnents.GUI
{
    public class Label : GuiComponent
    {
        #region Fields

        private readonly string fontAsset;
        private readonly Func<string> textFunc;
        private readonly object textObject;
        private SpriteFont font;
        private readonly bool isTextFunc;

        #endregion Fields

        #region Properties

        /// <summary>
        /// The position of the label
        /// </summary>
        /// <remarks>Relative to the viewport</remarks>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The color of the team
        /// </summary>
        public Color Color { get; set; }

        #endregion

        #region Constructor

        #region Base Constructor

        /// <summary>
        /// Initializes a new instance of Label
        /// </summary>
        /// <param name="textFunction">A getter for the text of the label</param>
        /// <param name="textObj">An object to get the string from</param>
        /// <param name="color">The color of the label</param>
        /// <param name="fontAssetName">The asset of the font of the label</param>
        /// <param name="spriteFont">The SpriteFont of the label</param>
        /// <param name="labelPosition">The position of the label</param>
        private Label(Func<string> textFunction, object textObj, Color color, string fontAssetName, SpriteFont spriteFont, Vector2 labelPosition)
        {
            textFunc = textFunction;
            textObject = textObj;
            fontAsset = fontAssetName;
            Color = color;
            Position = labelPosition;
            font = spriteFont;
        }

        #endregion

        #region Text Function Constructors

        /// <summary>
        /// Initializes a new instance of Label
        /// </summary>
        /// <param name="textFunction">A getter for the text of the label</param>
        /// <param name="color">The color of the label</param>
        /// <param name="spriteFont">The SpriteFont of the label</param>
        /// <param name="labelPosition">The position of the label</param>
        public Label(Func<string> textFunction, Color color, SpriteFont spriteFont, Vector2 labelPosition)
            : this(textFunction, null, color, null, spriteFont, labelPosition)
        {
            isTextFunc = true;
        }

        /// <summary>
        /// Initializes a new instance of Label
        /// </summary>
        /// <param name="textFunction">A getter for the text of the label</param>
        /// <param name="color">The color of the label</param>
        /// <param name="fontAssetName">The asset of the font of the label</param>
        /// <param name="labelPosition">The position of the label</param>
        public Label(Func<string> textFunction, Color color, string fontAssetName, Vector2 labelPosition)
               : this(textFunction, null, color, fontAssetName, null, labelPosition)
        {
            Utils.AssertStringArgumentNotNull(fontAssetName, "fontAssetName");
            isTextFunc = true;
        }

        #endregion

        #region Text Object Constructors

        /// <summary>
        /// Initializes a new instance of Label
        /// </summary>
        /// <param name="textObj">An object to get the string from</param>
        /// <param name="color">The color of the label</param>
        /// <param name="spriteFont">The SpriteFont of the label</param>
        /// <param name="labelPosition">The position of the label</param>
        public Label(object textObj, Color color, SpriteFont spriteFont, Vector2 labelPosition)
            : this(null, textObj, color, null, spriteFont, labelPosition)
        {
            isTextFunc = false;
        }

        /// <summary>
        /// Initializes a new instance of Label
        /// </summary>
        /// <param name="textObj">An object to get the string from</param>
        /// <param name="color">The color of the label</param>
        /// <param name="fontAssetName">The asset of the font of the label</param>
        /// <param name="labelPosition">The position of the label</param>
        public Label(object textObj, Color color, string fontAssetName, Vector2 labelPosition)
            : this(null, textObj, color, fontAssetName, null, labelPosition)
        {
            Utils.AssertStringArgumentNotNull(fontAssetName, "fontAssetName");
            isTextFunc = false;
        }

        #endregion

        #endregion

        #region GuiComponent Methods

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, Text, Position, Color);
        }

        public override void LoadContent(ContentManager content)
        {
            if (font == null)
                font = content.Load<SpriteFont>(fontAsset);
        }

        public override int DrawOrder => Constants.GUI.DrawOrder.Hud;
        public override bool IsHud => true;

        #endregion

        #region Helper Methods

        private string Text => isTextFunc ? textFunc() : textObject.ToString();

        public override string ToString()
        {
            return Text;
        }

        #endregion Helper Methods
    }
}
