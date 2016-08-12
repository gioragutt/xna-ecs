using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XnaTryLib.ECS.Components
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
        /// <param name="attributes"></param>
        /// <param name="sprite"></param>
        /// <param name="transform"></param>
        /// <param name="healthBarTextureAsset"></param>
        /// <param name="nameFontAsset"></param>
        public PlayerStatusBar(PlayerAttributes attributes, Sprite sprite, Transform transform, string healthBarTextureAsset, string nameFontAsset)
        {
            Util.AssertStringArgumentNotNull(healthBarTextureAsset, "healthBarTextureAsset");
            Util.AssertStringArgumentNotNull(nameFontAsset, "nameFontAsset");

            Attributes = attributes;
            Sprite = sprite;
            Transform = transform;
            HealthBarTextureAsset = healthBarTextureAsset;
            NameFontAsset = nameFontAsset;
        }

        private Vector2 healthBarPaddingInFrame = Vector2.Zero;

        public override void LoadContent(ContentManager content)
        {
            FrameTexture = content.Load<Texture2D>(Attributes.Team.TeamFrameTextureAsset);

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
            var framePosition = DrawFrameAndGetPosition(spriteBatch);
            DrawHealthBar(spriteBatch, framePosition);
            DrawName(spriteBatch, framePosition);
        }

        private void DrawName(SpriteBatch spriteBatch, Vector2 framePosition)
        {
            var upperCaseName = Attributes.Name.ToUpper();
            var nameTextSize = NameFont.MeasureString(upperCaseName);

            var textPosition = new Vector2(framePosition.X + (FrameTexture.Width / 2f) - (nameTextSize.X / 2f),
                framePosition.Y - nameTextSize.Y - NamePaddingHealthBar);

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
