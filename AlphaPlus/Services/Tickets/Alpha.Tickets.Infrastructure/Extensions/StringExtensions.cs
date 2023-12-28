namespace Alpha.Tickets.Infrastructure.Extensions;

public static class StringExtensions
{
    public static IEnumerable<int> ToListInt(this string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            var idArray = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in idArray)
            {
                if (int.TryParse(item, out var id))
                {
                    yield return id;
                }
            }
        }
    }
}
