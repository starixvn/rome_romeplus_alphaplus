using MediatR;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;

public class WinLossLimitQuery : MemberInfoBaseQuery, IRequest<WinLossLimitViewModel>
{
    public int BetType { get; set; }

    public int SportType { get; set; }

    public int MasterId { get; set; } = -1;

    public int RecommendId { get; set; } = -1;

    public string Username { get; set; }

    public string PlayerId { get; set; }

    public int UserSelectedProductId { get; set; }

    public int PageSize { get; set; } = 100;

    public int PageIndex { get; set; } = 1;
}