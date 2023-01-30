using System;
using Multiplayer.Health.domain.repositories;
using UniRx;
using Utils.Reactive;

namespace Multiplayer.Health.data.repositories
{
    public class HealthHandlersDefaultRepository : IHealthHandlersRepository
    {
        private readonly ReactiveDictionary<string, int> handlerIdToHealthMap = new();

        public int GetHealth(string handlerId) => handlerIdToHealthMap[handlerId];

        public void SetHealth(string handlerId, int health) => handlerIdToHealthMap[handlerId] = health;

        public IObservable<int> GetHealthFlow(string handlerId) => handlerIdToHealthMap
            .GetItemFlow(handlerId)
            .DistinctUntilChanged();
    }
}