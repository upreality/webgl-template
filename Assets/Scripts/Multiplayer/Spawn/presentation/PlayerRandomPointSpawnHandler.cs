using UnityEngine;

namespace Multiplayer.Spawn.presentation
{
    public class PlayerRandomPointSpawnHandler : MonoBehaviour
    {
        // [Inject] private SpawnPlayerAvailableUseCase spawnPlayerAvailableUseCase;
        // [Inject] private RandomOrFirstAvailableSpawnPointUseCase randomOrFirstAvailableSpawnPointUseCase;
        // [Inject] private SpawnCurrentPlayerUseCase spawnCurrentPlayerUseCase;
        //
        // private void Awake() => spawnPlayerAvailableUseCase
        //     .GetSpawnAvailableFlow()
        //     .Do(spawnAvailable => Debug.Log("Spawn Avaliable: " + spawnAvailable))
        //     .Select(GetSpawnPointId)
        //     .Switch()
        //     // .Delay(TimeSpan.FromSeconds(0.5))
        //     .Subscribe(TryAutoSpawn)
        //     .AddTo(this);
        //
        // private IObservable<int> GetSpawnPointId(bool spawnAvailable) => spawnAvailable
        //     ? randomOrFirstAvailableSpawnPointUseCase.GetSpawnPointId()
        //     : Observable.Empty<int>();
        //
        // private void TryAutoSpawn(int pointId) => spawnCurrentPlayerUseCase.Spawn(pointId);
    }
} 