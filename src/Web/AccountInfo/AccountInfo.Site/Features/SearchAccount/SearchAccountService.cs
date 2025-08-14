using System.Globalization;
using System.Resources;
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
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.Update.Balance;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;
using Newtonsoft.Json;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount;

public interface ISearchAccountService
{
    Task<CustomerInfoModel> GetCustomerInfo(string username, int uplineId, int roleId);

    Task<CreditInfo> GetCreditInfo(int custId);

    Task<IBalanceInfoModel> GetBalanceInfo(int custId, byte roleId, string agIP);

    Task<bool> IsAllDayBalanceTransfer(int custId);

    Task<bool> CheckFirstLoginByQRCode(int custId);

    Task<DownlineListModel> GetDownlineList(int custId, int pageIndex, byte roleId, byte statusId, int totalPages, bool isDirectMember = false);

    Task<IEnumerable<TotalDownlinesSummary>> GetTotalDownlinesSummary(int custId, byte roleId);

    Task<DateTime> GetLastTxn(string username, int uplineId, bool isInternal);

    Task<IEnumerable<OnlineUser>> GetOnlineList(int custid, bool adminCanViewVNIP);

    Task<Result> UpdateGivenCredit(GivenCreditCommand param);

    Task<Result> UpdateCustomerStatus(UpdateCustomerStatusParam param);

    Task<bool> CheckMultipleCustomersBelonging(int actorId, int subAccountId, string custIds);

    Task<Result> UpdateAllDayBalanceTransfer(UpdateAllDayBalanceTransferParam param);
}


