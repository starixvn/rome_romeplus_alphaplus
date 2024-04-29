using AlphaPlus.Settings.Site.Features.Shared.Requests;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Requests
{
    public class GetAccountQuery : BaseQuery
    {
        public string Username { get; set; }

        public override bool IsValid()
            => base.IsValid()
            && !string.IsNullOrWhiteSpace(Username)
            && userSession.SubPageAccess.ViewMemberInfo
            && !Username.Trim().Equals(ActorName, StringComparison.OrdinalIgnoreCase);
    }
}