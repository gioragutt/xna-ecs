using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaClientLib.ECS.Compnents;
using XnaCommonLib;

namespace XnaClientLib.ECS.Systems
{
    public class DebugPrintSystem : XnaCommonLib.ECS.Systems.System
    {
        public SpriteBatch SpriteBatch { get; set; }
        public SpriteFont Font { get; set; }

        public DebugPrintSystem(SpriteBatch spriteBatch, SpriteFont font, bool enabled = true) 
            : base(enabled)
        {
            SpriteBatch = spriteBatch;
            Font = font;
        }

        public override void Update(IList<IComponentContainer> entities, long delta)
        {
            SpriteBatch.Begin();
            var textPos = new Vector2(Constants.DebugPrintInitialX, Constants.DebugPrintInitialY);
            entities.Aggregate(textPos, (current, entity) => PrintDebugText(entity, current));
            SpriteBatch.End();
        }

        private Vector2 PrintDebugText(IComponentContainer entity, Vector2 textPos)
        {
            var debugPrintComp = entity.Get<DebugPrintText>();
            if (debugPrintComp.PrintValue == null && debugPrintComp.PrintFunc == null)
                return textPos;
            var text = debugPrintComp.PrintValue?.ToString() ?? debugPrintComp.PrintFunc();
            var textSize = Font.MeasureString(text);
            SpriteBatch.DrawString(Font, text, textPos, debugPrintComp.Color);
            textPos.Y += textSize.Y + Constants.DebugPrintSpacing;
            return textPos;
        }

        public override Predicate<IComponentContainer> RelevantEntities()
        {
            return c => c.Has<DebugPrintText>();
        }
    }
}
