using Multiplayer.Health.domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace Multiplayer.Health.presentation.UI
{
    public class PlayerHealthTransformBar : MonoBehaviour
    {
        [Inject] private RelativeHealthUseCase relativeHealthUseCase;
        [SerializeField] private Transform target;
        [SerializeField] private string playerId;

        private void Start()
        {
            relativeHealthUseCase
                .GetRelativeHealthFlow(playerId)
                .Subscribe(ApplyHealth)
                .AddTo(this);
        }
        private void ApplyHealth(float relativeHealth) => target.localScale = new Vector3(relativeHealth, 1, 1);
    }
}