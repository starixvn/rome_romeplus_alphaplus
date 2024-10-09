namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Models;

public class AccountInfoViewModelBase
{
    public string AccountInfoUrl => "/account-info/";

    protected string OldMainUrl => "/settings/";

    protected string BetListsUrl => "/tickets/";

    protected string ViewLogUrl => "/settings/";

    protected string ReportsUrl => "/reports/";
    protected string MemberInfoUrl => "/settings/";

    private static string ReportDomainUrl => "/reports/";

    private static string BetlistDomainUrl => "/tickets/";
}