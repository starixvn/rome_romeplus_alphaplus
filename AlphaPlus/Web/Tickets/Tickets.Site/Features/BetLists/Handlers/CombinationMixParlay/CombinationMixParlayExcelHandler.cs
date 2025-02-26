using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.RunningBetListByBetType;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Handlers.BetListByBetType;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CombinationMixParlay;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CombinationMixParlay;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;
using Microsoft.Extensions.Options;
using AlphaPlus.Tickets.Site.Features.Shared.Models;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.CombinationMixParlay;

public class CombinationMixParlayExcelHandler(
    IRunningBetListByBetTypeService runningBetListByBetTypeService,
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options,
    Workbook<TicketDto> workbook,
    IWebHostEnvironment webHostEnvironment) :
        BetListByBetTypeExcelBaseHandler(builder, excelBuilder, ticketHelper, monitorService, ipService, options, webHostEnvironment),
        IRequestHandler<CombinationMixParlayExcelQuery, ExcelFileViewModel>
{
    private Workbook<TicketDto> workbook = workbook;
    private readonly IRunningBetListByBetTypeService runningBetListByBetTypeService = runningBetListByBetTypeService;

    public async Task<ExcelFileViewModel> Handle(CombinationMixParlayExcelQuery message, CancellationToken cancellationToken)
    {
        var param = BetListByBetTypeHandler.CreateRunningBetListByBetTypeParam(message);
        var tickets = await runningBetListByBetTypeService.GetBetList(param);
        var viewModel = new CombinationMixParlayViewModel(message, new BetListData(tickets, null), null, TicketHelper);

        workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
        RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

        var fileContent = workbook
            .Header(Resources.ResourceManager.GetString)
            .Title(viewModel.Title)
            .Body(tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))
            .ExportToBytes();

        return new ExcelFileViewModel(viewModel.Title, fileContent);
    }
}