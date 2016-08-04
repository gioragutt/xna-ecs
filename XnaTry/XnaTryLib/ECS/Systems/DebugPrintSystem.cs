﻿using ECS.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using XnaTryLib.ECS.Components;

namespace XnaTryLib.ECS.Systems
{
    public class DebugPrintSystem : BaseSystem
    {
        public SpriteBatch SpriteBatch { get; set; }
        public SpriteFont Font { get; set; }

        public DebugPrintSystem() : base(true)
        {
            
        }

        public override void Update(ICollection<IComponentContainer> entities, long delta)
        {
            SpriteBatch.Begin();
            var textPos = new Vector2(Constants.DebugPrintInitialX, Constants.DebugPrintInitialY);
            entities.Aggregate(textPos, (current, entity) => PrintDebugText(entity, current));
            SpriteBatch.End();
        }

        private Vector2 PrintDebugText(IComponentContainer entity, Vector2 textPos)
        {
            var debugPrintComp = entity.Get<DebugPrintText>();
            if (debugPrintComp.ValueGetter == null)
                return textPos;
            var text = debugPrintComp.ValueGetter();
            var textSize = Font.MeasureString(text);
            SpriteBatch.DrawString(Font, text, textPos, debugPrintComp.Color);
            textPos.Y += textSize.Y + Constants.DebugPrintSpacing;
            return textPos;
        }

        public override ICollection<IComponentContainer> GetRelevant(IEntityPool pool)
        {
            return pool.AllThat(c => c.Has<DebugPrintText>()).ToList();
        }
    }
}
