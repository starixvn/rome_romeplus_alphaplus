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
            .AddSingleton(GetPartialUMConfig(configuration))
            .AddSingleton<IPartialUMService, PartialUMService>();

        return services;
    }

    private static Dictionary<string, IList<string>> GetPartialUMConfig(IConfiguration configuration)
    {
        var partialUMConfig = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);
        var partialUMArray = configuration.GetSection("PartialUMOptions:PartialUM").Get<UMConfig[]>();

        if (partialUMArray != null)
        {
            foreach (var partialUM in partialUMArray)
            {
                foreach (var key in partialUM.SettingKeys)
                {
                    if (!partialUMConfig.TryGetValue(key, out var functionNames))
                    {
                        functionNames = new List<string>();
                        partialUMConfig[key] = functionNames;
                    }

                    functionNames.Add(partialUM.FunctionName);
                }
            }
        }

        return partialUMConfig;
    }
}