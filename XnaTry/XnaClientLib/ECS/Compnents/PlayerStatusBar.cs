using ECS.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UtilsLib.Utility;
using XnaCommonLib;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Compnents
{
    public class PlayerStatusBar : GuiComponent
    {
        #region Components

        private PlayerAttributes Attributes { get; }
        private Sprite Sprite { get; }
        private Transform Transform { get; }

        #endregion

        #region Content

        private string HealthBarTextureAsset { get; }
        private string NameFontAsset { get; }

        private Texture2D FrameTexture { get; set; }
        private Texture2D HealthBarTexture { get; set; }
        private SpriteFont NameFont { get; set; }

        #endregion

        #region Constants

        private const int HealthBarHeight = 12;
        private const int NamePaddingHealthBar = -3;
        private const int HealthBarWidthPadding = 3;
        private const int HealthBarHeightPadding = -2;

        #endregion

        /// <summary>
        /// Initializes a new PlayerStatusBar
        /// </summary>
        /// <param name="entity">Components of the entity</param>
        /// <param name="healthBarTextureAsset">Asset of the health bar texture</param>
        /// <param name="nameFontAsset">Asset of the name label font</param>
        public PlayerStatusBar(IComponentContainer entity, string healthBarTextureAsset, string nameFontAsset)
        {
            Utils.AssertStringArgumentNotNull(healthBarTextureAsset, "healthBarTextureAsset");
            Utils.AssertStringArgumentNotNull(nameFontAsset, "nameFontAsset");

            Attributes = entity.Get<PlayerAttributes>();
            Sprite = entity.Get<Sprite>();
            Transform = entity.Get<Transform>();
            HealthBarTextureAsset = healthBarTextureAsset;
            NameFontAsset = nameFontAsset;
        }

        private Vector2 healthBarPaddingInFrame = Vector2.Zero;

        public override void LoadContent(ContentManager content)
        {
            FrameTexture = content.Load<Texture2D>(Attributes.Team.Frame);

            healthBarPaddingInFrame = new Vector2(HealthBarWidthPadding,
                FrameTexture.Height + HealthBarHeightPadding - HealthBarHeight);

            HealthBarTexture = content.Load<Texture2D>(HealthBarTextureAsset);
            NameFont = content.Load<SpriteFont>(NameFontAsset);
        }

        private static Rectangle CreateRectangleFromVector2(Vector2 position, Vector2 size)
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Attributes.IsDead)
            {
                DrawName(spriteBatch, Vector2.Zero);
                return;
            }

            var framePosition = DrawFrameAndGetPosition(spriteBatch);
            DrawHealthBar(spriteBatch, framePosition);
            DrawName(spriteBatch, framePosition);
        }

        private void DrawName(SpriteBatch spriteBatch, Vector2 framePosition)
        {
            var upperCaseName = Attributes.Name.ToUpper();
            var nameTextSize = NameFont.MeasureString(upperCaseName);
            Vector2 textPosition;

            if (!Attributes.IsDead)
            {
                textPosition = new Vector2(framePosition.X + (FrameTexture.Width / 2f) - (nameTextSize.X / 2f),
                    framePosition.Y - nameTextSize.Y - NamePaddingHealthBar);
            }
            else
            {
                var topCenter = GetTopCenterPointOfSprite(Sprite, Transform);
                textPosition = new Vector2(topCenter.X - nameTextSize.X / 2, topCenter.Y - nameTextSize.Y);
            }

            spriteBatch.DrawString(NameFont, upperCaseName, textPosition, Attributes.Team.Color);
        }

        private void DrawHealthBar(SpriteBatch spriteBatch, Vector2 framePosition)
        {
            var healthBarPosition = Vector2.Add(framePosition, healthBarPaddingInFrame);
            var healthBarWidth = (FrameTexture.Width - healthBarPaddingInFrame.X * 2f) *
                                 (Attributes.Health / Attributes.MaxHealth);
            var healthBarRectangle = CreateRectangleFromVector2(healthBarPosition, new Vector2(healthBarWidth, HealthBarHeight));

            spriteBatch.Draw(HealthBarTexture, healthBarRectangle, Color.White);
        }

        private Vector2 DrawFrameAndGetPosition(SpriteBatch spriteBatch)
        {
            var topCenter = GetTopCenterPointOfSprite(Sprite, Transform);
            var framePosition = topCenter - new Vector2(FrameTexture.Width / 2f, FrameTexture.Height);

            spriteBatch.Draw(FrameTexture, framePosition, Color.White);
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
    }
}
