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

    public static string[] ToList(this string value, char separator = ',')
        => string.IsNullOrWhiteSpace(value)
        ? Array.Empty<string>()
        : value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
}
