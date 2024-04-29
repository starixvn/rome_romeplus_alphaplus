using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.AccountInfo.Criteria;
using AlphaPlus.Settings.Site.Features.AccountInfo.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.AccountInfo
{
    public interface IAccountInfoService
    {
        Task<IEnumerable<TotalDownlinesSummary>> GetTotalDownlinesSummary(int custId, byte roleId);

        Task<DownlineListModel> GetDownlineList(int custId, int pageIndex, byte roleId, byte statusId, int totalPages);

        Task<CustomerInfoModel> GetCustomerInfo(string username, int uplineId, int roleId);

        Task<CreditInfo> GetCreditInfo(int custId);

        Task<IBalanceInfoModel> GetBalanceInfo(int custId, byte roleId, string agIP);

        Task<bool> IsAllDayBalanceTransfer(int custId);

        Task<bool> CheckFirstLoginByQRCode(int custId);
    }

    public class AccountInfoService(IDynamicRepository dynamicRepository) : IAccountInfoService
    {
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

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

        public Task<CreditInfo> GetCreditInfo(int custId)
            => dynamicRepository.GetAsync<CreditInfo>(new SelectCreditInfoCriteria(custId));

        public async Task<CustomerInfoModel> GetCustomerInfo(string username, int uplineId, int roleId)
        {
            var customerInfo = await dynamicRepository.GetAsync<CustomerInfoModel>(new SelectCustomerInfoCriteria(username, uplineId));
            if (customerInfo != null)
            {
                var custClosedReason = await dynamicRepository.GetAsync<CustomerClosedReasonModel>(
                    new SelectClosedReasonCriteria(customerInfo.CustId, customerInfo.RoleId));
                customerInfo.ClosedReason = custClosedReason?.ClosedReason ?? 0;

                var nickname = roleId - customerInfo.RoleId == 1 ? customerInfo.UserName2 : string.Empty;
                await UpdateUserSearch(uplineId, customerInfo.UserName, customerInfo.FirstName, customerInfo.LastName, nickname);
            }

            return customerInfo;
        }

        public Task<bool> UpdateUserSearch(int userId, string search, string firstName, string lastName, string nickname)
           => dynamicRepository.GetAsync<bool>(new UpdateUserSearchCriteria(userId, search, firstName, lastName, nickname));

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
    }
}