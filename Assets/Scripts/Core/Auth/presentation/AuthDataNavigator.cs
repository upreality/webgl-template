﻿using System;
using Core.Auth.domain;
using Core.Auth.domain.model;
using ParrelSync;
using UniRx;
using Zenject;
#if YANDEX_SDK
using Plugins.Platforms.YSDK;
#endif

namespace Core.Auth.presentation
{
    public class AuthDataNavigator
    {
#if YANDEX_SDK
        [Inject] private YandexSDK yandexSDK;
#endif
        [Inject] private LocalPlayerIdUseCase localPlayerIdUseCase;

        public IObservable<AuthData> RequestAuthData()
        {
#if YANDEX_SDK
            return GetYandexData();
#else
            return Observable.Return(GetLocalData());
#endif
        }

        private AuthData GetLocalData()
        {
            var playerId = ClonesManager.IsClone()
                ? localPlayerIdUseCase.GeneratedPlayerId()
                : localPlayerIdUseCase.GetOrCreate();
            return new AuthData
            {
                Type = AuthType.LocalId,
                Content = playerId
            };
        }

#if YANDEX_SDK
        private IObservable<AuthData> GetYandexData() => Observable.Create((IObserver<AuthData> observer) =>
            {
                void OnIdReceived(string yandexId)
                {
                    var data = string.IsNullOrWhiteSpace(yandexId)
                        ? GetLocalData() 
                        : new AuthData { Type = AuthType.YandexId, Content = yandexId };
                    observer.OnNext(data);
                    observer.OnCompleted();
                    yandexSDK.onPlayerIdReceived -= OnIdReceived;
                }
                yandexSDK.onPlayerIdReceived += OnIdReceived;
                yandexSDK.RequestPlayerIndentifier();
                return Disposable.Create(() => { });
            }
        );
#endif
    }
}