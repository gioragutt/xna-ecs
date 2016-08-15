using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Linkers
{
    public abstract class Linker : Component
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
