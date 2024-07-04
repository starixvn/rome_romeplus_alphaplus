using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.Running;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetType;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.BetListByBetType;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;
using Microsoft.Extensions.Options;
using AlphaPlus.Tickets.Site.Features.Shared.Models;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.BetListByBetType;

public class BetListByBetTypeExcelHandler(
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
        IRequestHandler<BetListByBetTypeExcelQuery, ExcelFileViewModel>
{
    private Workbook<TicketDto> workbook = workbook;
    private readonly IRunningBetListByBetTypeService runningBetListByBetTypeService = runningBetListByBetTypeService;

    public async Task<ExcelFileViewModel> Handle(BetListByBetTypeExcelQuery message, CancellationToken cancellationToken)
    {
        var param = new RunningBetListByBetTypeParam(
            message.ViewerId,
            message.ViewerLevelId,
            message.BetType,
            message.DisplayVNIP,
            message.Site);
        var tickets = await runningBetListByBetTypeService.GetBetList(param);

        var viewModel = new BetListByBetTypeViewModel(message, new BetListData(tickets, null), null, TicketHelper);

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