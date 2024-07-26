namespace AlphaPlus.Settings.MemberInfo.API.Services
{
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Params;
    using Criteria;
    using Fanex.Data.Repository;

    public interface CheckUMService
    {
        Result CheckUM(CheckUMParam param);
    }

    public class CheckUMServiceImpl : CheckUMService
    {
        private readonly CacheRefService refService;
        private readonly IDynamicRepository repository;

        public CheckUMServiceImpl(CacheRefService refService, IDynamicRepository repository)
        {
            this.refService = refService;
            this.repository = repository;
        }

        public Result CheckUM(CheckUMParam param)
        {
            var criteria = new CheckUMCriteria(param.CustId, param.ProductId, param.Language);

            repository.Execute(criteria);

            return new Result
            {
                Code = criteria.ErrCode,
                Message = string.Format(criteria.ErrMsg ?? string.Empty, refService.GetProductName(param.ProductId))
            };
        }
    }
}