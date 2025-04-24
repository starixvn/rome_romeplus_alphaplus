using AlphaPlus.Web.Models;

namespace AlphaPlus.Main.Site.Helpers;

public static class DomainsHelper
{
    private readonly static IConfigurationRoot configurationRoot = InitConfiguration();

    private static IConfigurationRoot InitConfiguration()
    {
        const string configFileName = "appsettings.json";
        const string domainMappingFileName = "Domains/domainmappings.json";

        if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), configFileName)))
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFileName, optional: false, reloadOnChange: true)
                .AddJsonFile(domainMappingFileName, optional: false, reloadOnChange: true);

            return builder.Build();
        }

        return null;
    }

    public static DomainMapping[] DomainMappings
            => configurationRoot.GetSection("DomainMappings").Get<DomainMapping[]>() ?? Array.Empty<DomainMapping>();
}