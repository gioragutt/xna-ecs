using ECS.Interfaces;

namespace XnaTryLib.ECS.Components
{
    public abstract class Component : IComponent
    {
        public IComponentContainer Container { get; set; }
        public bool Enabled { get; set; }

        protected Component(bool enabled = true)
        {
            Enabled = enabled;
        }

        public static bool IsEnabled(Component comp) { return comp != null && comp.Enabled; }
    }
}
