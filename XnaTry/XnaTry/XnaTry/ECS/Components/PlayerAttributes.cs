using Microsoft.Xna.Framework;
using XnaTryLib.ECS.Components;

namespace XnaTry.ECS.Components
{
    public class PlayerAttributes : Component
    {
        private float health;
        private float maxHealth;

        public string Name { get; set; }

        public float Health
        {
            get
            {
                return health;
            }
            set
            {
                health = MathHelper.Clamp(value, 0, MaxHealth);
            }
        }

        public float MaxHealth  
        {
            get
            {
                return maxHealth;
            }
            set
            {
                maxHealth = value < 0 ? 0 : value;
            }
        }

        public float HealthPercentage
        {
            get
            {
                if (MaxHealth <= 0 || Health <= 0)
                    return 0;
                return Health / MaxHealth;
            }
        }
    }
}
