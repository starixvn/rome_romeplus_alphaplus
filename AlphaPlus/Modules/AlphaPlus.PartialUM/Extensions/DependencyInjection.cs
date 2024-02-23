using AlphaPlus.PartialUM.Configurations;
using AlphaPlus.PartialUM.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaPlus.PartialUM.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPartialUM(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<PartialUMOptions>(configuration.GetSection(nameof(PartialUMOptions)))
            .AddSingleton(GetPartialUMHash(configuration))
            .AddSingleton<IPartialUMService, PartialUMService>();

        return services;
    }

    private static Dictionary<string, IList<string>> GetPartialUMHash(IConfiguration configuration)
    {
        var partialUMHash = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);
        var partialUMArray = configuration.GetSection("PartialUMOptions.PartialUM").Get<UMConfiguration[]>();

        foreach (var partialUM in partialUMArray)
        {
            foreach (var key in partialUM.SettingKeys)
            {
                if (!partialUMHash.TryGetValue(key, out var functionNames))
                {
                    functionNames = new List<string>();
                    partialUMHash[key] = functionNames;
                }

                functionNames.Add(partialUM.FunctionName);
            }
        }

        return partialUMHash;
    }
}