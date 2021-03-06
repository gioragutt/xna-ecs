﻿using System;
using ECS.Interfaces;

namespace ECS.BaseTypes
{
    public class Entity : IEntity, IEquatable<IEntity>
    {
        public void Dispose()
        {
            Parent?.Remove(this);
        }

        #region Equality Members

        public bool Equals(IEntity other)
        {
            return Id.Equals(other.Id);
        }

        protected bool Equals(Entity other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((Entity)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        /// <summary>
        /// The Guid assigned to this entity
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The entity pool containing this entity.
        /// Assigned to when entity is added to an entity pool
        /// </summary>
        public IEntityPool Parent { get; set; }

        public Entity(Guid id, IEntityPool parent = null)
        {
            Id = id;
            Parent = parent;
        }
    }
}
