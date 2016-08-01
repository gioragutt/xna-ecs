using System;

namespace ECS
{
    public class Entity : IEntity
    {
        public Guid Id { get; set; }

        public Entity(Guid id)
        {
            Id = id;
        }
    }
}
