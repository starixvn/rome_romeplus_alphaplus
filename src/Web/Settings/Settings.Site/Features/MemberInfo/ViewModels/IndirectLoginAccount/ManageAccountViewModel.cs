using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.MemberInfo.Domain.Models.IndirectLoginAccount;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.IndirectLoginAccount
{
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

        public string RootURL => MicroSites.SettingUrl("~/IndirectLoginAccount/");
    }
}