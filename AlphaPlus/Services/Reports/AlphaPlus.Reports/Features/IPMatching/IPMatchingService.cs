using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.IPMatching;

public interface IIPMatchingService
{
    Task<ReportData<IPMatchingModel>> GetIPMatching(IPMatchingParam param);
}

public class IPMatchingService(
    IDynamicRepository repository,
    IReportDateService reportDateService,
    IPartialUMService partialUMService,
    IPService ipService) : IIPMatchingService
{
    public async Task<ReportData<IPMatchingModel>> GetIPMatching(IPMatchingParam param)
    {
        var moveBetsDay = await reportDateService.GetMoveBetsDay();
        var criteria = new GetIPMatchingCriteria(
            param.CustId,
            param.WinlostDate,
            moveBetsDay,
            param.MemberCount);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var rawdata = await repository.FetchAsync<IPMatchingModel>(criteria);

        return FilterIPVN(param.IsAdminCanViewVNIP, rawdata);
    }

    public ReportData<IPMatchingModel> FilterIPVN(bool isAdminCanViewVNIP, IEnumerable<IPMatchingModel> rawdata)
    {
        var filteredRawData = new List<IPMatchingModel>();
        var total = new IPMatchingModel();

        foreach (var item in rawdata)
        {
            if (isAdminCanViewVNIP || !ipService.IsVNIP(item.IP))
            {
                filteredRawData.Add(item);
                total += item;
            }
        }

        return new ReportData<IPMatchingModel>(filteredRawData, total);
    }
}