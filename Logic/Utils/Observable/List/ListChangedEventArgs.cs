namespace Logic.Utils.Observable.List;

public class ListChangedEventArgs<T> : EventArgs
{
    public ListChangedEventArgs(CollectionChangeType change, T item)
    {
        Change = change;
        Item = item;
    }
   
    public CollectionChangeType Change { get; }
    public T Item { get; }
}