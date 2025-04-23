internal static class EnumerableExtensions
{
    public static string JoinWithComma<T>(this IEnumerable<T> array, string separator = ",")
        => array == null ? string.Empty : string.Join(separator, array);
}
