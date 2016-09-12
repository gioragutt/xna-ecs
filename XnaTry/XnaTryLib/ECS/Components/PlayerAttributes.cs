
using Newtonsoft.Json;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace XnaCommonLib.ECS.Components
{
    public class PlayerAttributes : Component, IUpdatable<PlayerAttributes>
    {
        #region Properties

        private float health;
        public string Name { get; set; }

        public float Health
        {
            get
            {
                return health;
            }
            set
            {
                PreviousHealth = health;
                health = value <= 0 ? 0 : value;
            }
        }

        public float MaxHealth { get; set; }

        public TeamData Team { get; set; } = new TeamData();

        #region Utility Properties

        [JsonIgnore]
        public float PreviousHealth { get; private set; }

        [JsonIgnore]
        public bool IsDead => HealthPercentage == 0.0f;

        [JsonIgnore]
        public bool JustDied => IsDead && PreviousHealth > float.Epsilon;

        [JsonIgnore]
        public float HealthPercentage
        {
            get
            {
                if (MaxHealth <= 0 || Health <= 0)
                    return 0;
                return Health / MaxHealth;
            }
        }

        #endregion

        #endregion Properties

        public void Update(PlayerAttributes instance)
        {
            Name = instance.Name;
            MaxHealth = instance.MaxHealth;
            Health = instance.Health;
            Team.Update(instance.Team);
        }
    }
}
