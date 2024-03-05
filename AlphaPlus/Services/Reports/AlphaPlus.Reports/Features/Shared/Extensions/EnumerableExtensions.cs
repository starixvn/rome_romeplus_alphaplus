namespace AlphaPlus.Reports.Features.Shared.Extensions;

public static class EnumerableExtensions
{
    private const char JoinSeparator = ',';

    public static string JoinWithComma<T>(this IEnumerable<T> array)
        => array == null ? string.Empty : string.Join(JoinSeparator, array);
}