public class SearchAccountService(
    IDynamicRepository dynamicRepository,
    IPService ipService,
    HttpClient httpClient) : ISearchAccountService
{
    private readonly IDynamicRepository dynamicRepository = dynamicRepository;
    private readonly IPService ipService = ipService;
    private readonly HttpClient httpClient = httpClient;
    //private readonly IIdentityClient identityClient = identityClient;
    private const string English = "en-US";
    private const string ChineseTraditional = "zh-TW";
    private const string ChineseSimplified = "zh-CN";
    private const string Thai = "th-TH";
    private const string Vietnamese = "vi-VN";
    private readonly int failErrorCode = 100;

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

    private async Task<bool> UpdateUserSearch(int userId, string search, string firstName, string lastName, string nickname)
       => await httpClient.PutAsJsonAsync<bool>("customer/username-suggestion", new { userId, search, firstName, lastName, nickname });

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

        return Task.FromResult<IBalanceInfoModel>(null);
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

    public async Task<DownlineListModel> GetDownlineList(int custId, int pageIndex, byte roleId, byte statusId, int totalPages, bool isDirectMember = false)
    {
        var (downlineListModel, downlines) = await dynamicRepository.FetchMultipleAsync<DownlineListModel, DownlineDto>(
            new SelectDownlineListByStatusCriteria(custId, pageIndex, roleId, statusId, totalPages, isDirectMember));
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
        decimal accumCreditAllCashMember = await GetAccumCreditAllCashMember(param.SearchAccountRoleId, param.CustId);

        UpdateSMGivenCreditCommand command;
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
                accumCreditAllCashMember,
                totalAmtCommand.TotalAmt);
            await dynamicRepository.ExecuteAsync(command);
        }
        else if (param.SearchAccountRoleId == UserLevel.Agent.Value)
        {
            command = new UpdateAgentGivenCreditCommand(
                param.SearchAccountId,
                param.SearchAccountRoleId,
                param.CreditAmount,
                param.ViewerId,
                param.SubAccountId,
                param.LangKey,
                param.IP,
                accumCreditAllCashMember);
            await dynamicRepository.ExecuteAsync(command);
        }
        else
        {
            command = new UpdateSMGivenCreditCommand(
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

    private async Task<decimal> GetAccumCreditAllCashMember(byte searchAccountRoleId, int custId)
    {
        return searchAccountRoleId switch
        {
            var roleId when roleId == UserLevel.Member.Value => await GetMemberAccumCredit(custId),
            var roleId when roleId == UserLevel.Agent.Value => await GetAccumCredit(custId),
            _ => 0
        };
    }

    private async Task<decimal> GetMemberAccumCredit(int custId)
    {
        var getUplineByCustIdCommand = new GetUplineByCustIdCommand(custId);
        await dynamicRepository.ExecuteAsync(getUplineByCustIdCommand);
        
        return await GetAccumCredit(getUplineByCustIdCommand.UplineId);
    }

    private async Task<decimal> GetAccumCredit(int custId)
    {
        var accumCreditCommand = new GetAccumCreditCommand(custId);
        await dynamicRepository.ExecuteAsync(accumCreditCommand);
        
        return accumCreditCommand.AccumCreditReturn;
    }

    public async Task<Result> UpdateCustomerStatus(UpdateCustomerStatusParam param)
    {
        UpdateCustomerStatusCommand command = param.StatusName switch
        {
            StatusNames.Closed => new UpdateClosedStatusCommand(
                                    param.SearchAccountId,
                                    param.ActorId,
                                    param.SubAccountId,
                                    param.Language,
                                    param.IP,
                                    param.StatusName,
                                    param.StatusValue),
            StatusNames.DisableAutoMasterPT => new UpdateDisableAutoMPTStatusCommand(
                                    param.SearchAccountId,
                                    param.ActorId,
                                    param.SubAccountId,
                                    param.Language,
                                    param.IP,
                                    param.StatusName,
                                    param.StatusValue),
            StatusNames.DoubleComm => new UpdateDoubleCommLosingStatusCommand(
                                    param.SearchAccountId,
                                    param.ActorId,
                                    param.SubAccountId,
                                    param.Language,
                                    param.IP,
                                    param.StatusName,
                                    param.StatusValue),
            StatusNames.DisableOutright => new UpdateDisableOutrightStatusCommand(
                                    param.SearchAccountId,
                                    param.ActorId,
                                    param.SubAccountId,
                                    param.Language,
                                    param.IP,
                                    param.StatusName,
                                    param.StatusValue),
            StatusNames.Suspended => new UpdateSuspendedStatusCommand(
                                    param.SearchAccountId,
                                    param.ActorId,
                                    param.SubAccountId,
                                    param.Language,
                                    param.IP,
                                    param.StatusName,
                                    param.StatusValue),
            StatusNames.Unlock => new UpdateUnlockedCommand(
                                    param.SearchAccountId,
                                    param.ActorId,
                                    param.SubAccountId,
                                    param.Language,
                                    param.IP,
                                    param.StatusName),
            _ => new UpdateDisableProtectedStatusCommand(
                                    param.SearchAccountId,
                                    param.ActorId,
                                    param.SubAccountId,
                                    param.Language,
                                    param.IP,
                                    param.StatusName,
                                    param.StatusValue),
        };
        await dynamicRepository.ExecuteAsync(command);
        return new Result(command.ErrCode, command.ErrMsg);
    }

    public async Task<bool> CheckMultipleCustomersBelonging(int actorId, int subAccountId, string custIds)
    {
        var criteria = new CheckMultipleCustomersBelongingCriteria(actorId, subAccountId, custIds);
        await dynamicRepository.ExecuteAsync(criteria);

        return criteria.RoleId > 0 && criteria.IsActive;

        // var belongingInfo = await identityClient.CheckBelongingMultiple(actorId, subAccountId, custIds);

        // return belongingInfo.IsActive;
    }

    public async Task<Result> UpdateAllDayBalanceTransfer(UpdateAllDayBalanceTransferParam param)
    {
        if (!await IsAllDayBalanceTransfer(param.ActorId))
        {
            const int DisallowedAllDayBalanceTransferError = 6001;
            return Result.CreateErrorResult(Resources.DisallowedAllDayBalanceTransferError, DisallowedAllDayBalanceTransferError);
        }

        if (IsAutoTransferAffect(param.Currency))
        {
            var autoTransferStatus = await GetAutoTransferJobStatus(param.AGIP);
            if (autoTransferStatus)
            {
                return Result.CreateErrorResult(AlphaPlus.Shared.AppResources.Settings.Resources.ADBTChangeFailed_AutoJobRunning, 1);
            }
        }

        var command = new UpdateAllDayBalanceTransferCommand(
            param.SearchAccountId,
            param.ActorId,
            param.Language,
            param.IP,
            param.StatusValue);
        await dynamicRepository.ExecuteAsync(command);
        var result = new Result(int.Parse(command.ErrCode), command.ErrMsg);

        if (result.IsSuccessful)
        {
            TurnOffAutoTransfer(param);
        }

        return result;
    }

    private void TurnOffAutoTransfer(UpdateAllDayBalanceTransferParam param)
    {
        if (!param.StatusValue)
        {
            return;
        }

        var turnOffCriteria = new TurnOffAutoTransferCriteria
        {
            UplineId = param.SearchAccountId,
            Creator = param.ActorId,
            IsAllDownline = true,
            IP = param.IP
        };

        var affects = dynamicRepository.Fetch<AffectedDownlinesInfoModel>(turnOffCriteria);

        if (!affects.Any())
        {
            return;
        }

        var rm = AlphaPlus.Shared.AppResources.Common.CommonResx.ResourceManager;
        foreach (var row in affects)
        {
            var uplineId = row.CustId;

            var toastMessageCriteria = new InsertToastMessageCriteria(
                uplineId,
                param.ActorId,
                GetToastMessageContent(rm, English),
                GetToastMessageContent(rm, ChineseSimplified),
                GetToastMessageContent(rm, ChineseTraditional),
                GetToastMessageContent(rm, Thai),
                GetToastMessageContent(rm, Vietnamese));
            dynamicRepository.Execute(toastMessageCriteria);

            var uplineName = row.Username;
            var downlines = row.Names;
            var personalMessageCriteria = new InsertPersonalMessageCriteria(
                GetPersonalMessage(rm, English, downlines),
                GetPersonalMessage(rm, ChineseTraditional, downlines),
                GetPersonalMessage(rm, ChineseSimplified, downlines),
                GetPersonalMessage(rm, Thai, downlines),
                GetPersonalMessage(rm, Vietnamese, downlines));

            dynamicRepository.Execute(personalMessageCriteria);

            if (personalMessageCriteria.MessageIDOutput != 0)
            {
                dynamicRepository.Execute(new AssignPersonalMessageCriteria(personalMessageCriteria.MessageIDOutput, uplineId, uplineName));
            }
        }
    }

    private static string GetToastMessageContent(ResourceManager rm, string languae)
    {
        var culture = CultureInfo.GetCultureInfo(languae);

        var content = new ToastContentModel
        {
            Title = rm.GetString("ADBTChangeToastTitle", culture),
            Body = rm.GetString("ADBTChangeToastMessage", culture)
        };

        return JsonConvert.SerializeObject(content, Formatting.None);
    }

    private static string GetPersonalMessage(ResourceManager rm, string languae, string downlines)
    {
        var culture = CultureInfo.GetCultureInfo(languae);

        return string.Format(
            rm.GetString("ADBTConditionChange_PersonalMessage", culture),
            downlines);
    }

    private async Task<bool> GetAutoTransferJobStatus(string agIP)
    {
        var criteria = new GetAutoTransferJobStatusCriteria();
        await dynamicRepository.ExecuteAsync(criteria, agIP);

        return criteria.IsAutoJobRunning;
    }

    private static bool IsAutoTransferAffect(string currency)
    {
        var s = Core.Configurations.ConfigurationManager.GetValue<string>("OfferAutoTransfer.Currencies");
        if (string.IsNullOrWhiteSpace(s))
        {
            return false;
        }

        if (s == "*")
        {
            return true;
        }

        return s.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Any(c => string.Equals(currency, c.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
