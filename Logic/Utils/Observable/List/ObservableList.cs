using System.Collections;

namespace Logic.Utils.Observable.List;

public class ObservableList<T> : ICollection<T>
{
    public ObservableList()
    {
        _items = new List<T>();
    }
    
    public ObservableList(IEnumerable<T> items)
    {
        _items = new List<T>(items);
    }

    public event EventHandler<ListChangedEventArgs<T>> Changed;

    private readonly List<T> _items;
    
    public T this[int index]
    {
        get => _items[index];
        set
        {
            _items[index] = value;
            
            var eventArgs = new ListChangedEventArgs<T>(CollectionChangeType.Replace, value);
            Changed?.Invoke(this, eventArgs);
        }
    }

    public void Add(T item)
    {
        _items.Add(item);
        
        var eventArgs = new ListChangedEventArgs<T>(CollectionChangeType.Add, item);
        Changed?.Invoke(this, eventArgs);
    }

    public bool Remove(T item)
    {
        var didRemove = _items.Remove(item);
        if (didRemove)
        {
            var eventArgs = new ListChangedEventArgs<T>(CollectionChangeType.Remove, item);
            Changed?.Invoke(this, eventArgs);
        }

        return didRemove;
    }

    public void RemoveAt(int index)
    {
        var item = _items[index];
        _items.RemoveAt(index);
        
        var eventArgs = new ListChangedEventArgs<T>(CollectionChangeType.Remove, item);
        Changed?.Invoke(this, eventArgs);
    }

    public void Clear()
    {
        var isNotEmpty = _items.Any();
        if (isNotEmpty)
        {
            _items.Clear();
            
            var eventArgs = new ListChangedEventArgs<T>(CollectionChangeType.Clear, default);
            Changed?.Invoke(this, eventArgs);
        }
    }
    
    #region Wrapper

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public System.Collections.ObjectModel.ReadOnlyCollection<T> AsReadOnly() => _items.AsReadOnly();
    public bool Contains(T item) => _items.Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
    public int FindIndex(Predicate<T> match) => _items.FindIndex(match);

    public int Count => _items.Count;
    public bool IsReadOnly => false;
    

    #endregion
}