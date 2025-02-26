using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.ViewModels;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount
{
    public class GetAccountQuery : BaseQuery, IRequest<AccountViewModel>
    {
        public string Username { get; set; }

        public override bool IsValid()
            => base.IsValid()
            && !string.IsNullOrWhiteSpace(Username)
            && ViewMemberInfo
            && !Username.Trim().Equals(ViewerName, StringComparison.OrdinalIgnoreCase);
    }
}