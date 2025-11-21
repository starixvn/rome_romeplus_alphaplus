using System.Net;
using System.Text.RegularExpressions;
using AlphaPlus.Utilities.IP.Criteria;
using AlphaPlus.Utilities.IP.Model;
using AlphaPlus.Utilities.IP.Models;
using Fanex.Data.Repository;
using Fanex.Logging;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Utilities.IP
{
    public interface IPService
    {
        bool IsVNIP(string ipAddress);

        FormatedIP FormatIP(string ip, bool isAdminCanViewVNIP);

        Task<IPInformationModel> GetIPInformation(string ip);

        Task<string> GetCountryCode(string ip);

        Task<bool> CheckHideIP(int superId, string system);
    }

    public class IPServiceImpl : IPService
    {
        private const string IPValidateRegex = @"(?:(?:25[0-5]|2[0-4]\d|[01]?\d\d?)\.){3}(?:25[0-5]|2[0-4]\d|[01]?\d\d?)";
        private const string VNIPListCacheKey = "Alpha.BetLists.Service.IPService.VNIPList";
        private const string CheckHideIPCacheKeyPrefix = "CheckHideIP_";
        private const string ExtNAP = "ExtNAP";
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPHelper ipHelper;
        private readonly IMemoryCache memoryCache;

        public IPServiceImpl(IDynamicRepository dynamicRepository, IPHelper ipHelper, IMemoryCache memoryCache)
        {
            this.dynamicRepository = dynamicRepository;
            this.ipHelper = ipHelper;
            this.memoryCache = memoryCache;
        }

        public bool IsVNIP(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                return false;
            }
            var ipVnRange = GetVNIPs();

            var isIpv6 = ipHelper.IsIPV6(ipAddress);
            if (isIpv6)
            {
                var ipv6Number = ipHelper.ConvertIPV6ToNumber(ipAddress);
                return ipVnRange.Item2.Any(ip => ip.FromNumber <= ipv6Number && ipv6Number <= ip.ToNumber);
            }

            double ipNumber = ipHelper.ConvertIPV4ToNumber(ipAddress);
            return ipVnRange.Item1.Any(ip => ip.From <= ipNumber && ipNumber <= ip.To);
        }

        public FormatedIP FormatIP(string ip, bool isAdminCanViewVNIP)
        {
            if (!IsValidIP(ip))
            {
                return new FormatedIP(ip, false);
            }

            Regex regex = new(IPValidateRegex);
            if (!regex.IsMatch(ip, 0))
            {
                return new FormatedIP(ip, false);
            }

            if (IsVNIP(ip) && !isAdminCanViewVNIP)
            {
                return new FormatedIP(string.Empty, false);
            }

            return new FormatedIP(ip, true);
        }

        public Task<IPInformationModel> GetIPInformation(string ip)
            => dynamicRepository.GetAsync<IPInformationModel>(new GetIPInformationCriteria(ip));

        private (IEnumerable<Ipv4Range>, IEnumerable<Ipv6Range>) GetVNIPs()
        {
            var vietnamIPs = memoryCache.GetOrCreate(
                VNIPListCacheKey,
                cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromHours(1);
                    return dynamicRepository.FetchMultiple<Ipv4Range, Ipv6Range>(new GetIPVNCriteria());
                });

            return vietnamIPs;
        }


        private static bool IsValidIP(string ipAddress)
        {
            return !string.IsNullOrWhiteSpace(ipAddress)
                && IPAddress.TryParse(ipAddress, out _);
        }

        public async Task<string> GetCountryCode(string ip)
        {
            var countryCode = "Unknown";
            var ipInformation = await GetIPInformation(ip);
            if (ipInformation != null)
            {
                countryCode = ipInformation.CountryLong == "Reserved" ? null : ipInformation.CountryLong;
            }

            return countryCode;
        }

        public async Task<bool> CheckHideIP(int superId, string system)
        {
            try
            {
                if (IsInternalByExtAdmin(system))
                {
                    return false;
                }

                var cacheKey = $"{CheckHideIPCacheKeyPrefix}{superId}";
                
                var isHideIP = await memoryCache.GetOrCreateAsync(
                    cacheKey,
                    async cacheEntry =>
                    {
                        cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(1);
                        var criteria = new CheckHideIPCriteria(superId);
                        await dynamicRepository.ExecuteAsync(criteria);
                        return criteria.IsHideIP;
                    });

                return isHideIP;
            }
            catch (Exception ex)
            {
                await Logger.Log.ErrorAsync($"CheckHideIP error for superId: {superId}, system: {system}, Message: {ex.Message}", ex);
                return false;
            }
        }

        internal static bool IsInternalByExtAdmin(string system)
            => system == ExtNAP;
    }
}