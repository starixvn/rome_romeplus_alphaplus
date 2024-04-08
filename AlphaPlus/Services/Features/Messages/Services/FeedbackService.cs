namespace AlphaPlus.Features.Messages.API.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AlphaPlus.Features.Messages.API.Criteria.Feedback;
    using AlphaPlus.Features.Messages.API.Models.Feedback;
    using AlphaPlus.Features.Messages.API.Params.Feedback;
    using Fanex.Caching;
    using Fanex.Data.Repository;

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

        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;

        public FeedbackServiceImpl(IDynamicRepository dynamicRepository, ICacheService cacheService)
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
            => cacheService.GetOrSetAsync(
            string.Format(FeedBackOnFunctionCacheKey, functionId),
            () => dynamicRepository.FetchMultipleAsync<FeedbackFunction, FeedbackArea>(new GetFeedbackFunctionCriteria(functionId)),
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(FeedBackOnFunctionCacheInMinutes)));

        public Task AddFeedbackAsync(AddFeedbackParam param)
            => dynamicRepository.ExecuteAsync(new AddFeedbackCriteria(
                param.CustId,
                param.SubAccountId,
                param.Comment,
                param.Rate,
                param.FunctionId,
                param.XMLFeedback));
    }
}