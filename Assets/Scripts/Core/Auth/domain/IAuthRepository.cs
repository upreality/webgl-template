using System;
using Core.Auth.domain.model;

namespace Core.Auth.domain
{
    public interface IAuthRepository
    {
        public IObservable<bool> GetLoggedInFlow();
        public string LoginUserId { get; }
        public IObservable<bool> Login(AuthData authData);
    }
}