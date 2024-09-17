using AlphaPlus.BetLists.RunningBetListByBetTypeAndMatch;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Shared.AppResources.Betlist.Site;
using AlphaPlus.Tickets.Site.Features.BetLists.Handlers.BetListByBetType;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetTypeAndMatch;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetTypeAndMatch;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;
using Microsoft.Extensions.Options;
using AlphaPlus.Tickets.Site.Features.Shared.Models;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.BetListByBetTypeAndMatch;

public class BetListByBetTypeAndMatchExcelHandler(
    Workbook<TicketDto> workbook,
    IRunningBetListByBetTypeAndMatchService runningBetListByBetTypeAndMatchService,
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options,
    IWebHostEnvironment webHostEnvironment) :
        BetListByBetTypeExcelBaseHandler(builder, excelBuilder, ticketHelper, monitorService, ipService, options, webHostEnvironment),
        IRequestHandler<BetListByBetTypeAndMatchExcelQuery, ExcelFileViewModel>
{
    private Workbook<TicketDto> workbook = workbook;
    private readonly IRunningBetListByBetTypeAndMatchService runningBetListByBetTypeAndMatchService = runningBetListByBetTypeAndMatchService;

    public async Task<ExcelFileViewModel> Handle(BetListByBetTypeAndMatchExcelQuery message, CancellationToken cancellationToken)
    {
        var param = BetListByBetTypeAndMatchHandler.CreateRunningBetListByBetTypeAndMatchParam(message);
        var betListData = await runningBetListByBetTypeAndMatchService.GetBetList(param);

        var viewModel = new BetListByBetTypeAndMatchViewModel(message, betListData, null, TicketHelper);

        workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
        RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

        var fileContent = workbook
            .Header(Resources.ResourceManager.GetString)
            .Title(viewModel.Title)
            .Body(betListData.Tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))
            .ExportToBytes();

        return new ExcelFileViewModel(viewModel.Title, fileContent);
    }
}