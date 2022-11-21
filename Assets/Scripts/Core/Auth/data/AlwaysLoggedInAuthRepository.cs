using System;
using Core.Auth.domain;
using UniRx;

namespace Core.Auth.data
{
    public class AlwaysLoggedInAuthRepository : IAuthRepository
    {
        public string LoginUserId => "";
        public IObservable<bool> GetLoggedInFlow() => Observable.Return(true);
        public void Login(Action onSuccess, Action onFailed) => onSuccess?.Invoke();
    }
}