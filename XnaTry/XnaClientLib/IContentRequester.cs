using Microsoft.Xna.Framework.Content;

namespace XnaClientLib
{
    /// <summary>
    /// Interface for all components that want to load content
    /// Through the resource manager class
    /// </summary>
    public interface IContentRequester
    {
        void LoadContent(ContentManager content);
    }
}