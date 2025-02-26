namespace AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit
{
    public class WinLossLimitGroupProductModel
    {
        public WinLossLimitGroupProductModel(string groupName, IEnumerable<WinLossLimitProductModel> products)
        {
            GroupName = groupName;
            Products = products;
        }

        public string GroupName { get; }

        public IEnumerable<WinLossLimitProductModel> Products { get; }

        public bool HasGroupName => !string.IsNullOrWhiteSpace(GroupName);
    }
}
