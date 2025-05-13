namespace AlphaPlus.CashService.API.Criteria;

using Fanex.Data.Repository;

public class BaseActionCommand : NonQueryCommand
{
    protected BaseActionCommand(int custId, int subAccId, string ip, string language)
    {
        CustId = custId;
        SubAccId = subAccId;
        IP = ip;
        Language = language;
    }

    public int CustId { get; set; }
    public int SubAccId { get; set; }
    public string IP { get; set; } 
    public string Language { get; set; } 

    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public override string GetSettingKey() => string.Empty;

    public override bool IsValid()
        => CustId > 0 &&
            SubAccId >= 0 &&
            !string.IsNullOrEmpty(IP) &&
            !string.IsNullOrEmpty(Language);
}