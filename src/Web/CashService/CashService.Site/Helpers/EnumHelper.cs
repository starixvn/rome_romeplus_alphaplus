namespace AlphaPlus.CashService.Site.Helpers;

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
}