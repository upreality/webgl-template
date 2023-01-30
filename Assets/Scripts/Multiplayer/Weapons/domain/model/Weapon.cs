namespace Multiplayer.Weapons.domain.model
{
    public class Weapon
    {
        public readonly long ID;
        public readonly string Name;
        public readonly int AmmoCapacity;
        public readonly int PerMinuteRate;
        public readonly DamageType Type;

        public Weapon(long id, string name, int ammoCapacity, int perMinuteRate, DamageType type)
        {
            ID = id;
            Name = name;
            AmmoCapacity = ammoCapacity;
            Type = type;
            PerMinuteRate = perMinuteRate;
        }

        public enum DamageType
        {
            Melee,
            Ranged
        }
    }
}