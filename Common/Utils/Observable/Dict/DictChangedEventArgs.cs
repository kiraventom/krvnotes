// Public API
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Collections.Specialized;

namespace Common.Utils.Observable.Dict;

public class DictChangedEventArgs<TKey, TValue> : EventArgs
{
    private DictChangedEventArgs(CollectionChangeType change, TKey oldKey, TKey newKey, int oldIndex,
        TValue oldValue, TValue newValue, IDictionary<TKey, TValue> oldItems)
    {
        Change = change;
        OldKey = oldKey;
        NewKey = newKey;
        OldIndex = oldIndex;
        OldValue = oldValue;
        NewValue = newValue;
        OldItems = oldItems;
    }

    internal static DictChangedEventArgs<TKey, TValue> OnAdd(TKey newKey, TValue newValue) =>
        new(CollectionChangeType.Add, default, newKey, -1, default, newValue, default);

    internal static DictChangedEventArgs<TKey, TValue> OnRemove(TKey oldKey, TValue oldValue, int oldIndex) =>
        new(CollectionChangeType.Remove, oldKey, default, oldIndex, oldValue, default, default);

    internal static DictChangedEventArgs<TKey, TValue> OnReplace(TKey key, TValue oldValue, TValue newValue) =>
        new(CollectionChangeType.Replace, key, key, -1, oldValue, newValue, default);

    internal static DictChangedEventArgs<TKey, TValue> OnClear(IDictionary<TKey, TValue> oldItems) =>
        new(CollectionChangeType.Clear, default, default, -1, default, default, oldItems);

    public CollectionChangeType Change { get; }
    public TKey OldKey { get; }
    public TKey NewKey { get; }
    public int OldIndex { get; }
    public TValue OldValue { get; }
    public TValue NewValue { get; }
    public IDictionary<TKey, TValue> OldItems { get; }

    public NotifyCollectionChangedEventArgs ToNotifyCollectionChangedEventArgs()
    {
        return Change switch
        {
            CollectionChangeType.Add => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, NewValue),
            CollectionChangeType.Remove => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, OldValue, OldIndex),
            CollectionChangeType.Replace => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, NewValue, OldValue),
            CollectionChangeType.Clear => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, OldItems.Values),

            _ => throw new ArgumentOutOfRangeException()
        };
    }
}