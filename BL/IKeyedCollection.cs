namespace BL;

public interface IKeyedCollection<out T> : IEnumerable<T>
{
    T this[string key] { get; }

    IKeyedCollection<TN> Cast<TN>(Func<T, TN> valueCaster);
}