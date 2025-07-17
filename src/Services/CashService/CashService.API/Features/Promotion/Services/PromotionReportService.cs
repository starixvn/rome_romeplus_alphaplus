using AlphaPlus.CashService.API.Features.Promotion.Criteria;
using AlphaPlus.CashService.API.Features.Promotion.Models;
using AlphaPlus.CashService.API.Features.Promotion.Requests;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Promotion.Services;

public interface IPromotionReportService
{
    Task<IEnumerable<PromotionReport>> GetMainReport(GetPromotionReportRequest request);

    Task<PromotionDetailsReport> GetDetailsReport(GetPromotionDetailsReportRequest request);
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

    public async Task<PromotionDetailsReport> GetDetailsReport(GetPromotionDetailsReportRequest request)
    {
        var criteria = new GetPromotionDetailsReportCriteria(
            request.CustId,
            request.PromotionId,
            request.FromDate,
            request.ToDate,
            request.Status);

        var promotionDetails = await repository.FetchAsync<PromotionDetails>(criteria);
        var totalBonus = 0m;
        var totalApproved = 0;
        var totalRejected = 0;

        foreach (var item in promotionDetails)
        {
            if (item.StatusId == 4) // Approved
            {
                totalApproved++;
                totalBonus += item.Amount; // Assuming Amount is the bonus amount
            }
            else if (item.StatusId == 5) // Rejected
            {
                totalRejected++;
            }
        }

        return new PromotionDetailsReport(
            new PromotionDetailsSummary(totalBonus, totalApproved, totalRejected),
            promotionDetails);
    }
}
