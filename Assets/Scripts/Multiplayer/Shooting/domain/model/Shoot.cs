namespace Multiplayer.Shooting.domain.model
{
    public struct Shoot
    {
        public string ShooterID;

        public Shoot(string shooterID)
        {
            ShooterID = shooterID;
        }
    }
}