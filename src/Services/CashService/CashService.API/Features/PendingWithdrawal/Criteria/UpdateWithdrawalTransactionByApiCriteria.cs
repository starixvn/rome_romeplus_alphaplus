namespace AlphaPlus.CashService.API.Features.PendingWithdrawal.Criteria;

using Fanex.Data.Repository;

public class UpdateWithdrawalTransactionByApiCriteria : NonQueryCommand
{
    public int CustId { get; set; }
    public int AgentId { get; set; }
    public long TransId { get; set; }
    public int StatusId { get; set; }
    public string IP { get; set; }
    public int SiteDbGroupId { get; set; }
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public override string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_API_CashWithdrawal_UpdateTransStatus";

    public override bool IsValid() => AgentId > 0 && CustId > 0 && TransId > 0;
}