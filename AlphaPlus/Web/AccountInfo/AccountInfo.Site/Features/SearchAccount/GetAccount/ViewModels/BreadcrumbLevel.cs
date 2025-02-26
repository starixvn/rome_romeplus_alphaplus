namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.ViewModels
{
    public class BreadcrumbLevel(
        int custId,
        string custName,
        byte custLevelId)
    {
        public int CustId { get; } = custId;

        public string CustName { get; } = custName;

        public byte CustLevelId { get; } = custLevelId;
    }
}