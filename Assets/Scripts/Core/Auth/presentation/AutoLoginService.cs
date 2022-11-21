using System;
using Core.Auth.domain;
using UniRx;
using Zenject;

namespace Core.Auth.presentation
{
    public class AutoLoginService : IInitializable, IDisposable
    {
        [Inject] private IAuthRepository authRepository;
        private IDisposable disposable;

        public void Initialize()
        {
            disposable = authRepository
                .GetLoggedInFlow()
                .Where(loggedIn => !loggedIn)
                .Subscribe(_ => authRepository.Login(() => { }, () => { }));
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}