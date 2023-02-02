﻿using System;
using UniRx;
using UnityEngine;

namespace Utils.Reactive
{
    public static class RxExtensions
    {
        public static IObservable<TV> GetItemFlow<TK, TV>(this IReadOnlyReactiveDictionary<TK, TV> target, TK key) 
        {
            var addFlow = target
                .ObserveAdd()
                .Where(addEvent => Equals(addEvent.Key, key))
                .Select(addEvent => addEvent.Value);
            var replaceFlow = target
                .ObserveReplace()
                .Where(replaceEvent => Equals(replaceEvent.Key, key))
                .Select(replaceEvent => replaceEvent.NewValue);
            var itemFlow = addFlow.Merge(replaceFlow);
            return target.ContainsKey(key) ? itemFlow.StartWith(target[key]) : itemFlow;
        }

        public static IObservable<TV> GetItemFlow<TK, TV>(
            this IReadOnlyReactiveDictionary<TK, TV> target,
            TK key,
            TV defaultItem)
        {
            var flow = target.GetItemFlow(key);
            return target.ContainsKey(key) ? flow : flow.StartWith(defaultItem);
        }
        
        public static IObservable<TResult> CombineWithPrevious<TSource,TResult>(
            this IObservable<TSource> source,
            Func<TSource, TSource, TResult> resultSelector)
        {
            return source.Scan(
                    Tuple.Create(default(TSource), default(TSource)),
                    (previous, current) => Tuple.Create(previous.Item2, current))
                .Select(t => resultSelector(t.Item1, t.Item2));
        }
        
        public static void CreateTimer(this Component component, int timeMs, Action onTimer)
        {
            var timerSpan = TimeSpan.FromMilliseconds(timeMs);
            Observable
                .Timer(timerSpan)
                .Repeat()
                .StartWith(0)
                .Subscribe(_ => onTimer.Invoke())
                .AddTo(component);
        }
    }
}