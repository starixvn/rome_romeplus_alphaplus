using MediatR;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;

public class ExcelSportsbookPotentialWinLimitQuery : SportsbookPotentialWinLimitQuery, IRequest<ExcelFileViewModel>
{
} 