using MediatR;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;

public class SportsbookPotentialWinLimitQuery : MemberInfoBaseQuery, IRequest<SportsbookPotentialWinLimitViewModel>
{
    public int BetType { get; set; }

    public int SportType { get; set; }

    public string Username { get; set; }

    public string Status { get; set; }

    public byte ProblemAccount { get; set; }

    public int PageSize { get; set; } = 100;

    public int PageIndex { get; set; } = 1;
} 