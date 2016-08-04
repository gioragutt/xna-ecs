using System;
using ECS.BaseTypes;
using ECS.Interfaces;

namespace XnaTryLib.ECS
{
    public class GameObject
    {
        public IComponentContainer Components { get; }
        public IEntity Entity { get; }
        public Transform Transform { get; }

        public GameObject()
        {
            Components = new ComponentContainer();
            Entity = new Entity(Guid.NewGuid());
            Transform = new Transform();
            Components.Add(Transform);
        }
    }
}
