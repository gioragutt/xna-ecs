using System.Diagnostics.CodeAnalysis;
using System.IO;
using UtilsLib;

namespace XnaCommonLib.ECS.Components
{
    public class PlayerAttributes : Component, ISharedComponent
    {
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
                health = value;
            }
        }

        public float MaxHealth { get; set; }
        public float PreviousHealth { get; private set; }
        public TeamData Team { get; set; } = new TeamData();

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        public bool IsDead => HealthPercentage == 0.0f;

        public float HealthPercentage
        {
            get
            {
                if (MaxHealth <= 0 || Health <= 0)
                    return 0;
                return Health / MaxHealth;
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(MaxHealth);
            writer.Write(Health);
            Team.Write(writer);
        }

        public void Read(BinaryReader reader)
        {
            Name = reader.ReadString();
            MaxHealth = reader.ReadSingle();
            Health = reader.ReadSingle();
            Team.Read(reader);
        }
    }
}
