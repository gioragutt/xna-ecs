using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;

namespace ECS.BaseTypes
{
    public class TypedContainer<TBase> : Dictionary<Type, TBase>, ITypedContainer<TBase>
    {
        protected static bool ImplementsType<TDerived>(Type otherType)
        {
            return typeof(TDerived).IsAssignableFrom(otherType);
        }

        public virtual void Add<TDerived>(TDerived instance) where TDerived : class, TBase
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            if (Has<TDerived>())
                return;

            Add(typeof(TDerived), instance);
        }

        private IEnumerable<TDerived> AllDerivedOf<TDerived>() where TDerived : class, TBase
        {
            return Keys.Where(ImplementsType<TDerived>).Select(type => this[type] as TDerived);
        }

        public TDerived Get<TDerived>() where TDerived : class, TBase
        {
            TBase instance;
            if (TryGetValue(typeof (TDerived), out instance))
                return (TDerived)instance;
            return AllDerivedOf<TDerived>().FirstOrDefault();
        }

        public ICollection<TDerived> GetAllOf<TDerived>() where TDerived : class, TBase
        {
            return new List<TDerived>(AllDerivedOf<TDerived>());
        }

        public bool Has<TDerived>() where TDerived : class, TBase
        {
            try
            {
                return AllDerivedOf<TDerived>().ToArray().Length > 0;
            }
            catch
            {
                return false;
            }
        }

        public void Remove<TDerived>() where TDerived : class, TBase
        {
            Remove(typeof (TDerived));
        }

        public void RemoveAllOf<TDerived>() where TDerived : class, TBase
        {
            var allOfTDerived = Keys.Where(ImplementsType<TDerived>).ToList();
            var count = allOfTDerived.Count;
            for (var i = 0; i < count; i++)
            {
                Remove(allOfTDerived[i]);
            }
        }
    }
}
