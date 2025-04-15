using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class CommDataQuery : MemberInfoBaseQuery, IRequest<CommDataViewModel>
{
    public int ProductId { get; set; } = ProductIdConstants.Sportsbook;

    public int MasterId { get; set; } = -1;

    public int RecommendId { get; set; } = -1;

    public bool IsShowDoubleComm
        => ProductId == ProductIdConstants.Sportsbook && viewer.IsDoubleCommLosing;

    public override bool IsValid()
        => ProductId > 0 && ActorLevel.IsMasterOrSuperLevel() && base.IsValid();
}
