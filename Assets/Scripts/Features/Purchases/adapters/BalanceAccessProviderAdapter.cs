using System;
using Features.Balance.domain;
using Features.Purchases.domain;
using Features.Purchases.domain.model;
using UniRx;
using Zenject;
using static Features.Balance.domain.DecreaseBalanceUseCase;

namespace Features.Purchases.adapters
{
    public class BalanceAccessProviderAdapter : IBalanceAccessProvider
    {
        [Inject] private DecreaseBalanceUseCase decreaseBalanceUseCase;

        public IObservable<bool> CanRemove(int value, string currencyID) => 
            decreaseBalanceUseCase.GetCanDecreaseFlow(value, currencyID);

        public IObservable<bool> Remove(int value, string currencyID)
        {
            return decreaseBalanceUseCase
                .Decrease(value, currencyID)
                .Select(result => result == DecreaseBalanceResult.Success);
        }
    }
}