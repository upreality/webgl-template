using Multiplayer.Health.domain.repositories;
using UniRx;
using Zenject;

namespace Multiplayer.Health.domain
{
    public class PlayerDeathEventUseCase
    {
        [Inject] private IPlayerHealthRepository repository;

        public ReactiveCommand GetDeathEventFlow() => repository
            .GetHealthFlow()
            .DistinctUntilChanged()
            .Select(health => health == 0)
            .ToReactiveCommand();
    }
}