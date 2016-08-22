﻿using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;

namespace ECS.BaseTypes
{
    public class EntityPool : IEntityPool
    {
        protected readonly Dictionary<IEntity, IComponentContainer> entities;

        public EntityPool()
        {
            entities = new Dictionary<IEntity, IComponentContainer>();
        }

        public EntityPool(EntityPool other)
        {
            entities = other.entities;
        }

        static void AssertParameterNotNull(object entity, string name)
        {
            if (entity == null)
                throw new ArgumentNullException(name);
        }

        public ICollection<TComponent> GetAllOf<TComponent>() where TComponent : class, IComponent
        {
            try
            {
                return
                    entities.Values.Where(components => components.Has<TComponent>()).SelectMany(
                        components => components.GetAllOf<TComponent>()).ToList();
            }
            catch
            {
                return new List<TComponent>();
            }
        }

        public IEnumerable<IComponentContainer> AllThat(Predicate<IComponentContainer> predicate)
        {
            return predicate == null ? default(IEnumerable<IComponentContainer>) :
                entities.Values.Where(c => predicate(c));
        }

        public bool Exists(IEntity entity)
        {
            AssertParameterNotNull(entity, "entity");
            return entities.ContainsKey(entity);
        }

        public IComponentContainer GetComponents(IEntity entity)
        {
            AssertParameterNotNull(entity, "entity");
            IComponentContainer componentContainer;
            return entities.TryGetValue(entity, out componentContainer) ? componentContainer : null;
        }

        public void Add(IEntity entity, IComponentContainer container)
        {
            AssertParameterNotNull(entity, "entity");
            AssertParameterNotNull(container, "container");

            entity.Parent = this;
            entities.Add(entity, container);
        }

        public void Add(IEntity entity)
        {
            AssertParameterNotNull(entity, "entity");

            if (Exists(entity))
                return;

            Add(entity, new ComponentContainer(entity));
        }

        public int Count
        {
            get
            {
                return entities.Count;
            }
        }

        public void Remove(IEntity entity)
        {
            if (entities.ContainsKey(entity))
                entities.Remove(entity);
        }

        public IEnumerable<IEntity> AllEntities
        {
            get
            {
                return entities.Keys.ToList();
            }
        }
    }
}
