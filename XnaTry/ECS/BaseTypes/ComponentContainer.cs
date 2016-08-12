using System;
using System.Linq;
using System.Text;
using ECS.Interfaces;

namespace ECS.BaseTypes
{
    public class ComponentContainer : TypedContainer<IComponent>, IComponentContainer
    {
        public IEntity Parent { get; set; }

        public ComponentContainer(IEntity parent)
        {
            Parent = parent;
        }

        public override void Add<TDerived>(TDerived instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            instance.Container = this;
            base.Add(instance);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var amountOfKeys = Keys.Count;
            if (amountOfKeys <= 0)
                return "No Components";

            var keysAsList = Keys.ToList();

            builder.Append(keysAsList[0].Name);

            for (var i = 1; i < amountOfKeys; ++i)
            {
                builder.AppendFormat(", {0}", keysAsList[i].Name);
            }

            return builder.ToString();
        }
    }
}
