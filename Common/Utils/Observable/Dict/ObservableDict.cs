// Public API
// ReSharper disable UnusedMember.Global

using System.Collections;

namespace Common.Utils.Observable.Dict;

public class ObservableDict<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _items;

    public ObservableDict()
    {
        _items = new Dictionary<TKey, TValue>();
    }

    public ObservableDict(IDictionary<TKey, TValue> dict)
    {
        _items = new Dictionary<TKey, TValue>(dict);
    }

    public event EventHandler<DictChangedEventArgs<TKey, TValue>> Changed;

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public TValue this[TKey key]
    {
        get => _items[key];
        set
        {
            _items[key] = value;
            var eventArgs = new DictChangedEventArgs<TKey, TValue>(CollectionChangeType.Replace, key, value);
            Changed?.Invoke(this, eventArgs);
        }
    }

    public void Add(TKey key, TValue value)
    {
        _items.Add(key, value);

        var eventArgs = new DictChangedEventArgs<TKey, TValue>(CollectionChangeType.Add, key, value);
        Changed?.Invoke(this, eventArgs);
    }

    public bool Remove(TKey key)
    {
        bool doesExist = _items.ContainsKey(key);
        if (doesExist)
        {
            var value = _items[key];
            _items.Remove(key);

            var eventArgs = new DictChangedEventArgs<TKey, TValue>(CollectionChangeType.Remove, key, value);
            Changed?.Invoke(this, eventArgs);
        }

        return doesExist;
    }

    public void Clear()
    {
        _items.Clear();

        var eventArgs = new DictChangedEventArgs<TKey, TValue>(CollectionChangeType.Clear, default, default);
        Changed?.Invoke(this, eventArgs);
    }

    #region Wrapper

    public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);
    public System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue> AsReadOnly() => new(_items);
    public bool Contains(KeyValuePair<TKey, TValue> item) => _items.Contains(item);
    public bool ContainsKey(TKey key) => _items.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
        ((IDictionary<TKey, TValue>) _items).CopyTo(array, arrayIndex);

    public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);
    public bool TryGetValue(TKey key, out TValue value) => _items.TryGetValue(key, out value);

    public int Count => _items.Count;
    public bool IsReadOnly => false;
    public ICollection<TKey> Keys => _items.Keys;
    public ICollection<TValue> Values => _items.Values;

    #endregion
}