using Features.Balance.domain;
using Features.Currencies.presentation;
using UnityEngine;
using Zenject;

namespace Features.Balance.presentation
{
    public class AddBalanceHandler : MonoBehaviour
    {
        [SerializeField] private int amount;
        [SerializeField] private NCurrencyType currencyType;
        [Inject] private AddBalanceNavigator balanceNavigator;

        public void AddBalance()
        {
            if (balanceNavigator == null) return;
            balanceNavigator.AddBalance(amount, currencyType.ID);
        }
    }
}