namespace Logic.Utils.Observable.Dict;

public class DictChangedEventArgs<TKey, TValue> : EventArgs
{
    public DictChangedEventArgs(CollectionChangeType change, TKey key, TValue value)
    {
        Change = change;
        Key = key;
        Value = value;
    }
   
    public CollectionChangeType Change { get; }
    public TKey Key { get; }
    public TValue Value { get; }
}