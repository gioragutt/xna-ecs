using System;

namespace XnaClientLib.ECS.Linkers
{
    /// <summary>
    /// Create a linker between two components, and use a lambda to for the link operation
    /// </summary>
    /// <typeparam name="T1">First component type</typeparam>
    /// <typeparam name="T2">Second component type</typeparam>
    public class ActionLinker<T1, T2> : Linker<T1, T2>
    {
        private Action<T1, T2> LinkMethod { get; }

        /// <summary>
        /// Initializes the linker with the two components and the linkd method
        /// </summary>
        /// <param name="first">First component in the linkage</param>
        /// <param name="second">Second component in the linkage</param>
        /// <param name="linkMethod">The linkage method, recieving both components and updating their state</param>
        public ActionLinker(T1 first, T2 second, Action<T1, T2> linkMethod) : base(first, second)
        {
            LinkMethod = linkMethod;
        }

        public override void Link() { LinkMethod(First, Second); }
    }
}
