using System.IO;

namespace XnaCommonLib.ECS.Components
{
    public class PlayerAttributes : Component, ISharedComponent
    {
        public string Name { get; set; }
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public TeamData Team { get; set; } = new TeamData();

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
            Util.WriteString(writer, Name);
            writer.Write(MaxHealth);
            writer.Write(Health);
            Team.Write(writer);
        }

        public void Read(BinaryReader reader)
        {
            Name = Util.ReadString(reader);
            MaxHealth = reader.ReadSingle();
            Health = reader.ReadSingle();
            Team.Read(reader);
        }
    }
}
