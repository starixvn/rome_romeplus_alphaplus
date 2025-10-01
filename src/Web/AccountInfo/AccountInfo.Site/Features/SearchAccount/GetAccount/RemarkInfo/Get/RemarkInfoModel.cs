namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.RemarkInfo.Get;

public class RemarkInfoViewModel
{
    public RemarkInfoViewModel(int searchAccountId, string searchAccountName, string currentRemark, bool isShowCancelButton)
    {
        SearchAccountId = searchAccountId;
        SearchAccountName = searchAccountName;
        CurrentRemark = currentRemark;
        IsShowCancelButton = isShowCancelButton;
    }
    public int SearchAccountId { get; set; }
    public string SearchAccountName { get; set; }
    public string CurrentRemark { get; set; }
    public bool IsShowCancelButton { get; set; }
}