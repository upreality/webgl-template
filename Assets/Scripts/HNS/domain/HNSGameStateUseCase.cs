using System;
using HNS.domain.Model;
using UniRx;
using Zenject;

namespace HNS.domain
{
    public class HNSGameStateUseCase: IInitializable
    {
        private ISubject<GameStates> stateSubject;

        public void Initialize()
        {
            var completedSubject = new Subject<GameStates>();
            completedSubject.OnCompleted();
            stateSubject = completedSubject;
        }

        public void SubmitState(GameStates state) => stateSubject.OnNext(state);
        
        public void Complete() => stateSubject.OnCompleted();

        public IObservable<GameStates> GetStateFlow() => stateSubject.DistinctUntilChanged();
    }
}