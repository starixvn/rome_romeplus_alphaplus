using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Settings.Transfer.API.Criteria;
using AlphaPlus.Settings.Transfer.API.Models;
using Fanex.Data.Repository;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Criteria.TransferCondition;
using AlphaPlus.Settings.Transfer.API.Params.TransferCondition;
using AlphaPlus.Settings.Transfer.API.Criteria.AutoTransfer;
using AlphaPlus.Settings.Transfer.API.Models.TransferCondition;
using System.Globalization;
using System.Resources;
using Newtonsoft.Json;

namespace AlphaPlus.Settings.Transfer.API.Services;

public interface ITransferService
{
    bool IsAllDayBalanceTransfer(int custId);

    Task<bool> IsAllDayBalanceTransferAsync(int custId);

    Task<string> GetAllowTransfer(int custId);

    Task<Tuple<bool, IEnumerable<string>>> GetTransferCondition(int custId);

    Task<string> GetTransferOption(int custId);

    Task<Result> UpdateTransferOption(TransferConditionParam param);

    Task<int> GetSuperIdAutomateTransfer(int custId);
}

public class TransferService(IDynamicRepository repository) : ITransferService
{
    private const int NumberDaysInWeek = 7;
    private static readonly string[] DaysInWeek =
    [
        Resources.Sunday,
        Resources.Monday,
        Resources.Tuesday,
        Resources.Wednesday,
        Resources.Thursday,
        Resources.Friday,
        Resources.Saturday
    ];
    private readonly IDynamicRepository repository = repository;

    public bool IsAllDayBalanceTransfer(int custId)
    {
        var transferModel = repository.Get<TransferModel>(new GetIsAllDayBalTransferCriteria(custId));

        return transferModel?.IsAllDayBalTransfer == true;
    }

    public async Task<bool> IsAllDayBalanceTransferAsync(int custId)
    {
        var transferModel = await repository.GetAsync<TransferModel>(new GetIsAllDayBalTransferCriteria(custId));

        return transferModel?.IsAllDayBalTransfer == true;
    }

    public async Task<string> GetAllowTransfer(int custId)
    {
        var (isDaily, days) = await GetTransferCondition(custId);
        var transferCondition = isDaily ? Resources.Daily : string.Join(" - ", days);

        return Resources.AllowTransfer + (string.IsNullOrEmpty(transferCondition) ? string.Empty : " " + transferCondition);
    }

    public async Task<Tuple<bool, IEnumerable<string>>> GetTransferCondition(int custId)
    {
        var transferOption = await GetTransferOption(custId);
        var charArray = transferOption.ToCharArray();

        if (charArray.Length == 0)
        {
            return Tuple.Create(false, Enumerable.Empty<string>());
        }

        if (charArray[NumberDaysInWeek] != '1')
        {
            return Tuple.Create(true, Enumerable.Empty<string>());
        }

        var dayBuilder = new List<string>();
        for (int i = 1, max = charArray.Length; i < max; i++)
        {
            var id = i % NumberDaysInWeek;

            if (charArray[id] == '1')
            {
                dayBuilder.Add(DaysInWeek[id]);
            }
        }

        return Tuple.Create<bool, IEnumerable<string>>(false, dayBuilder);
    }

    public async Task<string> GetTransferOption(int custId)
    {
        var criteria = new GetTransferOptionCriteria(custId);
        await repository.ExecuteAsync(criteria);

        return criteria.TransferOption;
    }

    public async Task<Result> UpdateTransferOption(TransferConditionParam param)
    {
        var superId = await GetSuperIdAutomateTransfer(param.CustId);

        if (superId > 0)
        {
            return Result.CreateErrorResult(param.ActorLevelId == UserLevel.Super.Value
                ? Resources.TransferConditionByCompany
                : Resources.TransferConditionByUpline);
        }

        var autoTransferJobStatusCriteria = new GetJobStatusCriteria(null, null, true);
        await repository.ExecuteAsync(autoTransferJobStatusCriteria, param.AGIP);

        if (autoTransferJobStatusCriteria.IsAutoJobRunning)
        {
            return Result.CreateErrorResult(Resources.ConditionChangeFailed_AutoJobRunning);
        }

        var updateTransferOptionCriteria = new UpdateTransferOptionCriteria(
            param.CustId,
            param.TransferOption,
            param.TurnOffAutoTransfer,
            param.SubAccId,
            param.ActorId,
            param.IP,
            param.Language,
            true);
        var personalMessages = await repository.FetchAsync<TransferConditionPersonalMessage>(updateTransferOptionCriteria);
        await InsertPersonalMessages(param.ActorId, personalMessages);

        return new Result(
            updateTransferOptionCriteria.ErrCode,
            updateTransferOptionCriteria.ErrCode == 0 ? Resources.UpdatedSuccessfully : updateTransferOptionCriteria.ErrMsg);
    }

    private async Task InsertPersonalMessages(int actorId, IEnumerable<TransferConditionPersonalMessage> personalMessages)
    {
        if (!personalMessages.Any())
        {
            return;
        }

        var rm = Resources.ResourceManager;
        foreach (var item in personalMessages)
        {
            var uplineId = item.CustId;
            var toastMessageCriteria = new InsertToastMessageCriteria(
                3,
                3,
                uplineId,
                actorId,
                BuildToastContent(rm, "en-US"),
                BuildToastContent(rm, "zh-TW"),
                BuildToastContent(rm, "zh-CN"),
                BuildToastContent(rm, "th-TH"));
            await repository.ExecuteAsync(toastMessageCriteria);

            var uplineName = item.Username;
            var dowlines = item.Names;
            var personalMessageCriteria = new InsertPersonalMessageCriteria(
                string.Format(rm.GetString("TransferConditionChange_PersonalMessage", new CultureInfo("en-US")), dowlines),
                string.Format(rm.GetString("TransferConditionChange_PersonalMessage", new CultureInfo("zh-TW")), dowlines),
                string.Format(rm.GetString("TransferConditionChange_PersonalMessage", new CultureInfo("zh-CN")), dowlines),
                string.Format(rm.GetString("TransferConditionChange_PersonalMessage", new CultureInfo("th-TH")), dowlines));

            var messageId = await repository.ExecuteAsync(personalMessageCriteria);
            if (personalMessageCriteria.MessageIdOutput > 0)
            {
                await repository.ExecuteAsync(new AssignPersonalMessageCriteria(messageId, uplineId, uplineName));
            }
        }
    }

    private static string BuildToastContent(ResourceManager rm, string language)
    {
        var cultureInfo = new CultureInfo(language);
        var toastContent = new ToastContentDto(
            rm.GetString("TransferConditionChangeToastTitle", cultureInfo),
            rm.GetString("TransferConditionChangeToastBody", cultureInfo));

        return JsonConvert.SerializeObject(toastContent, Formatting.None);
    }

    public Task<int> GetSuperIdAutomateTransfer(int custId)
        => repository.GetAsync<int>(new GetSuperAutomateTransferCriteria(custId));
}
