
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

public abstract class TransferYesterdayBalanceCriteria(
    string superIdsTransferTotal,
    string language,
    string ip,
    int creator,
    int subAccId) : NonQueryCommand
{
    public string SuperIdsTransferTotal { get; } = superIdsTransferTotal;

    public string Language { get; } = language;

    public string IP { get; } = ip;

    public int Creator { get; } = creator;

    public int SubAccId { get; } = subAccId;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }
}
