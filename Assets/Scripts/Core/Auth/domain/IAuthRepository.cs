using System;

namespace Core.Auth.domain
{
    public interface IAuthRepository
    {
        public IObservable<bool> GetLoggedInFlow();
        public string LoginUserId { get; }
        public void Login(Action onSuccess, Action onFailed);
    }
}