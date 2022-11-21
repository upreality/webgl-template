using System.Collections.Generic;
using Features.Currencies.domain;
using Features.Currencies.domain.model;
using UnityEngine;

namespace Features.Currencies.data
{
    [CreateAssetMenu(menuName = "Currency/SO Currency Repository", fileName = "SO Currency Repository", order = 0)]
    public class SOCurrencyRepository: ScriptableObject, ICurrencyRepository
    {
        [SerializeField] private List<Currency> definedCurrencies = new();
        
        public List<Currency> GetCurrencies() => definedCurrencies;

        public Currency GetCurrency(string id) => definedCurrencies.Find(currency => currency.ID == id);
    }
}