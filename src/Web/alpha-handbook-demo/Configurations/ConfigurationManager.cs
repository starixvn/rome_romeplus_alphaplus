using Microsoft.Extensions.Configuration;

namespace Alpha.Handbook.Demo.Configurations;

public static class ConfigurationManager
{
    private static IConfiguration _config;

    public static void Init(IConfiguration config) => _config = config;

    public static T GetValue<T>(string key, T defaultValue = default)
    {
        try
        {
            return _config.GetValue<T>(key, defaultValue);
        }
        catch (Exception ex)
        {
            throw new InvalidCastException($"Key: {key}", ex);
        }
    }

    public static T GetSection<T>(string section, T defaultValue = default)
    {
        try
        {
            return _config.GetSection(section).Get<T>() ?? defaultValue;
        }
        catch (Exception ex)
        {
            throw new InvalidCastException($"Section: {section}", ex);
        }
    }
}
