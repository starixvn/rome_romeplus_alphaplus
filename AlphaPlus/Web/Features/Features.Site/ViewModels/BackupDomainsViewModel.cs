using AlphaPlus.Features.Messages.API.Models.Constants;

namespace AlphaPlus.Features.Site.Messages.ViewModels
{
    public class BackupDomainsViewModel
    {
        public BackupDomainsViewModel(
            byte roleId,
            IEnumerable<string> agencyDomains,
            IEnumerable<string> memberDomains)
        {
            RoleName = UserLevel.GetDisplayName(roleId);
            AgencyDomains = agencyDomains;
            MemberDomains = memberDomains;
        }

        public string RoleName { get; }

        public IEnumerable<string> AgencyDomains { get; }

        public IEnumerable<string> MemberDomains { get; }
    }
}