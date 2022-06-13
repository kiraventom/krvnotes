using System.Collections.Specialized;

namespace BL;

public interface IKeyedCollection<out T> : INotifyCollectionChanged, IEnumerable<T>
{
    T this[string key] { get; }
}