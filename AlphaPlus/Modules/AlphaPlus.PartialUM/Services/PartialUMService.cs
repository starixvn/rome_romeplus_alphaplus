using AlphaPlus.PartialUM.Configurations;
using AlphaPlus.PartialUM.Exceptions;
using AlphaPlus.PartialUM.Models;
using Fanex.Caching;
using AlphaPlus.PartialUM.DataAccess;
using Microsoft.Extensions.Options;

namespace AlphaPlus.PartialUM.Services;

public interface IPartialUMService
{
    Task ThrowIfUM(string settingKey, string siteName);
}

public class PartialUMService(
    ICacheService cacheService,
    IPartialUMDataAccess partialUMDataAccess,
    Dictionary<string, IList<string>> partialUMConfig,
    IOptions<PartialUMOptions> options) : IPartialUMService
{
    private readonly ICacheService cacheService = cacheService;
    private readonly IPartialUMDataAccess partialUMDataAccess = partialUMDataAccess;
    private readonly Dictionary<string, IList<string>> partialUMConfig = partialUMConfig;
    private readonly int cacheInSeconds = options.Value.CacheInSeconds;

    public async Task ThrowIfUM(string settingKey, string siteName)
    {
        var umEntities = await GetUMEntities(siteName);

        if (!umEntities.Any())
        {
            return;
        }

        if (!partialUMConfig.TryGetValue(settingKey, out var functionNames))
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
        => cacheService.GetOrSetAsync(
            $"AlphaPlus.PartialUM.Services.PartialUM.SiteName-{siteName}",
            () => partialUMDataAccess.GetUMEntities(siteName),
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheInSeconds)));

    private static string FormatUMTime(DateTime dateTime)
        => dateTime.ToString("dd/MM/yyyy hh:mm tt");
}