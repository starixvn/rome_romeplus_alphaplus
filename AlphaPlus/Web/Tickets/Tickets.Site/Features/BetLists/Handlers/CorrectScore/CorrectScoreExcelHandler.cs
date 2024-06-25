using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.CorrectScore;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Handlers.BetListByBetType;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CorrectScore;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CorrectScore;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.CorrectScore;

public class CorrectScoreExcelHandler(
    ICorrectScoreService correctScoreService,
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IWebHostEnvironment webHostEnvironment,
    Workbook<TicketDto> workbook) :
        BetListByBetTypeExcelBaseHandler(builder, excelBuilder, ticketHelper, monitorService, ipService, webHostEnvironment),
        IRequestHandler<CorrectScoreExcelQuery, ExcelFileViewModel>
{
    private Workbook<TicketDto> workbook = workbook;
    private readonly ICorrectScoreService correctScoreService = correctScoreService;

    public async Task<ExcelFileViewModel> Handle(CorrectScoreExcelQuery message, CancellationToken cancellationToken)
    {
        var param = new CorrectScoreParam(message.ViewerId, message.ViewerLevelId, message.DisplayVNIP, message.Site);

        var tickets = await correctScoreService.GetBetList(param);
        var viewModel = new CorrectScoreViewModel(message, new BetListData(tickets, null), null, TicketHelper);

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