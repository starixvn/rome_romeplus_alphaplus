using AlphaPlus.Reports.Features.Outstanding;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Outstanding;

public class OutstandingQuery : BaseQuery, IRequest<OutstandingViewModel>
{
    public OutstandingQuery() : base() => MinDate = DateTime.Today;

    private const string AutoFollowBetIsDummyAgentSessionKey = "AutoFollowBetIsDummyAgent";

    public string UserSelectedProductIds { get; set; } = "*";

    public string SortPropertyName { get; set; } = nameof(OutstandingModel.CustName);

    public byte DescendingSort { get; set; }

    public bool AutoFollowBetIsDummyAgent => Viewer.GetProperty<bool>(AutoFollowBetIsDummyAgentSessionKey);
}