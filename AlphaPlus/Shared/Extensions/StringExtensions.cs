public static class StringExtensions
{
    public static IEnumerable<int> ToListInt(this string value, char separator = ',')
    {
        var arrayInt = new List<int>();
        var arrayString = ToList(value, separator);

        foreach (var item in arrayString)
        {
            if (int.TryParse(item, out var number))
            {
                arrayInt.Add(number);
            }
        }

        return arrayInt;
    }

    public static IEnumerable<int> ToListIntRemoveDuplicate(this string value, char separator = ',')
    {
        var hashSetInt = new HashSet<int>();
        var arrayString = ToList(value, separator);

        foreach (var item in arrayString)
        {
            if (int.TryParse(item, out var number))
            {
                hashSetInt.Add(number);
            }
        }

        return hashSetInt;
    }

    public static string[] ToList(this string value, char separator = ',')
        => string.IsNullOrWhiteSpace(value)
        ? []
        : value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
}
