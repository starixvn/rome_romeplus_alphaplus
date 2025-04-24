using System.ComponentModel;
using System.Reflection;

namespace AlphaPlus.CashService.API.Extensions;

using Enum = System.Enum;

public static class EnumHelper
{
    public static string GetDescription<TEnum>(TEnum enumValue) where TEnum : Enum
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

        if (fieldInfo != null)
        {
            var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();

            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }
        }

        return enumValue.ToString();
    }
}