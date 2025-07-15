using MediatR;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;

public class ResetCurrentWinAmountCommand : IRequest<Result>
{
    public int CustId { get; set; }
    public string Username { get; set; }
    public int ProductId { get; set; }
    public int SportType { get; set; }
    public int BetType { get; set; }
    public string Language { get; set; }
} 