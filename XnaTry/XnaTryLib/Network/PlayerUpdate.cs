using System;
using ECS.Interfaces;
using Newtonsoft.Json;
using XnaCommonLib.ECS.Components;

namespace XnaCommonLib.Network
{
    public class PlayerUpdate
    {
        public PlayerUpdate(IComponentContainer entity)
        {
            Guid = entity.Parent.Id;
            Transform = entity.Get<Transform>();
            Attributes = entity.Get<PlayerAttributes>();
            Input = new InputData(entity.Get<DirectionalInput>());
            Velocity = entity.Get<Velocity>();
        }

        public PlayerUpdate()
        {
        }

        [JsonIgnore]
        public static readonly Predicate<IComponentContainer> IsPlayer =
            c => c.Has<Transform>() && c.Has<PlayerAttributes>() && c.Has<DirectionalInput>() && c.Has<Velocity>();

        public Guid Guid { get; set; }
        public Transform Transform { get; set;  }
        public PlayerAttributes Attributes { get; set;  }
        public InputData Input { get; set; }
        public Velocity Velocity { get; set; }
    }
}