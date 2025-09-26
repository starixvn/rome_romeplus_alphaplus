namespace AlphaPlus.Settings.Transfer.API.Params.TransferAllAndClearCredit;

public class GetCalculationResultParam(
    int custId,
    string username,
    string language,
    bool isAllDayBalanceTransfer,
    int siteDbGroupId)
{
    public int CustId { get; set; } = custId;

    public string Username { get; set; } = username;

    public string Language { get; set; } = language;

    public bool IsAllDayBalanceTransfer { get; set; } = isAllDayBalanceTransfer;

    public int SiteDbGroupId { get; set; } = siteDbGroupId;
}