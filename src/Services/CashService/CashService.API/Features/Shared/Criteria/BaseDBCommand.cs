namespace AlphaPlus.CashService.API.Features.Shared.Criteria;

using Fanex.Data.Repository;

public class BaseDBCommand : CriteriaBase
{
    public BaseDBCommand(int custId, int subAccId, string ip, string language)
    {
        CustId = custId;
        SubAccId = subAccId;
        IP = ip;
        Language = language;
    }

    public int CustId { get; }
    public int SubAccId { get; }
    public string IP { get; } 
    public string Language { get; } 

    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public override string GetSettingKey() => string.Empty;

    public override bool IsValid()
        => CustId > 0 &&
            SubAccId >= 0 &&
            !string.IsNullOrEmpty(IP) &&
            !string.IsNullOrEmpty(Language);
}