using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTryLib.ECS.Components;
using XnaTryLib.ECS.Systems;

namespace XnaTry
{
    public class LabelSystem : BaseSystem
    {
        public SpriteFont DefaultFont { get; set; }
        public SpriteBatch SpriteBatch { get; set;  }
        private static readonly Color DefaultColor = Color.Blue;

        public override void Update(ICollection<IComponentContainer> entities, long delta)
        {
            SpriteBatch.Begin();
            foreach (var entity in entities)
            {
                DrawLabel(entity);
            }
            SpriteBatch.End();
        }

        public void DrawLabel(IComponentContainer entity)
        {
            var label = entity.Get<Label>();
            if (!label.Enabled)
                return;

            var transform = entity.Get<Transform>();
            var sprite = entity.Get<Sprite>();
            var font = label.Font ?? DefaultFont;
            var color = label.Color ?? DefaultColor;
            var textSize = font.MeasureString(label.Text);

            SpriteBatch.DrawString(font, label.Text, CalculateLabelPosition(sprite, transform, textSize), color);
        }

        private static Vector2 CalculateLabelPosition(Sprite sprite, Transform transform, Vector2 textSize)
        {
            if (sprite == null)
                return Vector2.Subtract(transform.Position, Vector2.Divide(textSize, 2f));

            return new Vector2((transform.Position.X + sprite.Texture.Width - textSize.X) / 2f * transform.Scale, transform.Position.Y - textSize.Y);
        }

        public override ICollection<IComponentContainer> GetRelevant(IEntityPool pool)
        {
            return pool.AllThat(c => c.Has<Transform>() && c.Has<Label>()).ToList();
        }
    }
}
