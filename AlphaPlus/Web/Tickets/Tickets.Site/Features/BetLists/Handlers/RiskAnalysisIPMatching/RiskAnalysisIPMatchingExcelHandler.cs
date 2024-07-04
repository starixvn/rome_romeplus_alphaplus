using AlphaPlus.Shared.AppResources.Betlist.Builders;
using AlphaPlus.BetLists.RiskAnalysisIPMatching;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.RiskAnalysisIPMatching;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Blocks;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.RiskAnalysisIPMatching;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;
using static ONEbook.SpreadSheets.Column;
using _UserLevel = AlphaPlus.Core.Models.UserLevel;
using Microsoft.Extensions.Options;
using AlphaPlus.Tickets.Site.Features.Shared.Models;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.RiskAnalysisIPMatching;

public class RiskAnalysisIPMatchingExcelHandler(
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options,
    IRiskAnalysisIPMatchingService riskAnalysisIPMatchingService,
    IWebHostEnvironment webHostEnvironment,
    Workbook<TicketDto> workbook) :
        BetListExcelBaseHandler(builder, excelBuilder, ticketHelper, monitorService, ipService, options, webHostEnvironment),
        IRequestHandler<RiskAnalysisIPMatchingExcelQuery, ExcelFileViewModel>
{
    private readonly IRiskAnalysisIPMatchingService riskAnalysisIPMatchingService = riskAnalysisIPMatchingService;
    private Workbook<TicketDto> workbook = workbook;
    private int currentRowIndex = 1;

    public async Task<ExcelFileViewModel> Handle(RiskAnalysisIPMatchingExcelQuery message, CancellationToken cancellationToken)
    {
        var param = new RiskAnalysisIPMatchingParam(
            message.CustId,
            message.IP,
            message.Date,
            message.DisplayVNIP,
            message.Site);
        var betlistData = await riskAnalysisIPMatchingService.GetBetList(param);
        var viewModel = new RiskAnalysisIPMatchingViewModel(message, betlistData, Builder, TicketHelper);

        workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
        RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

        var fileContent = workbook
            .Header(Resources.ResourceManager.GetString)
            .Title(viewModel.Title)
            .Body(betlistData.Tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))
            .Footer(new TicketDto(), GetFooterRowMapper(viewModel.Footer))
            .ExportToBytes();

        return new ExcelFileViewModel(viewModel.Title, fileContent);
    }

    protected internal override Mapper<TicketDto> GetBodyRowMapper(_UserLevel viewerLevel, BetListBaseViewModel viewModel)
    {
        var ticketData = viewModel.BetListData.TicketData;
        var mapper = new Mapper<TicketDto>()
            .Map(A, _ => currentRowIndex++)
            .Map(B, ticket => ticket.UserName)
            .Map(C, ticket => BuildTrans(ticket))
            .Map(D, ticket => BuildChoice(ticket, viewModel))
            .Map(E, ticket => BuildOdds(ticket, viewModel))
            .Map(F, ticket => BuildStake(ticket))
            .Map(G, ticket => BuildWinLoss(ticket))
            .Map(H, ticket => BuildStatus(ticket, viewModel))
            .Map(I, ticket => BuildPtCommmission(ticket, ticketData, viewerLevel))
            .Map(J, ticket => BuildPtCommmission(ticket, ticketData, _UserLevel.Master))
            .Map(K, ticket => BuildPtCommmission(ticket, ticketData, _UserLevel.Agent))
            .Map(L, ticket => BuildPlatform(ticket.Platform));

        return mapper;
    }

    protected internal override Mapper<TicketDto> GetFooterRowMapper(Footer footer)
        => new Mapper<TicketDto>()
            .Map(A, _ => AlphaPlus.BetLists.Builders.BetListExcelBuilderExt.BuildFooterHeader(RtfHelper))
            .Map(G, _ => BuildFooter(footer));
}