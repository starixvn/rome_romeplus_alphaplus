using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace AlphaPlus.Core.Configurations;

public static class ConfigurationManager
{
    private static IConfiguration _config;

    public static void Init(IConfiguration config) =>
        _config = config;

    public static T GetValue<T>(string key, T defaultValue = default)
    {
        try
        {
            var value = _config[key];
            if (value == null)
            {
                return defaultValue;
            }

            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));

            return (T)typeConverter.ConvertFromString(value);
        }
        catch (Exception ex)
        {
            throw new InvalidCastException($"Key: {key}", ex);
        }
    }
}
