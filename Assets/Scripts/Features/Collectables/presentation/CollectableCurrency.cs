using Features.Balance.domain;
using Features.Balance.presentation;
using Features.Coins.domain;
using ModestTree;
using UnityEngine;
using Utils.AutoId;
using Utils.PlayerTrigger;
using Zenject;

namespace Features.Collectables.presentation
{
    public class CollectableCurrency : PlayerTriggerBase
    {
        [Inject] private AddBalanceNavigator addBalanceNavigator;
        [Inject] private ICollectableRepository collectableRepository;
        [SerializeField] private GameObject target;
        [SerializeField] private ParticleSystem collectParticles;
        [SerializeField] private Animator animator;
        [SerializeField] private string currencyId = CurrencyType.Primary;
        [SerializeField] private string trigger = "collect";
        [SerializeField, AutoId] private string collectableId = "";

        private void Start()
        {
            if (collectableId.IsEmpty() || !collectableRepository.IsCollected(collectableId))
                return;

            gameObject.SetActive(false);
        }

        protected override void OnPlayerEntersTrigger()
        {
            animator.SetTrigger(trigger);
            collectParticles.Play();
            addBalanceNavigator.AddBalance(1, currencyType);
            if (collectableId.IsEmpty())
                return;

            collectableRepository.Collect(collectableId);
        }

        protected override void OnPlayerExitTrigger()
        {
            //Do nothing
        }
    }
}