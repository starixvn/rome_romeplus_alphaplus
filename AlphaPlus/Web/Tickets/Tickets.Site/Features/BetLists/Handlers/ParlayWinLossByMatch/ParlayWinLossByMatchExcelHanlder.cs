using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.ParlayWinLossByMatch;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Handlers.WinLossByMatch;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.ParlayWinLossByMatch;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.ParlayWinLossByMatch;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;
using _UserLevel = AlphaPlus.Core.Models.UserLevel;
using Microsoft.Extensions.Options;
using AlphaPlus.Tickets.Site.Features.Shared.Models;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.ParlayWinLossByMatch;

public class ParlayWinLossByMatchExcelHanlder(
    IParlayWinLossByMatchService parlayWinLossByMatchService,
    ISmartPunterBetListMonitor monitorService,
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    IPService ipService,
    IOptions<AppSettings> options,
    IWebHostEnvironment webHostEnvironment,
    Workbook<TicketDto> workbook) :
        BetListExcelBaseHandler(builder, excelBuilder, ticketHelper, monitorService, ipService, options, webHostEnvironment),
        IRequestHandler<ParlayWinLossByMatchExcelQuery, ExcelFileViewModel>
{
    private readonly IParlayWinLossByMatchService parlayWinLossByMatchService = parlayWinLossByMatchService;
    private Workbook<TicketDto> workbook = workbook;
    private readonly WinLossByMatchExcelHandler winLossByMatchExcelHandler = new WinLossByMatchExcelHandler(
        null,
        builder,
        excelBuilder,
        ticketHelper,
        monitorService,
        ipService,
        options,
        webHostEnvironment,
        workbook);

    public async Task<ExcelFileViewModel> Handle(ParlayWinLossByMatchExcelQuery message, CancellationToken cancellationToken)
    {
        var param = ParlayWinLossByMatchHandler.CreateWinLossParam(message);
        var tickets = await parlayWinLossByMatchService.GetBetList(param);
        var viewModel = new ParlayWinLossByMatchViewModel(message, new BetListData(tickets, null), null, TicketHelper);

        var custLevel = Core.Models.Enumeration.FromValue<_UserLevel>(message.CustLevelId);
        workbook = workbook.UseTemplate(winLossByMatchExcelHandler.GetTemplateInfo(custLevel));
        winLossByMatchExcelHandler.RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

        var fileContent = workbook
            .Header(Resources.ResourceManager.GetString)
            .Title(viewModel.Title)
            .Body(tickets, winLossByMatchExcelHandler.GetBodyRowMapper(message.ViewerLevel, viewModel))
            .Footer(new TicketDto(), winLossByMatchExcelHandler.GetFooterRowMapper(viewModel.Footer))
            .ExportToBytes();

        return new ExcelFileViewModel(viewModel.Title, fileContent);
    }
}
