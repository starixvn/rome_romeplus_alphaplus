// using AlphaPlus.BetLists.Shared.Services;
// using AlphaPlus.BetLists.WinLoss;
// using AlphaPlus.SmartPunter.Services;
// using AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;
// using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;
// using AlphaPlus.Utilities.IP;
// using Fanex.BetList.Core.Builder;
// using Fanex.BetList.Core.Entities;
// using MediatR;

// namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.WinLoss;

// public class WinLossHandler(
//     IProductService productService,
//     IWinLossService winLossService,
//     IBetListHTMLBuilder builder,
//     ITicketHelper ticketHelper,
//     ISmartPunterBetListMonitor monitorService,
//     IPService ipService) :
//         BetListBaseHandler(builder, ticketHelper, monitorService, ipService),
//         IRequestHandler<WinLossQuery, WinLossViewModel>
// {
//     private readonly IWinLossService winLossService = winLossService;
//     private readonly IProductService productService = productService;

//     public async Task<WinLossViewModel> Handle(WinLossQuery message)
//     {
//         var productByHLParam = new GetProductsByGroupHeadlineParam(message.ViewerId, message.MinDate);
//         var validUserSelectedProductIds = message.IsGroupHeadline
//             ? productService.FilterProductIdsByGroupHeadline(message.ProductIds.ToListInt(), productByHLParam)
//             : productService.FilterProductIds(message.Site, message.CurrencyId, message.MinDate, message.ProductIds.ToListInt());
//         var validResourceIds = await productDetailService.FilterResourceIdsByCustomer(message.Site, message.CurrencyId, message.ResourceIds);
//         var productDetailsXML = await productDetailService.BuildProductDetailsXMLAsync(validResourceIds, message.Is789Y);

//         var showContributionAmountRoundNote = message.IsGroupHeadline
//             ? productService.IsEnabledJackpot(message.Site, message.CurrencyId)
//             : message.IsEnabledJackpot;

//         var winLossParam = CreateWinLossParam(message, validUserSelectedProductIds, productDetailsXML, message.MaxTotalRow, message.MaxTotalRowConfig);
//         var betlistData = await winLossService.GetBetList(winLossParam);

//         WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

//         return new WinLossViewModel(message, betlistData, Builder, TicketHelper, showContributionAmountRoundNote, message.IsGroupHeadline, betlistOutputParams: betlistData.BetlistOutputParams);
//     }

//     protected internal static WinLossParam CreateWinLossParam(WinLossQuery message, IEnumerable<int> productIds, string productDetailsXML, int maxTotalRow = 10000, int maxTotalRowConfig = 10000)
//         => new WinLossParam(
//            message.CustId,
//            productIds,
//            message.FromDate,
//            message.ToDate,
//            message.LimitedDateToShowInfo,
//            message.Language,
//            productDetailsXML,
//            maxTotalRow,
//            maxTotalRowConfig);
// }
