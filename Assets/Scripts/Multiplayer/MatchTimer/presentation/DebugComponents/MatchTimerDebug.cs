using Multiplayer.MatchTimer.domain.repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Multiplayer.MatchTimer.presentation.DebugComponents
{
    internal class MatchTimerDebug : MonoBehaviour
    {
        [SerializeField] private bool logTimer = true;
        [Inject] private IMatchTimerRepository matchTimerRepository;

        private void Awake() => matchTimerRepository
            .GetMatchTimeSecondsFlow()
            .Where(_ => logTimer)
            .Subscribe(time =>
                Debug.Log(time.ToString())
            ).AddTo(this);
    }
}