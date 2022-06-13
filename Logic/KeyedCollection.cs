using System.Collections;
using System.Collections.Specialized;
using BL;
using Common.Utils.Observable.Dict;

namespace Logic;

internal class KeyedCollection<T> : IKeyedCollection<T>
{
    private readonly ObservableDict<string, T> _items;

    public event NotifyCollectionChangedEventHandler CollectionChanged;

    public KeyedCollection(ObservableDict<string, T> items)
    {
        _items = items;
        _items.Changed += ItemsOnChanged;
    }

    public T this[string key] => _items[key];
    
    private void ItemsOnChanged(object sender, DictChangedEventArgs<string, T> e)
    {
        var newArgs = e.ToNotifyCollectionChangedEventArgs();
         CollectionChanged?.Invoke(this, newArgs);
    }

    public IEnumerator<T> GetEnumerator() => _items.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}