using Multiplayer.SpawnPoints.domain;
using Multiplayer.SpawnPoints.domain.repositories;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Multiplayer.SpawnPoints.presentation
{
    public class SpawnPointView : MonoBehaviour
    {
        [Inject] private SpawnPointAvailableUseCase spawnPointAvailableUseCase;
        [Inject] private ISpawnPointCooldownRepository spawnPointCooldownRepository;
        [SerializeField] private UnityEvent onPointBecomeAvailable;
        [SerializeField] private UnityEvent onPointBecomeNotAvailable;
        [SerializeField] private TextMesh desc;

        public void Setup(int pointId)
        {
            spawnPointAvailableUseCase
                .GetSpawnAvailableFlow(pointId)
                .Subscribe(OnAvailabilityUpdate)
                .AddTo(this);

            desc.text = pointId.ToString();

            spawnPointCooldownRepository
                .GetCooldownFlow(pointId)
                .Subscribe(cooldown => desc.text = $"{pointId.ToString()} - {cooldown.ToString()}")
                .AddTo(this);
        }

        private void OnAvailabilityUpdate(bool available)
        {
            if (available) onPointBecomeAvailable.Invoke();
            else onPointBecomeNotAvailable.Invoke();
        }
    }
}