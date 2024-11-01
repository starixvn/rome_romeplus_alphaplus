using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.WinLossByMatch;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLossByMatch;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Blocks;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLossByMatch;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;
using static ONEbook.SpreadSheets.Column;
using Microsoft.Extensions.Options;
using AlphaPlus.Tickets.Site.Features.Shared.Models;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.WinLossByMatch;

public class WinLossByMatchExcelHandler(
    IWinLossByMatchService winLossByMatchService,
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options,
    IWebHostEnvironment webHostEnvironment,
    Workbook<TicketDto> workbook) :
        BetListExcelBaseHandler(builder, excelBuilder, ticketHelper, monitorService, ipService, options, webHostEnvironment),
        IRequestHandler<WinLossByMatchExcelQuery, ExcelFileViewModel>
{
    private readonly IWinLossByMatchService winLossByMatchService = winLossByMatchService;
    private Workbook<TicketDto> workbook = workbook;

    public async Task<ExcelFileViewModel> Handle(WinLossByMatchExcelQuery message, CancellationToken cancellationToken)
    {
        var param = WinLossByMatchHandler.CreateWinLossParam(message);
        var betlistData = await winLossByMatchService.GetBetList(param);
        var viewModel = new WinLossByMatchViewModel(message, betlistData, null, TicketHelper);

        var custLevel = Core.Models.Enumeration.FromValue<UserLevel>(message.CustLevelId);
        workbook = workbook.UseTemplate(GetTemplateInfo(custLevel));
        RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

        var fileContent = workbook
            .Header(Resources.ResourceManager.GetString)
            .Title(viewModel.Title)
            .Body(betlistData.Tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))
            .Footer(new TicketDto(), GetFooterRowMapper(viewModel.Footer))
            .ExportToBytes();

        return new ExcelFileViewModel(viewModel.Title, fileContent);
    }

    protected internal override string ExcelTemplatePath(UserLevel viewerLevel)
        => GetExcelTemplatePath(viewerLevel.DisplayName);

    protected internal override Mapper<TicketDto> GetBodyRowMapper(UserLevel viewerLevel, BetListBaseViewModel viewModel)
    {
        var ticketData = viewModel.BetListData.TicketData;
        var mapper = GetTicketDtoRowMapperIncludeUsername(viewModel, showWinLoss: true);

        mapper.Map(I, ticket => BuildPtCommmission(ticket, ticketData, viewerLevel));

        if (viewerLevel.IsMasterLevel())
        {
            mapper.Map(J, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));
        }

        if (viewerLevel.IsSuperLevel())
        {
            mapper
                .Map(J, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Master))
                .Map(K, ticket => BuildPtCommmission(ticket, ticketData, UserLevel.Agent));
        }

        string platformColId = J;
        if (viewerLevel.IsSuperLevel())
        {
            platformColId = L;
        }
        else if (viewerLevel.IsMasterLevel())
        {
            platformColId = K;
        }

        mapper.Map(platformColId, ticket => BuildPlatform(ticket.Platform));

        return mapper;
    }

    protected internal override Mapper<TicketDto> GetFooterRowMapper(Footer footer)
    {
        var mapper = base.GetFooterRowMapper(footer);
        mapper.Map(G, _ => BuildFooter(footer));

        return mapper;
    }
}