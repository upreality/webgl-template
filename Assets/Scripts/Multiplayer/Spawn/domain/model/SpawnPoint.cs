namespace Multiplayer.Spawn.domain.model
{
    public struct SpawnPoint
    {
        public int PointId;
        public int DefaultCooldown;

        public SpawnPoint(int pointId, int defaultCooldown)
        {
            this.PointId = pointId;
            this.DefaultCooldown = defaultCooldown;
        }
    }
}