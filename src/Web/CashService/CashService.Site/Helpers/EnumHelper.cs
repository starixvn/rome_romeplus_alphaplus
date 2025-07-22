namespace AlphaPlus.CashService.Site.Helpers;

using System;
using System.ComponentModel;
using System.Reflection;
using System.Resources;

public static class EnumHelper
{
    public static string GetEnumDescriptionByNumber<TEnum>(int value, ResourceManager resourceManager) where TEnum : Enum
    {
        var enumValue = (TEnum)Enum.ToObject(typeof(TEnum), value);
        var field = typeof(TEnum).GetField(enumValue.ToString());
        var attr = field?.GetCustomAttribute<DescriptionAttribute>();

        if (attr != null)
        {
            var resourceKey = attr.Description;
            var resourceValue = resourceManager.GetString(resourceKey);
            if (!string.IsNullOrEmpty(resourceValue))
                return resourceValue;
            return resourceKey;
        }

        return enumValue.ToString();
    }

    public static List<Option<int, string>> GetEnumOptions<TEnum>(ResourceManager resourceManager) where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Select(e => new Option<int, string>
            {
                Value = Convert.ToInt32(e),
                Label = GetEnumDescriptionByNumber<TEnum>(Convert.ToInt32(e), resourceManager)
            })
            .ToList();
    }
}

public class Option<TValue, TLabel>
{
    public TValue Value { get; set; }
    public TLabel Label { get; set; }
}