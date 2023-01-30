namespace Multiplayer.SpawnPoints.domain.model
{
    public struct ActivatePointEvent
    {
        
        public int PointId;
        public int Cooldown;

        public ActivatePointEvent(int pointId, int cooldown)
        {
            PointId = pointId;
            Cooldown = cooldown;
        }
    }
}