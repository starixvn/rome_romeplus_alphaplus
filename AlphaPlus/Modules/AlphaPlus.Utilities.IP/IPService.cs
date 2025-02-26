using AlphaPlus.Utilities.IP.Models;
using AlphaPlus.Utilities.IP.Criteria;
using AlphaPlus.Utilities.IP.Model;
using Fanex.Data.Repository;
using Fanex.Caching;
using System.Net;
using System.Text.RegularExpressions;

namespace AlphaPlus.Utilities.IP
{
    public interface IPService
    {
        bool IsVNIP(string ipAddress);

        FormatedIP FormatIP(string ip, bool isAdminCanViewVNIP);

        Task<IPInformationModel> GetIPInformation(string ip);

        Task<string> GetCountryCode(string ip);
    }

    public class IPServiceImpl : IPService
    {
        private const string IPValidateRegex = @"(?:(?:25[0-5]|2[0-4]\d|[01]?\d\d?)\.){3}(?:25[0-5]|2[0-4]\d|[01]?\d\d?)";
        private const string VNIPListCacheKey = "Alpha.BetLists.Service.IPService.VNIPList";
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPHelper ipHelper;

        public IPServiceImpl(IDynamicRepository dynamicRepository, IPHelper ipHelper)
        {
            this.dynamicRepository = dynamicRepository;
            this.ipHelper = ipHelper;
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
            var cache = new CacheService();

            var vietnamIPs = cache.GetOrSet(
                VNIPListCacheKey,
                () => dynamicRepository.FetchMultiple<Ipv4Range, Ipv6Range>(new GetIPVNCriteria()));

            return vietnamIPs;
        }


        private static bool IsValidIP(string ipAddress)
        {
            IPAddress parsedIpAddress;
            return !string.IsNullOrWhiteSpace(ipAddress)
                && IPAddress.TryParse(ipAddress, out parsedIpAddress);
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
    }
}