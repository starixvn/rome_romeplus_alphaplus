namespace AlphaPlus.Tickets.Site.Features.Shared.Extensions;

public static class StringExtensions
{
    private static readonly char[] SplitSeparator = { ',' };

    public static IEnumerable<int> ToListInt(this string value)
    {
        var arrayString = value.ToList();

        foreach (var item in arrayString)
        {
            if (int.TryParse(item, out var number))
            {
                yield return number;
            }
        }
    }

    public static string[] ToList(this string value)
        => string.IsNullOrWhiteSpace(value)
        ? Array.Empty<string>()
        : value.Split(SplitSeparator, StringSplitOptions.RemoveEmptyEntries);
}
