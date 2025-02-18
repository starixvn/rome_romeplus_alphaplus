using AlphaPlus.PartialUM.Configurations;
using AlphaPlus.PartialUM.DataAccess;
using AlphaPlus.PartialUM.Exceptions;
using AlphaPlus.PartialUM.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace AlphaPlus.PartialUM.Services;

public interface IPartialUMService
{
    Task ThrowIfUM(string settingKey, string siteName);
}

public class PartialUMService(
    IMemoryCache cacheService,
    IPartialUMDataAccess partialUMDataAccess,
    Dictionary<string, IList<string>> partialUMConfig,
    IOptions<PartialUMOptions> options) : IPartialUMService
{
    private readonly IMemoryCache cacheService = cacheService;
    private readonly IPartialUMDataAccess partialUMDataAccess = partialUMDataAccess;
    private readonly Dictionary<string, IList<string>> partialUMConfig = partialUMConfig;
    private readonly int cacheInSeconds = options.Value.CacheInSeconds;

    public async Task ThrowIfUM(string settingKey, string siteName)
    {
        if (!partialUMConfig.TryGetValue(settingKey, out var functionNames))
        {
            return;
        }

        var umEntities = await GetUMEntities(siteName);

        if (!umEntities.Any())
        {
            return;
        }

        var partialUMException = functionNames
            .Join(
                umEntities,
                funcName => funcName,
                um => um.FunctionTypeName,
                (_, um) => new PartialUMException(FormatUMTime(um.BeginTime), FormatUMTime(um.EndTime)))
            .FirstOrDefault();

        if (partialUMException != null)
        {
            throw partialUMException;
        }
    }

    private Task<IEnumerable<UMEntity>> GetUMEntities(string siteName)
        => cacheService.GetOrCreateAsync(
            $"AlphaPlus.PartialUM.Services.PartialUM.SiteName-{siteName}",
            cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheInSeconds));
                return partialUMDataAccess.GetUMEntities(siteName);
            });

    private static string FormatUMTime(DateTime dateTime)
        => dateTime.ToString("dd/MM/yyyy hh:mm tt");
}