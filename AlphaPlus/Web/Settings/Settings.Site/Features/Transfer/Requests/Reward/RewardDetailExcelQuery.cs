using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.Reward;

public class RewardDetailExcelQuery : RewardBaseQuery, IRequest<ExcelFileViewModel>
{
    public RewardDetailExcelQuery()
    {
        PageIndex = 1;
        PageSize = int.MaxValue - 1;
    }
}
