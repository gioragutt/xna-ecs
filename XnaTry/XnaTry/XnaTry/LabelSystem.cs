using System;
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

            SpriteBatch.DrawString(font, label.Text, CalculateLabelPosition(sprite, transform, textSize, label.Placement), color);
        }

        private static Vector2 FactorInLabelPlacement(Vector2 originPoint, Vector2 textSize, Sprite sprite, Transform transform, LabelPlacement placement)
        {
            switch (placement)
            {
                case LabelPlacement.TopLeft:
                    return Vector2.Subtract(originPoint, textSize);
                case LabelPlacement.TopCenter:
                    return new Vector2(originPoint.X + sprite.Texture.Width * transform.Scale / 2f - textSize.X / 2f, originPoint.Y - textSize.Y);
                case LabelPlacement.TopRight:
                    return new Vector2(originPoint.X + sprite.Texture.Width * transform.Scale, originPoint.Y - textSize.Y);
                case LabelPlacement.MiddleLeft:
                    break;
                case LabelPlacement.MiddleCenter:
                    break;
                case LabelPlacement.MiddleRight:
                    break;
                case LabelPlacement.BottomLeft:
                    break;
                case LabelPlacement.BottomCenter:
                    break;
                case LabelPlacement.BottomRight:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("placement");
            }

            return originPoint;
        }

        private static Vector2 CalculateLabelPosition(Sprite sprite, Transform transform, Vector2 textSize, LabelPlacement placement)
        {
            if (sprite == null)
                return Vector2.Subtract(transform.Position, Vector2.Divide(textSize, 2f));

            return FactorInLabelPlacement(GetTopLeftPointOfSprite(sprite, transform), textSize, sprite, transform, placement);
        }

        private static Vector2 GetTopLeftPointOfSprite(Sprite sprite, Transform transform)
        {
            return transform.Position - sprite.Origin * transform.Scale;
        }

        public override ICollection<IComponentContainer> GetRelevant(IEntityPool pool)
        {
            return pool.AllThat(c => c.Has<Transform>() && c.Has<Label>()).ToList();
        }
    }
}
