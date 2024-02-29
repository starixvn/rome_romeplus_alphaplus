namespace AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit
{
    using System.Collections.Generic;

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
