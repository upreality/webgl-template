using Multiplayer.Health.domain.repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Multiplayer.Health.presentation.UI
{
    public class CurrentPlayerHealthBar : MonoBehaviour
    {
        [Inject] private IPlayerHealthRepository healthRepository;
        [Inject] private IMaxHealthRepository maxHealthRepository;
        [SerializeField] private RectTransform bar;

        private Vector2 initialSizeDelta = Vector2.zero;

        private void Start()
        {
            initialSizeDelta = bar.sizeDelta;
            healthRepository.GetHealthFlow().Subscribe(DisplayHealth).AddTo(this);
        }

        private void DisplayHealth(int health)
        {
            var percent = ((float)health) / maxHealthRepository.GetMaxHealth();
            var newDelta = new Vector2(initialSizeDelta.x * percent, initialSizeDelta.y);
            bar.sizeDelta = newDelta;
        }
    }
}