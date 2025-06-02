namespace AlphaPlus.CashService.API.Features.Transactions.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;
using Models;

public class InsertTransactionLogTransferCommand(TransactionLog logItem, int siteDbGroupId) : NonQueryCommand
{
    public int UserId { get; set; } = logItem.UserId;
    public int FromCustId { get; set; } = logItem.FromCustId;
    public int ToCustId { get; set; } = logItem.ToCustId;
    public string IP { get; set; } = logItem.IP;
    public string Doing { get; set; } = logItem.Doing;

    [SpParam(Ignored = true)] public int SiteDbGroupId { get; set; } = siteDbGroupId;

    public override string GetSettingKey() => $"{(SiteDbGroupId >= 0 ? $"SiteDb_{SiteDbGroupId}_" : string.Empty)}WriteLog_CashService_UserLog_Transfer";

    public override bool IsValid() => UserId > 0 && FromCustId > 0 && ToCustId > 0;
}