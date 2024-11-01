using AlphaPlus.Shared.AppResources.Betlist.Services;
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
using Microsoft.Extensions.Options;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using static ONEbook.SpreadSheets.Column;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.DailyStatement;

public class DailyStatementExcelHandler(
    IDailyStatementService dailyStatementService,
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options,
    Workbook<TicketDto> workbook,
    IWebHostEnvironment webHostEnvironment) :
        BetListExcelBaseHandler(builder, excelBuilder, ticketHelper, monitorService, ipService, options, webHostEnvironment),
        IRequestHandler<DailyStatementExcelQuery, ExcelFileViewModel>
{
    private readonly IDailyStatementService dailyStatementService = dailyStatementService;
    private Workbook<TicketDto> workbook = workbook;

    public async Task<ExcelFileViewModel> Handle(DailyStatementExcelQuery message, CancellationToken cancellationToken)
    {
        var param = DailyStatementHandler.CreateDailyStatementParam(message);
        var betlistData = await dailyStatementService.GetBetList(param);
        var viewModel = new DailyStatementViewModel(message, betlistData, Builder, TicketHelper, supportRiskAmount: message.SupportRiskAmount);

        workbook = workbook.UseTemplate(message.SupportRiskAmount ? GetRiskAmountTemplateInfo(message.ViewerLevel) : GetTemplateInfo(message.ViewerLevel));
        RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

        var fileContent = workbook
            .Header(Resources.ResourceManager.GetString)
            .Title(viewModel.Title)
            .Body(betlistData.Tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))
            .Footer(new TicketDto(), GetFooterRowMapper(viewModel.Footer))
            .ExportToBytes();

        return new ExcelFileViewModel("BetList", fileContent);
    }

    protected internal override Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
    {
        var mapper = GetTicketDtoRowMapperExcludeUsername(viewModel);

        if (viewModel.SupportRiskAmount)
        {
            mapper.Map(H, ticket => ticket.MemberWinText);
            mapper.Map(I, ticket => viewModel.GetSMARiskAmount(ticket));
            mapper.Map(J, ticket => BuildPlatform(ticket.Platform));
        }
        else
        {
            mapper.Map(H, ticket => BuildPlatform(ticket.Platform));
        }

        return mapper;
    }

    private TemplateInfo GetRiskAmountTemplateInfo(UserLevel viewerLevel)
        => new TemplateInfo(
            GetExcelTemplatePath($"{viewerLevel.DisplayName}-RiskAmount"),
            noInfomationText: Resources.NoInformationIsAvailable,
            bodyStartIndex: 3,
            bodyEndIndex: 4,
            footerEndIndex: 5);
}