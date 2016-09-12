using System.Collections.Generic;
using ECS.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UtilsLib.Consts;
using UtilsLib.Utility;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Compnents.GUI.PlayerStatusBar
{
    public class PlayerStatusBar : GuiComponent
    {
        #region Fields

        private readonly string nameFontAsset;
        private readonly PlayerAttributes attributes;
        private readonly Sprite sprite;
        private readonly Transform transform;

        private Vector2 barPaddingInFrame = Vector2.Zero;
        private Texture2D frameTexture;
        private SpriteFont nameFont;
        
        #endregion Fields

        #region Properties

        public List<StatusBarItem> StatusBarItems { get; set; }

        #endregion Properties

        #region Constants

        private const float TotalHeightForBars = 21f;
        private const int NamePaddingAboveBars = -3;
        private const int BarWidthPadding = 3;
        private const int BarHeightPadding = -2;

        #endregion Constants

        #region Constructor

        /// <summary>
        /// Initializes a new PlayerStatusBar
        /// </summary>
        /// <param name="entity">Components of the entity</param>
        /// <param name="nameFontAsset">Asset of the name label font</param>
        public PlayerStatusBar(IComponentContainer entity, string nameFontAsset)
        {
            Utils.AssertStringArgumentNotNull(nameFontAsset, "nameFontAsset");

            StatusBarItems = new List<StatusBarItem>();
            attributes = entity.Get<PlayerAttributes>();
            sprite = entity.Get<Sprite>();
            transform = entity.Get<Transform>();
            this.nameFontAsset = nameFontAsset;
        }

        #endregion Constructor

        #region GuiComponent Methods

        public override bool IsHud => false;

        public override int DrawOrder => Constants.GUI.DrawOrder.Player;

        public override void LoadContent(ContentManager content)
        {
            frameTexture = content.Load<Texture2D>(attributes.Team.Frame);

            barPaddingInFrame = new Vector2(BarWidthPadding,
                frameTexture.Height + BarHeightPadding - TotalHeightForBars);

            StatusBarItems.ForEach(s => s.LoadContent(content));
            nameFont = content.Load<SpriteFont>(nameFontAsset);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Utils.NotNull(sprite.Texture, frameTexture))
                return;

            if (attributes.IsDead)
            {
                DrawName(spriteBatch, Vector2.Zero);
                return;
            }

            var framePosition = DrawFrameAndGetPosition(spriteBatch);
            DrawStatusBarItems(spriteBatch, framePosition);
            DrawName(spriteBatch, framePosition);
        }

        #endregion GuiComponent Methods

        #region Helper Methods

        private void DrawStatusBarItems(SpriteBatch spriteBatch, Vector2 framePosition)
        {
            float sumHeight = 0;
            var barHeight = TotalHeightForBars / StatusBarItems.Count;
            foreach (var item in StatusBarItems)
            {
                DrawBarInFrame(spriteBatch, framePosition, item.Texture, item.FillPercentage?.Invoke() ?? 0f, sumHeight, barHeight);
                sumHeight += barHeight;
            }
        }

        private void DrawName(SpriteBatch spriteBatch, Vector2 framePosition)
        {
            var upperCaseName = attributes.Name.ToUpper();
            var nameTextSize = nameFont.MeasureString(upperCaseName);
            var textPosition = GetTextPosition(framePosition, nameTextSize);

            spriteBatch.DrawString(nameFont, upperCaseName, textPosition + new Vector2(0.5f), Color.White);
            spriteBatch.DrawString(nameFont, upperCaseName, textPosition, attributes.Team.Color);
        }

        private Vector2 GetTextPosition(Vector2 framePosition, Vector2 nameTextSize)
        {
            // Draw Above Team Frame
            if (!attributes.IsDead)
            {
                return new Vector2(framePosition.X + (frameTexture.Width / 2f) - (nameTextSize.X / 2f),
                    framePosition.Y - nameTextSize.Y - NamePaddingAboveBars);
            }

            // Draw Above Sprite
            var topCenter = GetTopCenterPointOfSprite(sprite, transform);
            return new Vector2(topCenter.X - nameTextSize.X / 2, topCenter.Y - nameTextSize.Y);
        }

        private void DrawBarInFrame(SpriteBatch spriteBatch, Vector2 framePosition, Texture2D texture, float fillPercentage, float drawingHeight, float barHeight)
        {
            var healthBarPosition = Vector2.Add(Vector2.Add(framePosition, barPaddingInFrame), new Vector2(0, drawingHeight));
            var width = (frameTexture.Width - barPaddingInFrame.X * 2f) * fillPercentage;
            var rectangle = CreateRectangleFromVector2(healthBarPosition, new Vector2(width, barHeight));

            spriteBatch.Draw(texture, rectangle, Color.White);
        }

        private Vector2 DrawFrameAndGetPosition(SpriteBatch spriteBatch)
        {
            var topCenter = GetTopCenterPointOfSprite(sprite, transform);
            var framePosition = topCenter - new Vector2(frameTexture.Width / 2f, frameTexture.Height);

            spriteBatch.Draw(frameTexture, framePosition, Color.White);
            return framePosition;
        }

        private static Vector2 GetTopCenterPointOfSprite(Sprite sprite, Transform transform)
        {
            var topLeft = GetTopLeftPointOfSprite(sprite, transform);
            var spriteFactoredWidth = new Vector2(sprite.Texture.Width / 2f * transform.Scale, 0);
            var topcenter = Vector2.Add(topLeft, spriteFactoredWidth);
            return topcenter;
        }

        private static Vector2 GetTopLeftPointOfSprite(Sprite sprite, Transform transform)
        {
            return transform.Position - sprite.Origin * transform.Scale;
        }

        private static Rectangle CreateRectangleFromVector2(Vector2 position, Vector2 size)
        {
            return new Rectangle((int) position.X, (int) position.Y, (int) size.X, (int) size.Y);
        }
        
        #endregion
    }
}
