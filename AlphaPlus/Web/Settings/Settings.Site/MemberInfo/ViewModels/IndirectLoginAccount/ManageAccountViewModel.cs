namespace AlphaPlus.Settings.Site.ViewModels.IndirectLoginAccount
{
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Core.Helpers;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.IndirectLoginAccount;

    public class ManageAccountViewModel
    {
        public ManageAccountViewModel()
            : this(Enumerable.Empty<IndirectLoginAccountItem>())
        {
        }

        public ManageAccountViewModel(IEnumerable<IndirectLoginAccountItem> accounts)
        {
            Accounts = accounts;
        }

        public IEnumerable<IndirectLoginAccountItem> Accounts { get; }

        public string RootURL => MicroSites.Settings("~/IndirectLoginAccount/");
    }
}