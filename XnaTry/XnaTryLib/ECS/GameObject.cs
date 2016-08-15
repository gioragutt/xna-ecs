using System;
using ECS.BaseTypes;
using ECS.Interfaces;
using XnaCommonLib.ECS.Components;

namespace XnaCommonLib.ECS
{
    public class GameObject
    {
        public IComponentContainer Components { get; }
        public IEntity Entity { get; }
        public Transform Transform { get; }

        public GameObject(Guid id)
        {
            Entity = new Entity(id);
            Components = new ComponentContainer(Entity);
            Transform = new Transform();
            Components.Add(Transform);

        }
    }
}
