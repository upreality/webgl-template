using System;
using Features.Balance.domain;
using UnityEngine;
using Utils.AutoId;

namespace Features.Purchases.data.model
{
    [Serializable]
    public class PurchaseEntity
    {
        [AutoId]
        public string id;
        public string ruName;
        public string ruDescription;
        public string enName;
        public string enDescription;
        public int currencyCost = 0;
        public string currencyId = CurrencyType.Primary;
        public int rewardedVideoCount = 0;
        public Sprite image;
    }
}