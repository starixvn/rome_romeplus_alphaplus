using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class CommissionQuery : MemberInfoBaseQuery, IRequest<CommissionViewModel>
{
    public int Id { get; set; } = ProductIdConstants.Sportsbook;

    public bool IsShowDoubleComm
        => Id == ProductIdConstants.Sportsbook && viewer.IsDoubleCommLosing;

    public string DarkModeName => viewer.DarkModeName;
}
