namespace ECS.Interfaces
{
    public interface IComponentContainer : ITypedContainer<IComponent>
    {
        IEntity Parent { get; set; }
    }
}
