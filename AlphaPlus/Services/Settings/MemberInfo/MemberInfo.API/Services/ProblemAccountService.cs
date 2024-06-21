namespace AlphaPlus.Settings.MemberInfo.API.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Settings.MemberInfo.API.Criteria.ProblemAccount;
    using AlphaPlus.Settings.MemberInfo.Domain.Params.ProblemAccount;
    using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.ProblemAccount;
    using Fanex.Data.Repository;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;

    public interface IProblemAccountService
    {
        Result UpdateReviewStatus(UpdateProblemAccountParam param);

        IEnumerable<ProblemAccountModel> Get(GetProblemAccountParam param);
    }

    public class ProblemAccountService : IProblemAccountService
    {
        private readonly IDynamicRepository dynamicRepository;

        public ProblemAccountService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public Result UpdateReviewStatus(UpdateProblemAccountParam param)
        {
            var command = new UpdateReviewStatusProblemAccountCriteria(
                param.CustId,
                param.SubAccId,
                param.RoleId,
                param.CustomerList,
                param.IsAffectedAllDownlines,
                param.Language,
                param.LoginIp);
            dynamicRepository.Execute(command);

            return Result.CreateErrorResult(command.ErrCode == 0 ? Resources.UpdateSuccessfully : command.ErrMsg ?? string.Empty, command.ErrCode);
        }

        public IEnumerable<ProblemAccountModel> Get(GetProblemAccountParam param)
            => dynamicRepository.Fetch<ProblemAccountModel>(new GetProblemAccountCriteria(
                param.CustId,
                param.SubAccId,
                param.UplineUserName,
                param.SearchRoleId,
                param.SearchUserName,
                param.FromDate,
                param.ToDate,
                param.ReviewStatusId))
            .OrderBy(i => i.IsReviewed)
            .ThenByDescending(i => i.CreatedDate);
    }
}
