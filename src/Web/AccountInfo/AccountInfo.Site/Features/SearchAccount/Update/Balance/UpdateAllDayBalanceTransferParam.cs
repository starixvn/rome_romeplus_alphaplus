namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.Update.Balance;

public class UpdateAllDayBalanceTransferParam(
    int searchAccountId,
    int actorId,
    string language,
    string ip,
    bool statusValue,
    string currency,
    int siteDbGroupId)
{
    public int SearchAccountId { get; } = searchAccountId;

    public int ActorId { get; } = actorId;

    public string Language { get; } = language;

    public string IP { get; } = ip;

    public bool StatusValue { get; } = statusValue;

    public string Currency { get; } = currency;

    public string AGIP { get; }
    public int SiteDbGroupId { get; } = siteDbGroupId;
}