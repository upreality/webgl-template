using System;
using UniRx;
using Zenject;

namespace Utils.MirrorClient.domain
{
    public class MirrorCommandUseCase
    {
        [Inject] private IMirrorServerRepository serverRepository;
        [Inject] private IMatchConnectedStateRepository matchConnectedStateRepository;

        public IObservable<TResponse> Listen<TResponse>() => GetOnConnected(
            () => serverRepository.Listen<TResponse>()
        );

        public IObservable<TResponse> Request<TResponse>(long commandId) => Request<int, TResponse>(0);

        public IObservable<TResponse> Request<TRequest, TResponse>(TRequest content) => GetOnConnected(
            () => serverRepository.Request<TRequest, TResponse>(content)
        );
        
        public IObservable<TResponse> Subscribe<TResponse>() => Subscribe<int, TResponse>(0);

        public IObservable<TResponse> Subscribe<TRequest, TResponse>(TRequest content)
        {
            var requestFlow = Request<TRequest, TResponse>(content);
            var listenFlow = Listen<TResponse>();
            return requestFlow.Merge(listenFlow);
        }

        private IObservable<TResponse> GetOnConnected<TResponse>(
            Func<IObservable<TResponse>> responseProvider
        ) => matchConnectedStateRepository
            .GetConnectedState()
            .Where(state => state == MatchConnectedState.Connected)
            .Select(_ => responseProvider())
            .Switch();
    }
}