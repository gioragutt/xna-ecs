using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaTryLib.ECS.Components
{
    public abstract class Linker : BaseComponent
    {
        public abstract void Link();

        protected Linker(bool enabled = true) : base(enabled)
        {
        }
    }

    public abstract class Linker<T1, T2> : Linker
    {
        protected Linker(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }

        protected T1 First { get; set; }
        protected T2 Second { get; set; }

        public override abstract void Link();
    }
}
