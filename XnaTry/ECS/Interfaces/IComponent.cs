﻿namespace ECS.Interfaces
{
    public interface IComponent
    {
        /// <summary>
        /// The component should know its container so that components can interact
        /// With other components of the entity
        /// </summary>
        IComponentContainer Container { get; set; }

        /// <summary>
        /// Components always have a state of being enabled or disabled
        /// </summary>
        bool Enabled { get; set; }
    }
}
