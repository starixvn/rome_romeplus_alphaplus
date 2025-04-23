using System.Reflection;

namespace AlphaPlus.CashService.Site.Helpers;

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
            .Where(p => p.GetValue(obj) != null) // Exclude null values
            .ToDictionary(
                prop => prop.Name, // Use property name as the key
                prop => prop.GetValue(obj)?.ToString() // Convert value to string
            );
    }
}