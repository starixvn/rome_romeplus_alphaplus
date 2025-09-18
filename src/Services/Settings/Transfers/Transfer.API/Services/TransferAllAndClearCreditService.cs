namespace AlphaPlus.Settings.Transfer.API.Services;

using System.Text;
using Criteria.TransferAllAndClearCredit;
using Fanex.Data.Repository;
using Models.TransferAllAndClearCredit;
using Params.TransferAllAndClearCredit;

public interface TransferAllAndClearCreditService
{
    TransferResult GetCalculateResult(GetCalculationResultParam param);

    TransferResult GetTransferResult(GetTransferResultParam param);
}

public class TransferAllAndClearCreditServiceImpl(IDynamicRepository dynamicRepository)
    : TransferAllAndClearCreditService
{
    public TransferResult GetCalculateResult(GetCalculationResultParam param)
    {
        var getDownlineCriteria = new GetDownlineTransferCriteria(param.CustId, param.Username);
        dynamicRepository.Execute(getDownlineCriteria);

        var getNetPositionCriteria = new GetNetPositionCriteria(getDownlineCriteria.DownlineCustId, getDownlineCriteria.DownlineRoleId, param.SiteDbGroupId);
        var netPositionModels = dynamicRepository.Fetch<NetPositionModel>(getNetPositionCriteria);

        var criteria = new GetCalculateResultCriteria(
            param.CustId,
            param.Username,
            param.Language,
            param.IsAllDayBalanceTransfer,
            BuildDownlineNetPositionXml(netPositionModels),
            getDownlineCriteria.DownlineCustId,
            param.SiteDbGroupId);
        var (conditionSteps, transferSteps, clearCreditSteps)
            = dynamicRepository.FetchMultiple<ConditionStep, TransferStep, ClearCreditStep>(criteria);

        return new TransferResult(transferSteps, clearCreditSteps, conditionSteps.Select(c => c.Content));
    }

    private static string BuildDownlineNetPositionXml(IEnumerable<NetPositionModel> netPositions)
    {
        var xmlData = new StringBuilder("<ROOT>");
        foreach (var item in netPositions)
        {
            xmlData.Append(item.ToXML);
        }
        xmlData.Append("</ROOT>");

        return xmlData.ToString();
    }

    public TransferResult GetTransferResult(GetTransferResultParam param)
    {
        var skipTransfer = param.SkipTransfer();
        TransferAllResult transferResult;

        if (skipTransfer)
        {
            transferResult = new TransferAllResult();
        }
        else
        {
            var criteria = new TransferAllCriteria(
                param.CustId,
                param.UserId,
                param.SubAccId,
                param.Username,
                param.IP,
                param.Language,
                param.IsAllDayBalanceTransfer,
                param.SiteDbGroupId);
            var (conditionSteps, transferSteps) = dynamicRepository.FetchMultiple<ConditionStep, TransferStep>(criteria);

            transferResult = new TransferAllResult(transferSteps, criteria.ErrCode, criteria.ErrMsg ?? string.Empty, conditionSteps.Select(c => c.Content));
        }

        var clearCreditResult = (skipTransfer || transferResult.IsSuccess) && param.IsClearCredit
            ? ClearCredit(param)
            : new ClearCreditResult([], 1, string.Empty, []);

        return new TransferResult(transferResult, clearCreditResult, skipTransfer);
    }

    private ClearCreditResult ClearCredit(GetTransferResultParam param)
    {
        var command = new GetCustomerInfoCriteria(param.Username);
        dynamicRepository.Execute(command);

        var totalAmts = dynamicRepository.Fetch<TotalAmtModel>(new GetTotalAmtCriteria(command.CustId, command.RoleId, param.SiteDbGroupId));
        var criteria = new ClearCreditCriteria(
            param.CustId,
            param.UserId,
            param.SubAccId,
            param.Username,
            param.IP,
            param.Language,
            BuildMemberAmountXml(totalAmts));
        var (conditionSteps, clearCreditSteps) = dynamicRepository.FetchMultiple<ConditionStep, ClearCreditStep>(criteria);

        return new ClearCreditResult(clearCreditSteps, criteria.ErrCode, criteria.ErrMsg ?? string.Empty, conditionSteps.Select(c => c.Content));
    }

    private static string BuildMemberAmountXml(IEnumerable<TotalAmtModel> totalAmts)
    {
        var xmlData = new StringBuilder("<ROOT>");
        foreach (var amt in totalAmts)
        {
            xmlData.Append(amt.ToXML);
        }
        xmlData.Append("</ROOT>");

        return xmlData.ToString();
    }
}