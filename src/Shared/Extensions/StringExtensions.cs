internal static class StringExtensions
{
    public static IEnumerable<int> ToListInt(this string value, char separator = ',')
    {
        var arrayString = ToList(value, separator);

        foreach (var item in arrayString)
        {
            if (int.TryParse(item, out var number))
            {
                yield return number;
            }
        }
    }

    public static IEnumerable<int> ToListIntRemoveDuplicate(this string value, char separator = ',')
    {
        var hashSetInt = new HashSet<int>();
        var arrayString = ToList(value, separator);

        foreach (var item in arrayString)
        {
            if (int.TryParse(item, out var number) && hashSetInt.Add(number))
            {
                yield return number;
            }
        }
    }

    public static string[] ToList(this string value, char separator = ',')
        => string.IsNullOrWhiteSpace(value)
        ? []
        : value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
}
