using Multiplayer.Spawn.domain.model;
using Multiplayer.Spawn.presentation.Spawner;
using Multiplayer.SpawnPoints.domain.repositories;
using UnityEngine;
using Zenject;

namespace Multiplayer.SpawnPoints.presentation
{
    public class SimpleSpawnPoints : MonoBehaviour, ISpawnPositionNavigator
    {
        [Inject] private ISpawnPointsRepository spawnPointsRepository;
        [SerializeField] private SpawnPointView[] spawnPoints;
        [SerializeField] private int defaultSpawnCooldown = 10;

        private void Awake()
        {
            for (var i = 0; i < spawnPoints.Length; i++)
            {
                var point = new SpawnPoint(i, defaultSpawnCooldown);
                spawnPointsRepository.AddSpawnPoint(point);
                spawnPoints[i].Setup(i);
            }
        }

        public Transform GetPointTransform(int pointId) => spawnPoints[pointId].transform;
    }
}