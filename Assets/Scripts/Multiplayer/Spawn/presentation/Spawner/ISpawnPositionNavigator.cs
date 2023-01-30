using UnityEngine;

namespace Multiplayer.Spawn.presentation.Spawner
{
    public interface ISpawnPositionNavigator
    {
        public Transform GetPointTransform(int pointId);
    }
}