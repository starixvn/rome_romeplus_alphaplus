using System.Text;
using System.Text.Json;
using AlphaPlus.IPInfo.DataAccess;
using AlphaPlus.IPInfo.Extensions;
using AlphaPlus.IPInfo.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.IPInfo.Services;

public interface IIPInfoService
{
    Task<IDictionary<string, string>> GetCountries(IEnumerable<string> ipList);
}

public class IPInfoService : IIPInfoService
{
    private const string IPV4TopCountriesCacheKey = "Alpha.IPInfo.IPV4.TopCountries";
    private const string IPV4CacheKey = "Alpha.IPInfo.IPV4";
    private const string IPV6CacheKey = "Alpha.IPInfo.IPV6";

    private readonly IPInformationConfig cacheConfig;
    private readonly IIPInfoDataAccess ipInfoDataAccess;
    private readonly IDistributedCache distributedCache;
    private readonly IMemoryCache memoryCache;

    public IPInfoService(
        IPInformationConfig cacheConfig,
        IIPInfoDataAccess ipInfoDataAccess,
        IDistributedCache distributedCache,
        IMemoryCache memoryCache)
    {
        this.cacheConfig = cacheConfig;
        this.ipInfoDataAccess = ipInfoDataAccess;
        this.distributedCache = distributedCache;
        this.memoryCache = memoryCache;
    }

    public async Task<IDictionary<string, string>> GetCountries(IEnumerable<string> ipList)
    {
        var ipDictionary = new Dictionary<string, string>();
        var (ipV4List, ipV6List) = IPInfoHelper.SplitIPV4AndIPV6List(ipList);

        if (ipV4List.Count > 0)
        {
            await IPV4ToCountry(ipDictionary, ipV4List);
        }

        if (ipV6List.Count > 0)
        {
            await IPV6ToCountry(ipDictionary, ipV6List);
        }

        return ipDictionary;
    }

    private async Task IPV4ToCountry(IDictionary<string, string> ipDictionary, IList<string> ipList)
    {
        var ipV4TopCountries = await GetIPV4ByTopCountry();
        var ipV4NotInTopCountries = memoryCache.Get<List<IPRange>>(IPV4CacheKey) ?? [];
        var ipV4NotFoundList = new List<string>();

        foreach (var ip in ipList)
        {
            var ipNumber = IPInfoHelper.ConvertIPV4ToNumber(ip);
            var ipFound = ipV4TopCountries.BinarySearch2(ipNumber) ?? ipV4NotInTopCountries.BinarySearch2(ipNumber);

            if (ipFound == null)
            {
                ipV4NotFoundList.Add(ip);
            }
            else
            {
                ipDictionary[ip] = ipFound.Country;
            }
        }

        if (ipV4NotFoundList.Count > 0)
        {
            await GetIPV4Country(ipDictionary, ipV4NotFoundList);
        }
    }

    private async Task<IList<IPRange>> GetIPV4ByTopCountry()
    {
        var encodedCache = await distributedCache.GetAsync(IPV4TopCountriesCacheKey);
        IList<IPRange> ipV4TopCountries;
        if (encodedCache != null)
        {
            ipV4TopCountries = JsonSerializer.Deserialize<IList<IPRange>>(Encoding.UTF8.GetString(encodedCache));
        }
        else
        {
            ipV4TopCountries = await GetIPV4ByTopCountryFromDB();
            await distributedCache.SetAsync(
                IPV4TopCountriesCacheKey,
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(ipV4TopCountries)),
                new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheConfig.IPV4TopCountriesCacheInMinutes)));
        }

        return ipV4TopCountries;
    }

    private async Task<IList<IPRange>> GetIPV4ByTopCountryFromDB()
    {
        var ipRanges = (await ipInfoDataAccess.GetIPV4InfoByTopCountries(cacheConfig.IPV4TopCountries))
            .Select(ip => new IPRange(ip.IPFrom, ip.IPTo, ip.CountryLong))
            .ToList();
        ipRanges.Sort();

        return ipRanges;
    }

    private async Task GetIPV4Country(IDictionary<string, string> ipDictionary, IList<string> ipList)
    {
        var availableIPV4List = await MergeIPV4Country(ipList);

        foreach (var ip in ipList)
        {
            var ipNumber = IPInfoHelper.ConvertIPV4ToNumber(ip);
            var ipFound = availableIPV4List.BinarySearch2(ipNumber);

            if (ipFound != null)
            {
                ipDictionary[ip] = ipFound.Country;
            }
        }
    }

    private async Task<IList<IPRange>> MergeIPV4Country(IList<string> ipList)
    {
        var availableIPV4List = memoryCache.Get<List<IPRange>>(IPV4CacheKey) ?? [];
        availableIPV4List.AddRange(await GetIPV4CountryFromDB(ipList));

        var comparer = new IPRangeEqualityComparer();
        availableIPV4List = [.. availableIPV4List.Distinct(comparer)];
        availableIPV4List.Sort();

        memoryCache.Set(
            IPV4CacheKey,
            availableIPV4List,
            new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheConfig.IPV4CacheInMinutes)));

        return availableIPV4List;
    }

    private async Task<IEnumerable<IPRange>> GetIPV4CountryFromDB(IList<string> ipList)
        => (await ipInfoDataAccess.GetIPV4Info(ipList.JoinComma()))
            .Select(ip => new IPRange(ip.IPFrom, ip.IPTo, ip.CountryLong));

    private async Task IPV6ToCountry(IDictionary<string, string> ipDictionary, IList<string> ipList)
    {
        var ipNotFoundList = new List<string>();

        foreach (var ip in ipList)
        {
            var country = memoryCache.Get<string>($"{IPV6CacheKey}_{ip}");

            if (country == null)
            {
                ipNotFoundList.Add(ip);
            }
            else
            {
                ipDictionary[ip] = country;
            }
        }

        if (ipNotFoundList.Count > 0)
        {
            await GetIPV6Country(ipDictionary, ipNotFoundList);
        }
    }

    private async Task GetIPV6Country(IDictionary<string, string> ipDictionary, IList<string> ipList)
    {
        var ipV6Info = await ipInfoDataAccess.GetIPV6Info(ipList.JoinComma());

        foreach (var ip in ipV6Info)
        {
            memoryCache.Set(
                $"{IPV6CacheKey}_{ip.IP}",
                ip.CountryLong,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheConfig.IPV6CacheInMinutes)));
            ipDictionary[ip.IP] = ip.CountryLong;
        }
    }
}
