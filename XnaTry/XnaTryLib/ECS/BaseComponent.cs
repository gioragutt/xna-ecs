using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECS.Interfaces;

namespace XnaTryLib.ECS
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
