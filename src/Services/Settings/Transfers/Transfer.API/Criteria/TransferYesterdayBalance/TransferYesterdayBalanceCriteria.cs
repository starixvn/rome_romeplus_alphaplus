namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

using Core.Criteria;

public abstract class TransferYesterdayBalanceCriteria(
    string language,
    string ip,
    int creator,
    int subAccId) : SiteDbBaseCommand
{
    public string Language { get; } = language;

    public string IP { get; } = ip;

    public int Creator { get; } = creator;

    public int SubAccId { get; } = subAccId;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }
}