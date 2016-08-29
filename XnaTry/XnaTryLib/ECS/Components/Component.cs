using ECS.Interfaces;
using Newtonsoft.Json;

namespace XnaCommonLib.ECS.Components
{
    public abstract class Component : IComponent
    {
        [JsonIgnore]
        public IComponentContainer Container { get; set; }

        [JsonIgnore]
        public bool Enabled { get; set; }

        protected Component(bool enabled = true)
        {
            Enabled = enabled;
        }

        public static bool IsEnabled(Component comp) { return comp != null && comp.Enabled; }
    }
}
