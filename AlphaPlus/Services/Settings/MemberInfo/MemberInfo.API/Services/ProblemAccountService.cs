using AlphaPlus.Settings.MemberInfo.API.Criteria.ProblemAccount;
using AlphaPlus.Settings.MemberInfo.Domain.Params.ProblemAccount;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.Domain.Models.ProblemAccount;
using Fanex.Data.Repository;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.MemberInfo.API.Services
{
    public interface IProblemAccountService
    {
        Task<Result> UpdateReviewStatus(UpdateProblemAccountParam param);

        Task<IEnumerable<ProblemAccountModel>> Get(GetProblemAccountParam param);
    }

    public class ProblemAccountService : IProblemAccountService
    {
        private readonly IDynamicRepository dynamicRepository;

        public ProblemAccountService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<Result> UpdateReviewStatus(UpdateProblemAccountParam param)
        {
            var command = new UpdateReviewStatusProblemAccountCriteria(
                param.CustId,
                param.SubAccId,
                param.RoleId,
                param.CustomerList,
                param.IsAffectedAllDownlines,
                param.Language,
                param.LoginIp);
            await dynamicRepository.ExecuteAsync(command);

            return Result.CreateErrorResult(command.ErrCode == 0 ? Resources.UpdateSuccessfully : command.ErrMsg ?? string.Empty, command.ErrCode);
        }

        public async Task<IEnumerable<ProblemAccountModel>> Get(GetProblemAccountParam param)
            => (await dynamicRepository.FetchAsync<ProblemAccountModel>(new GetProblemAccountCriteria(
                param.CustId,
                param.SubAccId,
                param.UplineUserName,
                param.SearchRoleId,
                param.SearchUserName,
                param.FromDate,
                param.ToDate,
                param.ReviewStatusId)))
            .OrderBy(i => i.IsReviewed)
            .ThenByDescending(i => i.CreatedDate);
    }
}
