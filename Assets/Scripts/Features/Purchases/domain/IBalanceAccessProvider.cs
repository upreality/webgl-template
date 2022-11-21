using System;
using Features.Purchases.domain.model;

namespace Features.Purchases.domain
{
    public interface IBalanceAccessProvider
    {
        IObservable<bool> CanRemove(int value, string currencyID);
        IObservable<bool> Remove(int value, string currencyID);
    }
}