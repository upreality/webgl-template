using System;
using Features.Purchases.domain.repositories;
using UniRx;
using Zenject;

namespace Features.Purchases.domain
{
    public class CurrencyPurchaseUseCase
    {
        [Inject] private IPurchaseRepository purchaseRepository;
        [Inject] private ICurrencyPurchaseRepository currencyPurchaseRepository;
        [Inject] private IBalanceAccessProvider balanceAccessProvider;

        public IObservable<CurrencyPurchaseResult> ExecutePurchase(string purchaseId) => currencyPurchaseRepository
            .GetPurchasedState(purchaseId)
            .Take(1)
            .SelectMany(state =>
                state ? Observable.Return(CurrencyPurchaseResult.AlreadyPurchased) : ExecuteNewPurchase(purchaseId)
            );

        private IObservable<CurrencyPurchaseResult> ExecuteNewPurchase(string purchaseId)
        {
            var cost = currencyPurchaseRepository.GetCost(purchaseId);
            var type = purchaseRepository.GetById(purchaseId).Type;
            return balanceAccessProvider
                .CanRemove(cost, type)
                .Take(1)
                .SelectMany(enoughBalance =>
                    {
                        if (!enoughBalance) return Observable.Return(CurrencyPurchaseResult.Failure);
                        return balanceAccessProvider.Remove(cost, type).Select(result =>
                            {
                                if (!result)
                                    return CurrencyPurchaseResult.Failure;

                                currencyPurchaseRepository.SetPurchased(purchaseId);
                                return CurrencyPurchaseResult.Success;
                            }
                        );
                    }
                );
        }

        public enum CurrencyPurchaseResult
        {
            Success,
            AlreadyPurchased,
            Failure
        }
    }
}