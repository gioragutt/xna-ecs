using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;

namespace ECS.BaseTypes
{
    public class TypedContainer<TBase> : ConcurrentDictionary<Type, TBase>, ITypedContainer<TBase>
    {
        protected static bool ImplementsType<TDerived>(Type otherType)
        {
            return ImplementsType(typeof(TDerived), otherType);
        }

        protected static bool ImplementsType(Type myType, Type otherType)
        {
            return myType.IsAssignableFrom(otherType);
        }

        public virtual void Add<TDerived>(TDerived instance) where TDerived : class, TBase
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            // No need to check if key already exists since TryAdd would simply return false
            // If it does and we are satisfied with it, since it removes the need to call Has before adding
            TryAdd(typeof(TDerived), instance);
        }

        private IList<TDerived> AllDerivedOf<TDerived>() where TDerived : class, TBase
        {
            return Keys.Where(ImplementsType<TDerived>).Select(type => this[type] as TDerived).ToList();
        }

        public TDerived Get<TDerived>() where TDerived : class, TBase
        {
            TBase instance;
            if (TryGetValue(typeof (TDerived), out instance))
                return (TDerived)instance;
            return AllDerivedOf<TDerived>().FirstOrDefault();
        }

        public IList<TDerived> GetAllOf<TDerived>() where TDerived : class, TBase
        {
            return new List<TDerived>(AllDerivedOf<TDerived>());
        }

        public bool Has<TDerived>() where TDerived : class, TBase
        {
            return AllDerivedOf<TDerived>().ToArray().Length > 0;
        }

        public void Remove<TDerived>() where TDerived : class, TBase
        {
            Remove(typeof (TDerived));
        }

        public void RemoveAllOf<TDerived>() where TDerived : class, TBase
        {
            RemoveAllOf(typeof(TDerived));
        }

        public void Remove(Type type)
        {
            TBase outVal;
            TryRemove(type, out outVal);
        }

        public void RemoveAllOf(Type type)
        {
            var allOfTDerived = Keys.Where(t => ImplementsType(type, t)).ToList();
            var count = allOfTDerived.Count;
            for (var i = 0; i < count; i++)
            {
                Remove(allOfTDerived[i]);
            }
        }
    }
}
