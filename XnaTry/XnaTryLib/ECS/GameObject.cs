using System;
using ECS.BaseTypes;
using ECS.Interfaces;
using XnaTryLib.ECS.Components;

namespace XnaTryLib.ECS
{
    public class GameObject
    {
        public IComponentContainer Components { get; }
        public IEntity Entity { get; }
        public Transform Transform { get; }

        public GameObject()
        {
            Entity = new Entity(Guid.NewGuid());
            Components = new ComponentContainer(Entity);
            Transform = new Transform();
            Components.Add(Transform);
        }
    }
}
