using System.Collections;
using BL;
using Common.Utils.Observable.Dict;

namespace Logic;

internal class KeyedCollection<T> : IKeyedCollection<T>
{
    private readonly ObservableDict<string, T> _items;

    public KeyedCollection(ObservableDict<string, T> items)
    {
        _items = items;
    }

    public T this[string key] => _items[key];

    public IKeyedCollection<TN> Cast<TN>(Func<T, TN> valueCaster)
    {
        var dict = new ObservableDict<string, TN>();
        foreach (var pair in _items)
            dict.Add(pair.Key, valueCaster(pair.Value));

        return new KeyedCollection<TN>(dict);
    }

    public IEnumerator<T> GetEnumerator() => _items.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}