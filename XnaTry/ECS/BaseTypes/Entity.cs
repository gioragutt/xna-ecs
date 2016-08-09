using System;
using ECS.Interfaces;

namespace ECS.BaseTypes
{
    public class Entity : IEntity, IEquatable<IEntity>
    {
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

        public Guid Id { get; }

        public Entity(Guid id)
        {
            Id = id;
        }
    }
}
