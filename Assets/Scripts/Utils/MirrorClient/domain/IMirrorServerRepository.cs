using System;

namespace Utils.MirrorClient.domain
{
    public interface IMirrorServerRepository
    {
        public IObservable<TResponse> Listen<TResponse>();
        public IObservable<TResponse> Request<TRequest, TResponse>(TRequest content);
    }
}