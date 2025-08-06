using AlphaPlus.Features.Messages.API.Criteria.Feedback;
using AlphaPlus.Features.Messages.API.Models.Feedback;
using AlphaPlus.Features.Messages.API.Params.Feedback;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Features.Messages.API.Services;

public interface FeedbackService
{
    Task<bool> CheckExistsAsync(int custId, int subAccId, int functionId);

    Task<(IEnumerable<FeedbackFunction>, IEnumerable<FeedbackArea>)> GetFeedbackFunctionAsync(int functionId);

    Task AddFeedbackAsync(AddFeedbackParam param);
}

public class FeedbackServiceImpl : FeedbackService
{
    private const string FeedBackOnFunctionCacheKey = "AlphaPlus.Features.Messages.API.Services.FeedbackService.FeedBackOnFunction_{0}";
    private const int FeedBackOnFunctionCacheInMinutes = 60 * 2;
    private const string FeedBackNewStatusIdCacheKey = "AlphaPlus.Features.Messages.API.Services.FeedbackService.FeedBackNewStatusId";

    private readonly IDynamicRepository dynamicRepository;
    private readonly IMemoryCache cacheService;

    public FeedbackServiceImpl(IDynamicRepository dynamicRepository, IMemoryCache cacheService)
    {
        this.dynamicRepository = dynamicRepository;
        this.cacheService = cacheService;
    }

    public async Task<bool> CheckExistsAsync(int custId, int subAccId, int functionId)
    {
        var criteria = new CheckExistsCriteria(custId, subAccId, functionId);
        await dynamicRepository.ExecuteAsync(criteria);

        return criteria.IsExists;
    }

    public Task<(IEnumerable<FeedbackFunction>, IEnumerable<FeedbackArea>)> GetFeedbackFunctionAsync(int functionId)
        => cacheService.GetOrCreateAsync(
        string.Format(FeedBackOnFunctionCacheKey, functionId),
        cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(FeedBackOnFunctionCacheInMinutes));
            return dynamicRepository.FetchMultipleAsync<FeedbackFunction, FeedbackArea>(new GetFeedbackFunctionCriteria(functionId));
        });

    public async Task AddFeedbackAsync(AddFeedbackParam param)
        => await dynamicRepository.ExecuteAsync(new AddFeedbackCriteria(
            param.CustId,
            param.SubAccountId,
            param.Comment,
            param.Rate,
            param.FunctionId,
            param.XMLFeedback,
            await GetFeedbackNewStatusId(),
            param.ExtraInfo));

    private Task<int> GetFeedbackNewStatusId()
        => cacheService.GetOrCreateAsync(
            FeedBackNewStatusIdCacheKey,
            async cacheEntry =>
            {
                var feedbacks = await dynamicRepository.FetchAsync<FeedbackStatus>(new GetAllFeedbackStatusCriteria());
                var feedbackWithNewStatus = feedbacks.FirstOrDefault(f => f.SettingValue.Equals("New", StringComparison.OrdinalIgnoreCase));
                var newStatusId = feedbackWithNewStatus == null ? 0 : feedbackWithNewStatus.SettingId;

                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(FeedBackOnFunctionCacheInMinutes));
                return newStatusId;
            });
}
