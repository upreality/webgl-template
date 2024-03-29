﻿using Features.Purchases.domain.repositories;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.Purchases.presentation.ui
{
    public class RewardedVideoPurchaseItem: PurchaseItem
    {
        [Inject] private IRewardedVideoPurchaseRepository rewardedVideoPurchaseRepository;
        
        [SerializeField] private GameObject requireWatch;
        [SerializeField] private Text watchesText;

        protected override void Setup(string purchaseId, bool purchasedState)
        {
            base.Setup(purchaseId, purchasedState);

            if (purchasedState)
            {
                requireWatch.SetActive(false);
                return;
            }

            var watchesCount = rewardedVideoPurchaseRepository.GetRewardedVideoCurrentWatchesCount(purchaseId);
            var requireWatches = rewardedVideoPurchaseRepository.GetRewardedVideoCurrentWatchesCount(purchaseId);
            watchesText.text = $"{watchesCount}/{requireWatches}";
            requireWatch.SetActive(true);
        }

        public class Factory : PlaceholderFactory<RewardedVideoPurchaseItem> { }
    }
}