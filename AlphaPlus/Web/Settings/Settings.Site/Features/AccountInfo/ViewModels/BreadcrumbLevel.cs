namespace AlphaPlus.Settings.Site.Features.AccountInfo.ViewModels
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