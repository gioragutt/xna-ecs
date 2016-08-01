namespace ECS
{
    public interface IComponent
    {
        /// <summary>
        /// The component should know its container so that components can interact
        /// With other components of the entity
        /// </summary>
        IComponentContainer Container { get; }

        /// <summary>
        /// Components always have a state of being enabled or disabled
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Components need to be updated
        /// </summary>
        /// <param name="delta">Milliseconds since the last update</param>
        void Update(long delta);
    }
}
