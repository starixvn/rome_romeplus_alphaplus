using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Outstanding;

public class OutstandingQuery : BaseQuery, IRequest<OutstandingViewModel>
{
    private const string AutoFollowBetIsDummyAgentSessionKey = "AutoFollowBetIsDummyAgent";

    public string UserSelectedProductIds { get; set; }

    public string SortPropertyName { get; set; }

    public bool IsDescendingSort { get; set; }

    public bool IsFilterVisible { get; set; } = true;

    public bool AutoFollowBetIsDummyAgent => Viewer.GetProperty<bool>(AutoFollowBetIsDummyAgentSessionKey);
}