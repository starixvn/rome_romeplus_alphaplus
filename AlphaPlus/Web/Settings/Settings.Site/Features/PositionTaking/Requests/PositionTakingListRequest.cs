using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.PositionTaking.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.PositionTaking.Requests;

public class PositionTakingListRequest : IRequest<IEnumerable<PositionTakingViewModel>>
{
    public int CustId { get; set; } = UserSession.Current.CustId;

    public bool IsValid() => CustId > 0;
}