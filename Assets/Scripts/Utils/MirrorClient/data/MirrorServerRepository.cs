using System;
using System.Collections.Generic;
using Mirror;
using ModestTree;
using UniRx;
using Utils.MirrorClient.domain;

namespace Utils.MirrorClient.data
{
    public class MirrorServerRepository: IMirrorServerRepository
    {
        private readonly MessageSubjects responseSubjects = new();

        IObservable<TResponse> IMirrorServerRepository.Request<TRequest, TResponse>(TRequest content)
        {
            var requestId = Guid.NewGuid().ToString();
            var requestMessage = new Message<TRequest>(content, requestId);
            NetworkClient.Send(requestMessage);
            return GetOrCreateSubject<TResponse>()
                .Where(message => message.GetRequestId(out var responseRequestId) && responseRequestId == requestId)
                .Select(message => message.Content);
        }

        IObservable<TResponse> IMirrorServerRepository.Listen<TResponse>() => GetOrCreateSubject<TResponse>()
            .Where(message => !message.GetRequestId(out _))
            .Select(message => message.Content);

        private Subject<Message<TResponse>> GetOrCreateSubject<TResponse>()
        {
            if (responseSubjects.TryGet<TResponse>(out var subject))
                return subject;

            var newSubject = responseSubjects.Create<TResponse>();
            NetworkClient.RegisterHandler<Message<TResponse>>(newSubject.OnNext);
            return newSubject;
        }

        private class MessageSubjects
        {
            private readonly Dictionary<Type, object> typeToSubject = new();

            public bool TryGet<T>(out Subject<Message<T>> subject)
            {
                var key = typeof(T);
                if (!typeToSubject.TryGetValue(key, out var item))
                {
                    subject = default;
                    return false;
                }

                subject = item as Subject<Message<T>>;
                return true;
            }

            public Subject<Message<T>> Create<T>()
            {
                var key = typeof(T);
                var subject = new Subject<Message<T>>();
                typeToSubject[key] = subject;
                return subject;
            }
        }

        private readonly struct Message<T> : NetworkMessage
        {
            public readonly T Content;

            private readonly string requestIdOrEmpty;

            public Message(T content, string requestId = "")
            {
                Content = content;
                requestIdOrEmpty = requestId;
            }

            public bool GetRequestId(out string requestId)
            {
                requestId = requestIdOrEmpty;
                return !requestIdOrEmpty.IsEmpty();
            }
        }
    }
}