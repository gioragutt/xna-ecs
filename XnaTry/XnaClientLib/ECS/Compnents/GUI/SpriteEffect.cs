using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Compnents.GUI
{
    public class SpriteEffect : Component, IContentRequester
    {
        public Effect Effect { get; private set; }
        public string EffectAsset { get; }
        public string AppliedPass { get; private set; }

        public SpriteEffect(string asset) { EffectAsset = asset; }

        public void LoadContent(ContentManager content)
        {
            Effect = content.Load<Effect>(EffectAsset);
        }

        public void ApplyPass(string passName)
        {
            AppliedPass = passName;
            Enabled = true;
        }

        public void ResetPass()
        {
            AppliedPass = string.Empty;
            Enabled = false;
        }
    }
}
