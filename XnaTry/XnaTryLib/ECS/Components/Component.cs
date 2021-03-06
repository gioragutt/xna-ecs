﻿using ECS.Interfaces;
using EMS;
using Newtonsoft.Json;

namespace XnaCommonLib.ECS.Components
{
    public abstract class Component : EmsClient, IComponent
    {
        [JsonIgnore]
        public IComponentContainer Container { get; set; }

        [JsonIgnore]
        public bool Enabled { get; set; }

        protected Component(bool enabled = true)
        {
            Enabled = enabled;
        }

        public void Dispose()
        {
            Container.Remove(GetType());
        }

        public static bool IsEnabled(Component comp) { return comp != null && comp.Enabled; }
    }
}
