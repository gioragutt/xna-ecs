using ECS.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaTryLib;
using XnaTryLib.ECS.Components;

namespace XnaTry.ECS.Components
{
    public class PlayerStatusBar : GuiComponent
    {
        private PlayerAttributes Attributes { get; set; }
        private Sprite Sprite { get; set; }
        private Transform Transform { get; set; }

        public string FrameTextureAsset { get; set; }
        public string HealthBarTextureAsset { get; set; }

        public Texture2D FrameTexture { get; set; }
        public Texture2D HealthBarTexture { get; set; }

        private const int HealthBarHeight = 12;
        private Vector2 healthBarPaddingInFrame = Vector2.Zero;

        public override void Update(IComponentContainer entity)
        {
            if (!Util.NotNull(Attributes, Sprite, Transform))
                LoadPlayerComponentsFromEntity(entity);
        }

        public override void LoadContent(ContentManager content)
        {
            if (FrameTexture == null && !string.IsNullOrEmpty(FrameTextureAsset))
            {
                FrameTexture = content.Load<Texture2D>(FrameTextureAsset);
                healthBarPaddingInFrame = new Vector2(2, FrameTexture.Height - 2 - HealthBarHeight);
            }

            if (HealthBarTexture == null && !string.IsNullOrEmpty(HealthBarTextureAsset))
                HealthBarTexture = content.Load<Texture2D>(HealthBarTextureAsset);
        }

        private void LoadPlayerComponentsFromEntity(IComponentContainer entity)
        {
            Attributes = entity.Get<PlayerAttributes>();
            Sprite = entity.Get<Sprite>();
            Transform = entity.Get<Transform>();
        }

        private static Rectangle CreateRectangleFromVector2(Vector2 position, Vector2 size)
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Util.NotNull(Attributes))
                return;

            var topCenter = GetTopCenterPointOfSprite(Sprite, Transform);
            var framePosition = topCenter - new Vector2(FrameTexture.Width / 2f, FrameTexture.Height);

            spriteBatch.Draw(FrameTexture, framePosition, Color.White);

            var healthBarPosition = Vector2.Add(framePosition, healthBarPaddingInFrame);
            var healthBarWidth = (FrameTexture.Width - healthBarPaddingInFrame.X * 2f) *
                                 (Attributes.Health / Attributes.MaxHealth);
            var healthBarRectangle = CreateRectangleFromVector2(healthBarPosition,
                new Vector2(healthBarWidth, HealthBarHeight));

            spriteBatch.Draw(HealthBarTexture, healthBarRectangle, Color.White);
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
