using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.LastBets;

internal class DeleteFilterCommand(
    int custId,
    int subAccId,
    int filterId,
    string languageKey,
    string ip) : NonQueryCommand
{
    public int CustId { get; } = custId;

    public int SubAccId { get; } = subAccId;

    public int FilterId { get; } = filterId;

    public string Language { get; } = languageKey;

    public string IP { get; } = ip;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "Age_CustLastBetFilter_Del";

    public override bool IsValid()
        => CustId > 0 && SubAccId >= 0 && FilterId > 0;
}