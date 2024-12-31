using MediatR;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public class SettlementQuery : SettlementGetDataQuery, IRequest<SettlementViewModel>
{
}