using System.Collections.Generic;

namespace ECS.Interfaces
{
    public interface ITypedContainer<in TBase>
    {
        /// <summary>
        /// Save an instance of type (or derived type of) TDerived
        /// </summary>
        /// <typeparam name="TDerived">Type that implements interface TBase</typeparam>
        /// <param name="instance">Instance of type TDerived to save</param>
        void Add<TDerived>(TDerived instance) where TDerived : class, TBase;

        /// <summary>
        /// Retrieves the stored instance of type TDerived
        /// </summary>
        /// <typeparam name="TDerived">Type that implements interface TBase</typeparam>
        /// <returns>TDerived instance if found; first instance of type derived from
        /// TDerived; otherwise null</returns>
        TDerived Get<TDerived>() where TDerived : class, TBase;

        /// <summary>
        /// Retrieves all stored instances that are of type (or derived type of) TDerived
        /// </summary>
        /// <typeparam name="TDerived">Type that implements interface TBase</typeparam>
        /// <returns>ICollection<TDerived/> that are of type (or derived type of) TDerived</returns>
        ICollection<TDerived> GetAllOf<TDerived>() where TDerived : class, TBase;

        /// <summary>
        /// Checks if instance of type TDerived exists
        /// </summary>
        /// <typeparam name="TDerived">Type that implements interface TBase</typeparam>
        /// <returns></returns>
        bool Has<TDerived>() where TDerived : class, TBase;

        /// <summary>
        /// Count of instances in the container
        /// </summary>
        int Count { get; }
    }
}
