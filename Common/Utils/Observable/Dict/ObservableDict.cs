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
            var oldValue = _items[key];
            _items[key] = value;
            var eventArgs = DictChangedEventArgs<TKey, TValue>.OnReplace(key, oldValue, value);
            Changed?.Invoke(this, eventArgs);
        }
    }

    public void Add(TKey key, TValue value)
    {
        _items.Add(key, value);

        var eventArgs = DictChangedEventArgs<TKey, TValue>.OnAdd(key, value);
        Changed?.Invoke(this, eventArgs);
    }

    public bool Remove(TKey key)
    {
        bool doesExist = _items.ContainsKey(key);
        if (doesExist)
        {
            var value = _items[key];
            var oldIndex = Array.IndexOf(_items.Keys.ToArray(), key); // не прикольно но wpf требует индекс
            _items.Remove(key);

            var eventArgs = DictChangedEventArgs<TKey, TValue>.OnRemove(key, value, oldIndex);
            Changed?.Invoke(this, eventArgs);
        }

        return doesExist;
    }

    public void Clear()
    {
        var clearedItems = _items.ToDictionary(pair => pair.Key, pair => pair.Value);
        _items.Clear();

        var eventArgs = DictChangedEventArgs<TKey, TValue>.OnClear(clearedItems);
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