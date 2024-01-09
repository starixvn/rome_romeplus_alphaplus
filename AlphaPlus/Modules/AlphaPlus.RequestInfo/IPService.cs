using AlphaPlus.RequestInfo.Criteria;
using AlphaPlus.RequestInfo.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.RequestInfo;

public interface IPService
{
    Task<IPInformationModel> GetIPInformation(string ip);

    Task<string> GetCountryCode(string ip);
}

public class IPServiceImpl : IPService
{
    private readonly IDynamicRepository dynamicRepository;

    public IPServiceImpl(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public Task<IPInformationModel> GetIPInformation(string ip)
        => dynamicRepository.GetAsync<IPInformationModel>(new GetIPInformationCriteria { Ip = ip });

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
