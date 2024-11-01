using AlphaPlus.Core.Extensions;
using AlphaPlus.Reports.Features.BonusDetail;
using AlphaPlus.Reports.Features.Shared.Models;
using MediatR;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.BonusDetail;

public class BonusDetailHandler(IBonusDetailService bonusDetailService) :
    IRequestHandler<BonusDetailQuery, BonusDetailViewModel>,
    IRequestHandler<ConfirmPaymentCommand, Result>
{
    public async Task<BonusDetailViewModel> Handle(BonusDetailQuery message, CancellationToken cancellationToken)
    {
        var report = await bonusDetailService.GetBonusDetail(new BonusDetailParam(
            message.ViewerId,
            message.ViewerLevel.Value,
            message.Member,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            message.StatusId,
            message.IsComplained,
            message.PageIndex,
            message.PageSize));

        return new BonusDetailViewModel(message, report);
    }

    public async Task<Result> Handle(ConfirmPaymentCommand message, CancellationToken cancellationToken)
    {
        var result = await bonusDetailService.ConfirmPayment(new ConfirmPaymentParam(
            message.ViewerId,
            message.SubAccId,
            message.CustId,
            message.TransId,
            message.PaymentDate,
            message.IP,
            message.Language));

        if (result.IsOk)
        {
            result.Message = Resources.ConfirmPaymentSuccessfully;
        }

        return result;
    }
}