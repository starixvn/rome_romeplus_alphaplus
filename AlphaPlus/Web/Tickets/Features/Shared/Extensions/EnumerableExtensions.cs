namespace AlphaPlus.Tickets.Site.Features.Shared.Extensions;

public static class EnumerableExtensions
{
    private static readonly char JoinSeparator = ',';

    public static string Join<T>(this IEnumerable<T> array)
        => array == null ? string.Empty : string.Join(JoinSeparator, array);
}
