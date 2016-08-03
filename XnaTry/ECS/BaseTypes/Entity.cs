using System;
using ECS.Interfaces;

namespace ECS.BaseTypes
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
