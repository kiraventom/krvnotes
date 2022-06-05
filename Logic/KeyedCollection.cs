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

    public IEnumerator<T> GetEnumerator() => _items.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}