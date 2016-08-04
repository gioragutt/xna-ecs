using ECS.Interfaces;

namespace XnaTryLib.ECS.Components
{
    public abstract class BaseComponent : IComponent
    {
        public IComponentContainer Container { get; set; }
        public bool Enabled { get; set; }

        protected BaseComponent(bool enabled = true)
        {
            Enabled = enabled;
        }
    }
}
