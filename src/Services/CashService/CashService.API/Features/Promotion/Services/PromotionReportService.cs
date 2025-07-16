using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using AlphaPlus.CashService.API.Features.Promotion.Models;
using AlphaPlus.CashService.API.Features.Promotion.Requests;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Promotion.Services;

public interface IPromotionReportService
{
    Task<IEnumerable<PromotionReport>> GetMainReport(GetPromotionReportRequest request);
}

public class PromotionReportService(IDynamicRepository repository) : IPromotionReportService
{
    private readonly IDynamicRepository repository = repository;

    public Task<IEnumerable<PromotionReport>> GetMainReport(GetPromotionReportRequest request)
    {
        var criteria = new GetPromotionReportCriteria(
            request.CustId,
            request.FromDate,
            request.ToDate,
            request.PromotionType,
            request.Status);

        return repository.FetchAsync<PromotionReport>(criteria);
    }
}
