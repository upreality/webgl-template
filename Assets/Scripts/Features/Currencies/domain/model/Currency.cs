using System;
using UnityEngine;

namespace Features.Currencies.domain.model
{
    [Serializable]
    public class Currency
    {
        public string ID;
        public string Name;
        public string Description;
        public Sprite Icon;
    }
}