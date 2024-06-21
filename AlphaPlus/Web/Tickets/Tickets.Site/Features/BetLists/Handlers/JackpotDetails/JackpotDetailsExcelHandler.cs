using AlphaPlus.BetLists.AppResources;
using AlphaPlus.BetLists.JackpotDetails;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.JackpotDetails;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.JackpotDetails;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.JackpotDetails;

public class JackpotDetailsExcelHandler(
    IJackpotDetailsService jackpotDetailsService,
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IWebHostEnvironment webHostEnvironment,
    Workbook<TicketDto> workbook) :
        BetListExcelBaseHandler(builder, excelBuilder, ticketHelper, monitorService, ipService, webHostEnvironment),
        IRequestHandler<JackpotDetailsExcelQuery, ExcelFileViewModel>
{
    private Workbook<TicketDto> workbook = workbook;
    private readonly IJackpotDetailsService jackpotDetailsService = jackpotDetailsService;

    public async Task<ExcelFileViewModel> Handle(JackpotDetailsExcelQuery message, CancellationToken cancellationToken)
    {
        var param = new JackpotDetailsParam(message.TransId, message.CustId, message.BetType);
        var tickets = await jackpotDetailsService.GetBetList(param);

        var viewModel = new JackpotDetailsViewModel(message, new BetListData(tickets, null), null, TicketHelper);

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