using System.Text.Json;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance.Criteria;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Criteria;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.CustomerStatus;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.CustomerStatus.Criteria;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.DonwlineList;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.LoginInfo;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Models;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.OnlineList;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.Core.Models;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount
{
    public interface ISearchAccountService
    {
        Task<Account> GetAccount(string username, int uplineId, int roleId);

        Task<CreditInfo> GetCreditInfo(int custId);

        Task<IBalanceInfoModel> GetBalanceInfo(int custId, byte roleId, string agIP);

        Task<bool> IsAllDayBalanceTransfer(int custId);

        Task<bool> CheckFirstLoginByQRCode(int custId);

        Task<DownlineListModel> GetDownlineList(int custId, int pageIndex, byte roleId, byte statusId, int totalPages);

        Task<IEnumerable<TotalDownlinesSummary>> GetTotalDownlinesSummary(int custId, byte roleId);

        Task<DateTime> GetLastTxn(string username, int uplineId, bool isInternal);

        Task<IEnumerable<OnlineUser>> GetOnlineList(int custid, bool adminCanViewVNIP);

        Task<Result> UpdateGivenCredit(GivenCreditCommand param);

        Task<Result> UpdateCustomerStatus(CustomerNonProductStatusCommand param);
    }


    public class SearchAccountService(IDynamicRepository dynamicRepository, IPService ipService) : ISearchAccountService
    {
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;
        private readonly IPService ipService = ipService;
        private readonly int failErrorCode = 100;

        public async Task<Account> GetAccount(string username, int uplineId, int roleId)
        {
            var customerInfo = await dynamicRepository.GetAsync<Account>(new SelectCustomerInfoCriteria(username, uplineId));
            if (customerInfo != null)
            {
                var custClosedReason = await dynamicRepository.GetAsync<CustomerClosedReason>(
                    new SelectClosedReasonCriteria(customerInfo.CustId, customerInfo.RoleId));
                customerInfo.ClosedReason = custClosedReason?.ClosedReason ?? 0;

                var nickname = roleId - customerInfo.RoleId == 1 ? customerInfo.UserName2 : string.Empty;
                await UpdateUserSearch(uplineId, customerInfo.UserName, customerInfo.FirstName, customerInfo.LastName, nickname);
            }

            return customerInfo;
        }

        private Task<bool> UpdateUserSearch(int userId, string search, string firstName, string lastName, string nickname)
           => dynamicRepository.GetAsync<bool>(new UpdateUserSearchCriteria(userId, search, firstName, lastName, nickname));

        public Task<CreditInfo> GetCreditInfo(int custId)
            => dynamicRepository.GetAsync<CreditInfo>(new SelectCreditInfoCriteria(custId));

        public Task<IBalanceInfoModel> GetBalanceInfo(int custId, byte roleId, string agIP)
        {
            var userLevel = Enumeration.FromValue<UserLevel>(roleId);
            var criteria = new SelectBalanceInfoCriteria(custId, roleId);

            if (userLevel.IsMasterLevel())
            {
                return GetMasterBalanceInfoModel(criteria);
            }

            if (userLevel.IsAgentLevel())
            {
                return GetAgentBalanceInfoModel(criteria);
            }

            if (userLevel.IsMemberLevel())
            {
                return GetMemberBalanceInfoModel(criteria, agIP);
            }

            return null;
        }

        internal async Task<IBalanceInfoModel> GetMasterBalanceInfoModel(SelectBalanceInfoCriteria criteria)
          => await dynamicRepository.GetAsync<MasterBalanceInfoModel>(criteria);

        internal async Task<IBalanceInfoModel> GetAgentBalanceInfoModel(SelectBalanceInfoCriteria criteria)
          => await dynamicRepository.GetAsync<AgentBalanceInfoModel>(criteria);

        internal async Task<IBalanceInfoModel> GetMemberBalanceInfoModel(SelectBalanceInfoCriteria criteria, string agIP)
        {
            var tasks = new List<Task>();

            var memberOutstandingCriteria = new SelectMemberOutstandingCriteria(criteria.CustId);
            var memberBalanceCriteria = new SelectMemberBalanceCriteria(criteria.CustId);
            var memberBalanceInfoDTOTask = dynamicRepository.GetAsync<MemberBalanceInfoDTO>(criteria);

            tasks.Add(dynamicRepository.ExecuteAsync(memberOutstandingCriteria));
            tasks.Add(dynamicRepository.ExecuteAsync(memberBalanceCriteria, agIP));
            tasks.Add(memberBalanceInfoDTOTask);

            await Task.WhenAll(tasks);

            return new MemberBalanceInfoModel(
                await memberBalanceInfoDTOTask,
                memberBalanceCriteria.Balance,
                memberBalanceCriteria.PendingTransfer3rd,
                memberOutstandingCriteria.Outstanding,
                memberBalanceCriteria.NetPosition);
        }

        public async Task<bool> IsAllDayBalanceTransfer(int custId)
        {
            var transferModel = await dynamicRepository.GetAsync<TransferModel>(new SelectIsAllDayBalTransferCriteria(custId));

            return transferModel != null && transferModel.IsAllDayBalTransfer;
        }

        public Task<bool> CheckFirstLoginByQRCode(int custId)
            => dynamicRepository.GetAsync<bool>(new SelectFirstLoginByQRCodeCriteria(custId));

        public async Task<DownlineListModel> GetDownlineList(int custId, int pageIndex, byte roleId, byte statusId, int totalPages)
        {
            var (downlineListModel, downlines) = await dynamicRepository.FetchMultipleAsync<DownlineListModel, DownlineDto>(
                new SelectDownlineListByStatusCriteria(custId, pageIndex, roleId, statusId, totalPages));
            var downlineListFirst = downlineListModel.FirstOrDefault();

            return downlineListFirst == null ? downlineListFirst : new DownlineListModel(downlines, downlineListFirst);
        }

        public async Task<IEnumerable<TotalDownlinesSummary>> GetTotalDownlinesSummary(int custId, byte roleId)
            => await dynamicRepository.FetchAsync<TotalDownlinesSummary>(new SelectTotalDownlineSummaryCriteria(custId, roleId));

        public async Task<DateTime> GetLastTxn(string username, int uplineId, bool isInternal)
        {
            var lastTxn = await dynamicRepository.GetAsync<DateTime?>(
                new SelectLastTxnCriteria(username, uplineId, isInternal));

            return lastTxn ?? DateTime.MinValue;
        }

        public async Task<IEnumerable<OnlineUser>> GetOnlineList(int custid, bool adminCanViewVNIP)
        {
            var onlineUsers = await dynamicRepository.FetchAsync<OnlineUser>(
                new SelectOnlineListCriteria(custid));

            if (!adminCanViewVNIP)
            {
                foreach (var item in onlineUsers)
                {
                    item.IP = ipService.IsVNIP(item.IP) ? string.Empty : item.IP;
                }
            }

            return onlineUsers;
        }

        public async Task<Result> UpdateGivenCredit(GivenCreditCommand param)
        {
            UpdateSMAGivenCreditCommand command;
            if (param.SearchAccountRoleId == UserLevel.Member.Value)
            {
                var totalAmtCommand = new GetTotalAmtCommand(param.SearchAccountId);
                await dynamicRepository.ExecuteAsync(totalAmtCommand, param.AGIP);
                command = new UpdateMemberGivenCreditCommand(
                    param.SearchAccountId,
                    param.SearchAccountRoleId,
                    param.CreditAmount,
                    param.ViewerId,
                    param.SubAccountId,
                    param.LangKey,
                    param.IP,
                    totalAmtCommand.TotalAmt);
                await dynamicRepository.ExecuteAsync(command);
            }
            else
            {
                command = new UpdateSMAGivenCreditCommand(
                    param.SearchAccountId,
                    param.SearchAccountRoleId,
                    param.CreditAmount,
                    param.ViewerId,
                    param.SubAccountId,
                    param.LangKey,
                    param.IP);

                await dynamicRepository.ExecuteAsync(command);
            }

            if (command.IsInvalid() || command.CreditProblem)
            {
                return Result.CreateErrorResult(command.ErrMsg, failErrorCode);
            }
            else
            {

                var replicateServerCommand = new UpdateGivenCreditReplicateServerCommand(param.SearchAccountId, param.CreditAmount);
                await dynamicRepository.ExecuteAsync(replicateServerCommand);
                return Result.CreateSuccessfulResult();
            }
        }

        public async Task<Result> UpdateCustomerStatus(CustomerNonProductStatusCommand param)
        {
            UpdateCustomerStatusCommand command = param.StatusName switch
            {
                StatusNames.Closed => new UpdateClosedStatusCommand(
                                        param.SearchAccountId,
                                        param.ViewerId,
                                        param.SubAccountId,
                                        param.LangKey,
                                        param.IP,
                                        param.StatusName,
                                        param.StatusValue),
                StatusNames.DisableAutoMasterPT => new UpdateDisableAutoMPTStatusCommand(
                                        param.SearchAccountId,
                                        param.ViewerId,
                                        param.SubAccountId,
                                        param.LangKey,
                                        param.IP,
                                        param.StatusName,
                                        param.StatusValue),
                StatusNames.DoubleComm => new UpdateDoubleCommLosingStatusCommand(
                                        param.SearchAccountId,
                                        param.ViewerId,
                                        param.SubAccountId,
                                        param.LangKey,
                                        param.IP,
                                        param.StatusName,
                                        param.StatusValue),
                StatusNames.DisableOutright => new UpdateDisableOutrightStatusCommand(
                                        param.SearchAccountId,
                                        param.ViewerId,
                                        param.SubAccountId,
                                        param.LangKey,
                                        param.IP,
                                        param.StatusName,
                                        param.StatusValue),
                StatusNames.Suspended => new UpdateSuspendedStatusCommand(
                                        param.SearchAccountId,
                                        param.ViewerId,
                                        param.SubAccountId,
                                        param.LangKey,
                                        param.IP,
                                        param.StatusName,
                                        param.StatusValue),
                StatusNames.Unlock => new UpdateUnlockedCommand(
                                        param.SearchAccountId,
                                        param.ViewerId,
                                        param.SubAccountId,
                                        param.LangKey,
                                        param.IP,
                                        param.StatusName),
                _ => new UpdateDisableProtectedStatusCommand(
                                        param.SearchAccountId,
                                        param.ViewerId,
                                        param.SubAccountId,
                                        param.LangKey,
                                        param.IP,
                                        param.StatusName,
                                        param.StatusValue),
            };
            await dynamicRepository.ExecuteAsync(command);
            return new Result(command.ErrCode, command.ErrMsg);
        }
    }
}