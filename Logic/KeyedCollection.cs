using System.Collections;
using BL;
using Common.Utils.Observable.Dict;

namespace Logic;

internal class KeyedCollection<T, IT> : IKeyedCollection<IT> where T : class, IT
{
    private readonly ObservableDict<string, T> _items;

    public KeyedCollection(ObservableDict<string, T> items)
    {
        _items = items;
    }

    public IT this[string key] => _items[key];

    public IEnumerator<IT> GetEnumerator() => _items.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}