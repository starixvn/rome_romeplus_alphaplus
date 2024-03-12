using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.IPMatching;

public interface IIPMatchingService
{
    Task<IEnumerable<IPMatchingModel>> GetIPMatching(IPMatchingParam param);
}

public class IPMatchingService(
    IDynamicRepository repository,
    IReportDateService reportDateService,
    IPartialUMService partialUMService) : IIPMatchingService
{
    public async Task<IEnumerable<IPMatchingModel>> GetIPMatching(IPMatchingParam param)
    {
        var moveBetsDay = await reportDateService.GetMoveBetsDay();
        var criteria = new GetIPMatchingCriteria(
            param.CustId,
            param.WinlostDate,
            moveBetsDay,
            param.MemberCount);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        return await repository.FetchAsync<IPMatchingModel>(criteria);
    }
}