using System;

namespace ECS.Interfaces
{
    public interface IEntity
    {
        /// <summary>
        /// The only member an entity should have is an ID, to diffrentiate it from other entities
        /// </summary>
        Guid Id { get; set; }
    }
}
