using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.Reward;

public class RewardPaymentExcelQuery : RewardBaseQuery, IRequest<ExcelFileViewModel>
{
}