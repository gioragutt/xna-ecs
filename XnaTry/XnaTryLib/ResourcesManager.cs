using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace XnaTryLib
{
    /// <summary>
    /// Smart content loading manager
    /// </summary>
    public class ResourcesManager : IResourcesManager
    {
        private ContentManager Content { get; set; }

        private Queue<IContentRequeser> ContentRequesters { get; }

        /// <summary>
        /// Initialize ResourceManagers properties
        /// </summary>
        public ResourcesManager()
        {
            ContentRequesters = new Queue<IContentRequeser>();
        }

        /// <summary>
        /// Set the ResourceManager's ContentManager after it has been initialized
        /// </summary>
        /// <param name="contentManager">A ContentManager instance</param>
        public void SetContentManager(ContentManager contentManager)
        {
            Content = contentManager;
        }

        /// <summary>
        /// Registers a content requester to load content
        /// </summary>
        /// <param name="contentRequeser">A component that derived from IContentRequester</param>
        public T Register<T>(T contentRequeser) where T : IContentRequeser
        {
            Util.AssertArgumentNotNull(contentRequeser, "contentRequeser");
            ContentRequesters.Enqueue(contentRequeser);
            return contentRequeser;
        }

        /// <summary>
        /// Loads content of all currently register IContentRequesters
        /// </summary>
        public void LoadContent()
        {
            while (ContentRequesters.Count > 0)
            {
                ContentRequesters.Dequeue().LoadContent(Content);
            }
        }
    }
}
