using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.LastBets;

internal class AddFilterCommand(
    int custId,
    int subAccId,
    string filterName,
    bool isDefault,
    bool isNotify,
    string betType,
    int stake,
    string sportTypeListXml,
    byte liveStatus,
    byte filterByMembers,
    string inputList,
    string myWatchlist,
    string languageKey,
    string ip) : NonQueryCommand
{
    public int CustId { get; } = custId;

    public int SubAccId { get; } = subAccId;

    public string FilterName { get; } = filterName;

    public bool IsDefault { get; } = isDefault;

    public bool IsNotify { get; } = isNotify;

    public string BetType { get; } = betType;

    public int Stake { get; } = stake;

    public string SportTypeList { get; } = sportTypeListXml;

    public byte IsLive { get; } = liveStatus;

    public byte IsWatchList { get; } = filterByMembers;

    public string InputList { get; } = inputList;

    public string WatchList { get; } = myWatchlist;

    public string Language { get; } = languageKey;

    public string IP { get; } = ip;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "Age_CustLastBetFilter_Ins";

    public override bool IsValid()
        => CustId > 0
        && SubAccId >= 0
        && !string.IsNullOrWhiteSpace(FilterName)
        && !string.IsNullOrWhiteSpace(BetType)
        && Stake >= 0
        && !string.IsNullOrWhiteSpace(SportTypeList);
}
