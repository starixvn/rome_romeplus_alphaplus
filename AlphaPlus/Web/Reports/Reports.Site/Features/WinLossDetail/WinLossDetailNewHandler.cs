using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailNewHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    IEnteredResultService enteredResultService,
    IWinLossDetailService winlossDetailService) :
        BaseHandler(productService, options),
        IRequestHandler<WinLossDetailNewQuery, WinLossDetailNewViewModel>,
        IRequestHandler<WinLossDetailGetDataQuery, ReportData<WinLossDetailModel>>
{
    public async Task<WinLossDetailNewViewModel> Handle(WinLossDetailNewQuery message, CancellationToken cancellationToken)
    {
        var (productViewModel, _) = BuildProductViewModel(message, message.UserSelectedProductIds, message.UserSelectedResourceIds);
        var enteredResult = await enteredResultService.GetTotalEnteredResults(message.Site);

        return new WinLossDetailNewViewModel(message, productViewModel, enteredResult);
    }

    public Task<ReportData<WinLossDetailModel>> Handle(WinLossDetailGetDataQuery message, CancellationToken cancellationToken)
    {
        var (validUserSelectedProductIds, productDetailXml) = FilterProductAndProductDetailIds(
            message,
            message.UserSelectedProductIds,
            message.UserSelectedResourceIds);

        return GetWinLossDetail(message, validUserSelectedProductIds.JoinWithComma(), productDetailXml);
    }

    private Task<ReportData<WinLossDetailModel>> GetWinLossDetail(
        WinLossDetailNewQuery message,
        string validUserSelectedProductIds,
        string productDetailXml)
    {
        var param = new WinLossDetailParam(
            message.CustId,
            message.CustLevel,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            validUserSelectedProductIds,
            productDetailXml,
            message.Username,
            message.Site);

        return winlossDetailService.GetWinLossDetail(param);
    }
}