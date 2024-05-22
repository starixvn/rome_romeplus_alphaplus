using AlphaPlus.Settings.Site.Features.Shared.Requests;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Requests
{
    public class GivenCreditQuery : BaseQuery
    {
        public string SearchAccountName { get; set; }

        public byte SearchAccountRoleId { get; set; }
    }
}