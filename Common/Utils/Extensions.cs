namespace Common.Utils
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            ArgumentNullException.ThrowIfNull(collection);
            ArgumentNullException.ThrowIfNull(action);

            foreach (var item in collection) 
                action(item);
        }
    }
}
