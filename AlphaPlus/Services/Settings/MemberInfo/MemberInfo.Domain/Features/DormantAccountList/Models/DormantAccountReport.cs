namespace AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class DormantAccountReport
    {
        public DormantAccountReport(
            IEnumerable<DormantAccount> dormantAccounts,
            int totalItems)
        {
            DormantAccounts = dormantAccounts.Where(i => !i.IsSummaryRow);
            TotalAccount = dormantAccounts.FirstOrDefault(i => i.IsSummaryRow);
            TotalItems = totalItems;
        }

        public IEnumerable<DormantAccount> DormantAccounts { get; }

        public DormantAccount TotalAccount { get; }

        public int TotalItems { get; }

        public bool HasData => TotalItems > 0;
    }
}
