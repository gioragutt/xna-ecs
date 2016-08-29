namespace XnaCommonLib.ECS.Components
{
    public interface IUpdatable<in T>
    {
        void Update(T instance);
    }
}