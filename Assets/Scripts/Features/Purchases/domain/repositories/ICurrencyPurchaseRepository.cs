using System;
using Features.Balance.domain;

namespace Features.Purchases.domain.repositories
{
    public interface ICurrencyPurchaseRepository
    {
        int GetCost(string purchaseId);
        void SetPurchased(string purchaseId);
        IObservable<bool> GetPurchasedState(string purchaseId);
    }
}