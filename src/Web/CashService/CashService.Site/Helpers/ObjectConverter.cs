namespace AlphaPlus.CashService.Site.Helpers;

using System.Reflection;

public static class ObjectConverter
{
    public static Dictionary<string, string> ToDictionary<T>(T obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        return obj.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetValue(obj) != null)
            .ToDictionary(
                prop => prop.Name,
                prop => prop.GetValue(obj)?.ToString()
            );
    }
}