using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class CommissionQuery : MemberInfoBaseQuery, IRequest<CommissionViewModel>
{
    public int ProductId { get; set; } = ProductIdConstants.Sportsbook;

    public bool IsShowDoubleComm
        => ProductId == ProductIdConstants.Sportsbook && viewer.IsDoubleCommLosing;

    public string DarkModeName => viewer.DarkModeName;
}
