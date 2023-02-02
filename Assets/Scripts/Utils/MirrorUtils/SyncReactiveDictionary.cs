using System;
using System.Collections.Generic;
using Mirror;
using UniRx;

namespace Utils.MirrorUtils
{
    public class SyncReactiveDictionary<TKey, TValue> : SyncIDictionary<TKey, TValue>, IReactiveDictionary<TKey, TValue>
    {
        private ReactiveDictionary<TKey, TValue> RDictionary => (ReactiveDictionary<TKey, TValue>)objects;

        public SyncReactiveDictionary() : base(new ReactiveDictionary<TKey, TValue>()) { }
        public SyncReactiveDictionary(IEqualityComparer<TKey> eq) : base(new ReactiveDictionary<TKey, TValue>(eq)) {}
    
        public new Dictionary<TKey, TValue>.ValueCollection Values => RDictionary.Values;
        public new Dictionary<TKey, TValue>.KeyCollection Keys => RDictionary.Keys;
        public new Dictionary<TKey, TValue>.Enumerator GetEnumerator() => RDictionary.GetEnumerator();

        public IObservable<DictionaryAddEvent<TKey, TValue>> ObserveAdd() => RDictionary.ObserveAdd();

        public IObservable<int> ObserveCountChanged(bool notifyCurrentCount = false) => RDictionary.ObserveCountChanged(notifyCurrentCount);

        public IObservable<DictionaryRemoveEvent<TKey, TValue>> ObserveRemove() => RDictionary.ObserveRemove();

        public IObservable<DictionaryReplaceEvent<TKey, TValue>> ObserveReplace() => RDictionary.ObserveReplace();

        public IObservable<Unit> ObserveReset() => ((ReactiveDictionary<TKey, TValue>)objects).ObserveReset();
    }
}