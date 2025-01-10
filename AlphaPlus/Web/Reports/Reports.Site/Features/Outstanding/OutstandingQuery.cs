using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Features.Outstanding;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Outstanding;

public class OutstandingQuery : BaseQuery, IRequest<OutstandingViewModel>
{
    private const string AutoFollowBetIsDummyAgentSessionKey = "AutoFollowBetIsDummyAgent";

    public OutstandingQuery()
    {
        MinDate = SystemTime.Today;
        AutoFollowBetIsDummyAgent = Viewer.GetProperty<bool>(AutoFollowBetIsDummyAgentSessionKey);
    }

    public string UserSelectedProductIds { get; set; } = "*";

    public byte LevelType { get; set; }

    public string SortPropertyName { get; set; } = nameof(OutstandingModel.CustName);

    public byte DescendingSort { get; set; }

    public bool AutoFollowBetIsDummyAgent { get; }
}