using AlphaPlus.AccountInfo.Site.Features.ProtectedStatus.Criteria;
using AlphaPlus.AccountInfo.Site.Features.ProtectedStatus.Params;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.CustomerStatus;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.AccountInfo;
using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.ProtectedStatus
{
    public interface IProtectedStatusService
    {
        Task<IEnumerable<CustomerProtectedStatus>> GetProtectedStatus(GetProtectedStatusParam param);

        Task<IEnumerable<Result>> UpdateMultiple(UpdateMultipleProtectedStatusParam param);
    }

    public class ProtectedStatusService : IProtectedStatusService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ISearchAccountService searchAccountService;
        private readonly IKickOutCustomerService kickOutCustomerService;

        public ProtectedStatusService(IDynamicRepository dynamicRepository, ISearchAccountService searchAccountService, IKickOutCustomerService kickOutCustomerService)
        {
            this.dynamicRepository = dynamicRepository;
            this.searchAccountService = searchAccountService;
            this.kickOutCustomerService = kickOutCustomerService;
        }

        public Task<IEnumerable<CustomerProtectedStatus>> GetProtectedStatus(GetProtectedStatusParam param)
            => dynamicRepository.FetchAsync<CustomerProtectedStatus>(
                new GetProtectedStatusCriteria(param.ViewerId, param.ViewerLevelId, param.CustIds));

        public async Task<IEnumerable<Result>> UpdateMultiple(UpdateMultipleProtectedStatusParam param)
        {
            var custIds = string.Join("^", param.ProtectedStatus.Select(p => p.CustId));
            var isValidCheckMultipleCustomersBelonging = await searchAccountService.CheckMultipleCustomersBelonging(param.ActorId, param.SubAccId, custIds);

            return isValidCheckMultipleCustomersBelonging
                ? await UpdateMultipleProtectedStatus(param)
                : [Result.CreateErrorResult("You are not authorized on this function.", StatusCodes.Status403Forbidden)];
        }

        private async Task<IEnumerable<Result>> UpdateMultipleProtectedStatus(UpdateMultipleProtectedStatusParam param)
        {
            Result result;
            IList<Result> errorResults = [];

            foreach (var item in param.ProtectedStatus)
            {
                result = await searchAccountService.UpdateCustomerStatus(
                    new UpdateCustomerStatusParam(
                        item.CustId,
                        param.ActorId,
                        param.SubAccId,
                        param.LangKey,
                        param.IP,
                        item.IsProtectedMode,
                        StatusNames.Protected,
                        param.ActorName));

                if (result.IsSuccessful)
                {
                    if (item.IsProtectedMode)
                    {
                        await kickOutCustomerService.KickOutDueToProtectedStatus(item.CustId, item.RoleId, param.ServerId, param.IP);
                    }
                }
                else
                {
                    errorResults.Add(Result.CreateErrorResult($"{item.Username}: {result.Message}", result.Code));
                }
            }

            return BuildResults(param.ProtectedStatus.Length, errorResults);
        }

        private static IEnumerable<Result> BuildResults(int total, IList<Result> errorResults)
        {
            var errorCount = errorResults.Count;

            if (errorCount == 0)
            {
                errorResults.Add(Result.CreateSuccessfulResult(ProtectedStatusResources.EditMultipleAllSuccessfully));
            }
            else if (errorCount < total)
            {
                errorResults.Insert(0, Result.CreateSuccessfulResult(ProtectedStatusResources.EditMultiplePartialSuccessfully));
            }

            return errorResults;
        }
    }
}