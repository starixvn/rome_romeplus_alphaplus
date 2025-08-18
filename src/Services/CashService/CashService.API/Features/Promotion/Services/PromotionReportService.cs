using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using AlphaPlus.CashService.API.Features.Promotion.Models;
using AlphaPlus.CashService.API.Features.Promotion.Requests;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Promotion.Services;

public interface IPromotionReportService
{
    Task<IEnumerable<PromotionReport>> GetMainReport(GetPromotionReportRequest request);

    Task<PromotionDetailsReport> GetDetailsReport(int promotionId, GetPromotionDetailsReportRequest request);

    Task<IEnumerable<ManualBonus>> GetManualBonus(GetManualBonusRequest request);
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

    public async Task<PromotionDetailsReport> GetDetailsReport(int promotionId, GetPromotionDetailsReportRequest request)
    {
        var criteria = new GetPromotionDetailsReportCriteria(
            request.CustId,
            promotionId,
            request.FromDate,
            request.ToDate,
            request.Status);

        var (promotionDetails, summary) = await repository.FetchMultipleAsync<PromotionDetails, PromotionDetailsSummary>(criteria);

        return new PromotionDetailsReport(summary.FirstOrDefault(), promotionDetails);
    }

    public Task<IEnumerable<ManualBonus>> GetManualBonus(GetManualBonusRequest request)
    {
        var criteria = new GetManualBonusCriteria(
            request.CustId,
            request.FromDate,
            request.ToDate);

        return repository.FetchAsync<ManualBonus>(criteria);
    }
}
