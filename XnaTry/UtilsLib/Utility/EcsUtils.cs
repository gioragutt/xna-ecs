using System.Linq;
using ECS.Interfaces;

namespace UtilsLib.Utility
{
    public static partial class Utils
    {
        public static class Ecs
        {
            public static bool ComponentsEnabled(params IComponent[] components)
            {
                return components.All(comp => !ReferenceEquals(comp, null) && comp.Enabled);
            }
        }
    }
}
