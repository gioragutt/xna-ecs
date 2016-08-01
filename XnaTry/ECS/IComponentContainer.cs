namespace ECS
{
    public interface IComponentContainer
    {
        void AddComponent<TComponent>(TComponent component) where TComponent : class, IComponent;
        TComponent GetComponent<TComponent>() where TComponent : class, IComponent;
        bool HasComponent<TComponent>() where TComponent : class, IComponent;
        int Count { get; }
    }
}
