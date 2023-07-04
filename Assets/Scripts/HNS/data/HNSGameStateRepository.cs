using System;
using HNS.Model;
using UniRx;
using Zenject;

namespace HNS.data
{
    public class HNSGameStateRepository: IInitializable
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

        public IObservable<GameStates> GetStateFlow() => stateSubject;
    }
}