using Fanex.Data.Repository;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.CustomerList.Criteria;
using AlphaPlus.Settings.Site.Features.CustomerList.Params;
using AlphaPlus.Settings.Site.Features.CustomerList.Models;
using AlphaPlus.Settings.Site.Features.Shared.Models;
using AlphaPlus.Settings.MemberInfo.API.Services.Transfer;

namespace AlphaPlus.Settings.Site.Features.CustomerList
{
    public interface CustomerListService
    {
        Task<CustomerListDTO> GetCustomerList(CustomerListParam param);

        Task<CustomerStatusModel> GetCustomerStatus(int custId, byte roleId, int uplineId);

        Task<Result> UpdateCustomerStatus(UpdateCustomerStatusParam param);
    }

    public class CustomerListServiceImpl(IDynamicRepository dynamicRepository, TransferService transferService) : CustomerListService
    {
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;
        private readonly TransferService transferService = transferService;

        public Task<CustomerListDTO> GetCustomerList(CustomerListParam param)
        {
            if (param.IsDirectMember)
            {
                return FetchMemberList(param);
            }

            return param.DirectUplineCustLevel.Value switch
            {
                4 => FetchMasterList(param),
                3 => FetchAgentList(param),
                2 => FetchMemberList(param),
                _ => Task.FromResult(new CustomerListDTO([], 0, 0)),
            };
        }

        public async Task<CustomerStatusModel> GetCustomerStatus(int custId, byte roleId, int uplineId)
        {
            var (customerStatus, dcsProducts)
                = await dynamicRepository.FetchMultipleAsync<NonProductStatusModel, DCSProductStatusModel>(
                    new SelectCustomerAllStatusModelCriteria(custId));

            var customerStatusModel = new CustomerStatusModel(
                customerStatus.FirstOrDefault(),
                dcsProducts
                    .GroupBy(product => product.ProductId)
                    .Select(products =>
                    {
                        foreach (var product in products)
                        {
                            if (!product.IsHidden)
                            {
                                return product;
                            }
                        }

                        return products.First();
                    }));

            if (customerStatusModel.NonProduct != null)
            {
                var closedReasonAndIsProtectedMode = await dynamicRepository.GetAsync<ClosedReasonAndIsProtectedMode>(
                    new SelectClosedReasonCriteria(custId, roleId));
                customerStatusModel.NonProduct.IsProtectedMode = closedReasonAndIsProtectedMode != null
                    && closedReasonAndIsProtectedMode.IsProtectedMode;

                var scheduleOptionCommand = new SelectScheduleOptionCommand(custId);
                await dynamicRepository.ExecuteAsync(scheduleOptionCommand);
                customerStatusModel.NonProduct.IsAllowAutoTransfer = scheduleOptionCommand.IsAllowAutoTransfer;
                customerStatusModel.NonProduct.ScheduleWeekDays = scheduleOptionCommand.ScheduleWeekDays;
                customerStatusModel.NonProduct.TransferOption = await transferService.GetTransferOption(uplineId);
            }

            return customerStatusModel;
        }

        public async Task<Result> UpdateCustomerStatus(UpdateCustomerStatusParam param)
        {
            UpdateCustomerStatusCommand command = null;
            switch (param.StatusName)
            {
                case StatusNames.Closed:
                    command = new UpdateClosedStatusCommand(
                        param.SearchAccountId,
                        param.ActorId,
                        param.SubAccountId,
                        param.Language,
                        param.IP,
                        param.StatusName,
                        param.StatusValue);
                    break;

                case StatusNames.DisableAutoMasterPT:
                    command = new UpdateDisableAutoMPTStatusCommand(
                        param.SearchAccountId,
                        param.ActorId,
                        param.SubAccountId,
                        param.Language,
                        param.IP,
                        param.StatusName,
                        param.StatusValue);
                    break;

                case StatusNames.DoubleComm:
                    command = new UpdateDoubleCommLosingStatusCommand(
                        param.SearchAccountId,
                        param.ActorId,
                        param.SubAccountId,
                        param.Language,
                        param.IP,
                        param.StatusName,
                        param.StatusValue);
                    break;

                case StatusNames.DisableOutright:
                    command = new UpdateDisableOutrightStatusCommand(
                        param.SearchAccountId,
                        param.ActorId,
                        param.SubAccountId,
                        param.Language,
                        param.IP,
                        param.StatusName,
                        param.StatusValue);
                    break;

                case StatusNames.Suspended:
                    command = new UpdateSuspendedStatusCommand(
                        param.SearchAccountId,
                        param.ActorId,
                        param.SubAccountId,
                        param.Language,
                        param.IP,
                        param.StatusName,
                        param.StatusValue);
                    break;

                case StatusNames.Unlock:
                    command = new UpdateUnlockedCommand(
                        param.SearchAccountId,
                        param.ActorId,
                        param.SubAccountId,
                        param.Language,
                        param.IP,
                        param.StatusName);
                    break;

                case StatusNames.AllowAutoTransfer:
                    command = new UpdateAllowAutoTransferStatusCommand(
                        param.SearchAccountId,
                        param.ActorId,
                        param.SubAccountId,
                        param.Language,
                        param.IP,
                        param.StatusName,
                        param.StatusValue);
                    break;

                default:
                    return new Result();
            }

            await dynamicRepository.ExecuteAsync(command);

            return new Result { Code = command.ErrCode, Message = command.ErrMsg };
        }

        internal async Task<CustomerListDTO> FetchMasterList(CustomerListParam param)
        {
            var criteria = new SelectMasterListCriteria(
              param.DirectUplineId,
              param.Username,
              param.Enabled,
              param.Closed,
              param.Suspended,
              param.Locked,
              param.DoubleCommLosing,
              param.AllowAutoTransfer,
              param.PageSize,
              param.PageIndex);

            var model = await dynamicRepository.FetchAsync<MasterListModel>(criteria);

            return new CustomerListDTO(model, criteria.TotalItems, criteria.TotalPages);
        }

        internal async Task<CustomerListDTO> FetchAgentList(CustomerListParam param)
        {
            var criteria = new SelectAgentListCriteria(
                param.UserId,
                param.DirectUplineId,
                param.Username,
                param.Enabled,
                param.Closed,
                param.Suspended,
                param.Locked,
                param.DoubleCommLosing,
                param.AllowAutoTransfer,
                param.PageSize,
                param.PageIndex);

            var model = await dynamicRepository.FetchAsync<AgentListModel>(criteria);

            return new CustomerListDTO(model, criteria.TotalItems, criteria.TotalPages);
        }

        internal async Task<CustomerListDTO> FetchMemberList(CustomerListParam param)
        {
            var criteria = new SelectMemberListCriteria(
                param.UserId,
                param.DirectUplineId,
                param.Username,
                param.Enabled,
                param.Closed,
                param.Suspended,
                param.Locked,
                param.DoubleCommLosing,
                param.AllowAutoTransfer,
                param.ServiceType,
                param.PageSize,
                param.PageIndex);

            var model = await dynamicRepository.FetchAsync<MemberListModel>(criteria);

            return new CustomerListDTO(model, criteria.TotalItems, criteria.TotalPages);
        }
    }
}