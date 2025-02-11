using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.ViewModels;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount;

public class CustomerInfoQuery : BaseQuery, IRequest<CustomerInfoViewModel>
{
    public string SearchAccountName { get; set; }

    public int ActorId => CustId;

    public string ActorName => ViewerName;

    public byte ActorRoleId => ViewerLevelId;

    public override bool IsValid() =>
        !string.IsNullOrWhiteSpace(SearchAccountName) &&
        ViewMemberInfo &&
        !SearchAccountName.Trim().Equals(ActorName, StringComparison.OrdinalIgnoreCase);
}