using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Requests
{
    public class GetAccountQuery : BaseQuery
    {
        public string Username { get; set; }

        public override bool IsValid()
            => base.IsValid()
            && !string.IsNullOrWhiteSpace(Username)
            && viewer.SubPageAccess.ViewMemberInfo
            && !Username.Trim().Equals(ViewerUsername, StringComparison.OrdinalIgnoreCase);
    }
}