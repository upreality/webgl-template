using Features.Purchases.adapters;
using Features.Purchases.domain;
using Features.Purchases.presentation.ui;
using UnityEngine;
using Zenject;

namespace Features.Purchases._di
{
    public class PurchasesPresentationInstaller : MonoInstaller
    {
        [SerializeField] private CoinsPurchaseItem coinsPurchaseItemPrefab;
        [SerializeField] private RewardedVideoPurchaseItem rewardedVideoPurchaseItemPrefab;

        public override void InstallBindings()
        {
            //Presentation
            //UI
            Container
                .Bind<PurchaseItem.IPurchaseItemController>()
                .To<DefaultPurchaseItemController>()
                .FromNewComponentOnNewGameObject().AsTransient();
            //Item Factories
            Container
                .BindFactory<CoinsPurchaseItem, CoinsPurchaseItem.Factory>()
                .FromComponentInNewPrefab(coinsPurchaseItemPrefab);
            Container
                .BindFactory<RewardedVideoPurchaseItem, RewardedVideoPurchaseItem.Factory>()
                .FromComponentInNewPrefab(rewardedVideoPurchaseItemPrefab);
            Container
                .Bind<IPurchaseItemFactory>()
                .To<DefaultPurchaseItemFactory>()
                .AsSingle();
        }
    }
}