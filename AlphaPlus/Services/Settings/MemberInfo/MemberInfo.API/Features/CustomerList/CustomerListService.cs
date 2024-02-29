namespace AlphaPlus.Settings.MemberInfo.API.Features.CustomerList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.MemberInfo.API.Features.CustomerList.Criteria;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.CustomerList.Models;
    using AlphaPlus.Settings.MemberInfo.API.Features.CustomerList.SPs.Criteria;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Site.Features.CustomerList.Params;
    using AlphaPlus.Settings.MemberInfo.Domain.Site.Features.CustomerList.ViewModels;
    using Fanex.Data.Repository;

    public interface CustomerListService
    {
        CustomerListDTO GetCustomerList(CustomerListParam param);

        CustomerStatusModel GetCustomerStatus(int custId, byte roleId);

        Result UpdateCustomerStatus(UpdateCustomerStatusParam param);
    }

    public class CustomerListServiceImpl : CustomerListService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IDictionary<UserLevel, Func<CustomerListParam, CustomerListDTO>> fetchData;

        public CustomerListServiceImpl(IDynamicRepository dynamicRepository)
            : this(dynamicRepository, null)
        {
        }

        internal CustomerListServiceImpl(
            IDynamicRepository dynamicRepository,
            IDictionary<UserLevel, Func<CustomerListParam, CustomerListDTO>> fetchData)
        {
            this.dynamicRepository = dynamicRepository;
            this.fetchData = fetchData ??
                new Dictionary<UserLevel, Func<CustomerListParam, CustomerListDTO>>
                {
                    [UserLevel.Super] = FectchMasterList,
                    [UserLevel.Master] = FetchAgentList,
                    [UserLevel.Agent] = FetchMemberList
                };
        }

        public CustomerListDTO GetCustomerList(CustomerListParam param)
            => fetchData.ContainsKey(param.DirectUplineCustLevel) ? fetchData[param.DirectUplineCustLevel](param) :
                new CustomerListDTO(Enumerable.Empty<CustomerListModelBase>(), 0, 0);

        public CustomerStatusModel GetCustomerStatus(int custId, byte roleId)
        {
            var (customerStatus, nonDCSProduct, dcsProducts)
                = dynamicRepository.FetchMultiple<NonProductStatusModel, NonDCSProductStatusModel, DCSProductStatusModel>(
                    new SelectCustomerAllStatusModelCriteria(custId));

            var customerStatusModel = new CustomerStatusModel(
                customerStatus.FirstOrDefault(),
                nonDCSProduct.FirstOrDefault(),
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
                var closedReasonAndIsProtectedMode = dynamicRepository.Get<ClosedReasonAndIsProtectedMode>(
                    new SelectClosedReasonCriteria(custId, roleId));
                customerStatusModel.NonProduct.IsProtectedMode = closedReasonAndIsProtectedMode != null
                    && closedReasonAndIsProtectedMode.IsProtectedMode;
            }

            return customerStatusModel;
        }

        public Result UpdateCustomerStatus(UpdateCustomerStatusParam param)
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

            dynamicRepository.Execute(command);

            return new Result { Code = command.ErrCode, Message = command.ErrMsg };
        }

        internal CustomerListDTO FectchMasterList(CustomerListParam param)
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

            var model = dynamicRepository.Fetch<MasterListModel>(criteria);

            return new CustomerListDTO(model, criteria.TotalItems, criteria.TotalPages);
        }

        internal CustomerListDTO FetchAgentList(CustomerListParam param)
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

            var model = dynamicRepository.Fetch<AgentListModel>(criteria);

            return new CustomerListDTO(model, criteria.TotalItems, criteria.TotalPages);
        }

        internal CustomerListDTO FetchMemberList(CustomerListParam param)
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
                param.PageSize,
                param.PageIndex);

            var model = dynamicRepository.Fetch<MemberListModel>(criteria);

            return new CustomerListDTO(model, criteria.TotalItems, criteria.TotalPages);
        }
    }
}