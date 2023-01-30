namespace Multiplayer.Spawn.domain.model
{
    public struct SpawnEvent
    {
        public int PointId;
        public int EventCooldown;

        public SpawnEvent(int pointId, int eventCooldown)
        {
            this.PointId = pointId;
            this.EventCooldown = eventCooldown;
        }

        public static SpawnEvent FromPointDefault(SpawnPoint point) => new(point.PointId, point.DefaultCooldown);
    }
}