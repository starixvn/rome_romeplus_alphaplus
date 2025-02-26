using AlphaPlus.Reports.Features.Customer;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Customer;

public class UsernameSuggestionQuery : BaseQuery, IRequest<IEnumerable<UsernameSuggestion>>
{
    public UsernameSuggestionQuery()
        => IsInternal = Viewer.IsInternal;

    public string Query { get; set; }

    public bool DirectDownline { get; set; }

    public bool IsAccInfo { get; set; } = true;

    public bool IsInternal { get; }

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(CustName) && base.IsValid();
}