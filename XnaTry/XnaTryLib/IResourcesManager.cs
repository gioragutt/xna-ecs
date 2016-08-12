namespace XnaTryLib
{
    public interface IResourcesManager
    {
        /// <summary>
        /// Registers a content requester to load content
        /// </summary>
        /// <param name="contentRequeser">A component that derived from IContentRequester</param>
        T Register<T>(T contentRequeser) where T : IContentRequeser;
    }
}