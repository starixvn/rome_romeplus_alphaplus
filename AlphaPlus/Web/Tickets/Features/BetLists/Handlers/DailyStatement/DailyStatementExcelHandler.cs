using AlphaPlus.BetLists.AppResources;
using AlphaPlus.BetLists.DailyStatement;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.DailyStatement;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.DailyStatement;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;
using _UserLevel = AlphaPlus.Core.Models.UserLevel;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.DailyStatement;

public class DailyStatementExcelHandler(
    IDailyStatementService dailyStatementService,
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    Workbook<TicketDto> workbook,
    IWebHostEnvironment webHostEnvironment) :
        BetListExcelBaseHandler(builder, excelBuilder, ticketHelper, monitorService, ipService, webHostEnvironment),
        IRequestHandler<DailyStatementExcelQuery, ExcelFileViewModel>
{
    private readonly IDailyStatementService dailyStatementService = dailyStatementService;
    private Workbook<TicketDto> workbook = workbook;

    public async Task<ExcelFileViewModel> Handle(DailyStatementExcelQuery message, CancellationToken cancellationToken)
    {
        var param = new DailyStatementParam(
            message.CustId,
            message.WinLossDate,
            message.TransType,
            message.DisplayVNIP,
            message.Site);
        var betlistData = await dailyStatementService.GetBetList(param);
        var dailyStatementViewModel = new DailyStatementViewModel(message, betlistData, Builder, TicketHelper);

        workbook = workbook.UseTemplate(GetTemplateInfo(_UserLevel.Member));
        RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

        var fileContent = workbook
            .Header(Resources.ResourceManager.GetString)
            .Title(dailyStatementViewModel.Title)
            .Body(betlistData.Tickets, GetBodyRowMapper(message.ViewerLevel, dailyStatementViewModel))
            .Footer(new TicketDto(), GetFooterRowMapper(dailyStatementViewModel.Footer))
            .ExportToBytes();

        return new ExcelFileViewModel("BetList", fileContent);
    }

    protected internal override Mapper<TicketDto> GetBodyRowMapper(_UserLevel viewerLevel, BetListBaseViewModel viewModel)
    {
        var mapper = GetTicketDtoRowMapperExcludeUsername(viewModel);

        mapper.Map(Column.H, ticket => BuildPlatform(ticket.Platform));

        return mapper;
    }
}