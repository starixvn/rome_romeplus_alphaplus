namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.Update.Balance;

public class UpdateAllDayBalanceTransferParam
{
    public UpdateAllDayBalanceTransferParam(
        int searchAccountId,
        int actorId,
        string language,
        string ip,
        bool statusValue,
        string currency,
        string agIP)
    {
        SearchAccountId = searchAccountId;
        ActorId = actorId;
        Language = language;
        IP = ip;
        StatusValue = statusValue;
        Currency = currency;
        AGIP = agIP;
    }

    public int SearchAccountId { get; }

    public int ActorId { get; }

    public string Language { get; }

    public string IP { get; }

    public bool StatusValue { get; }

    public string Currency { get; }

    public string AGIP { get; }
}