using System;
using Core.Auth.domain;
using HNS.domain.model;
using UniRx;
using Zenject;

namespace HNS.domain
{
    public class PlayerHiderStateUseCase
    {
        [Inject] private HiderStateUseCase hiderStateUseCase;
        [Inject] private IAuthRepository authRepository;

        public IObservable<HiderState> GetStateFlow() => long
            .TryParse(authRepository.LoginUserId, out var playerId)
            ? hiderStateUseCase.GetStateFlow(playerId)
            : Observable.Empty<HiderState>();
    }
}