namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.RemarkInfo.Get;

public class RemarkInfoViewModel
{
    public RemarkInfoViewModel(int searchAccountId, string searchAccountName, string currentRemark)
    {
        SearchAccountId = searchAccountId;
        SearchAccountName = searchAccountName;
        CurrentRemark = currentRemark;
    }
    public int SearchAccountId { get; set; }
    public string SearchAccountName { get; set; }
    public string CurrentRemark { get; set; }
}