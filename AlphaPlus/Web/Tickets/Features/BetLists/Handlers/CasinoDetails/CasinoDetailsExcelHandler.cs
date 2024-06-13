using AlphaPlus.BetLists.AppResources;
using AlphaPlus.BetLists.CasinoDetails;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CasinoDetails;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CasinoDetails;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Components;
using AlphaPlus.Utilities.IP;
using BetListNPOI.SS.UserModel;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;
using static ONEbook.SpreadSheets.Column;
using _UserLevel = AlphaPlus.Core.Models.UserLevel;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.CasinoDetails;

public class CasinoDetailsExcelHandler(
    ICasinoDetailsService casinoDetailsService,
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IWebHostEnvironment webHostEnvironment,
    Workbook<TicketDto> workbook) :
        BetListExcelBaseHandler(builder, excelBuilder, ticketHelper, monitorService, ipService, webHostEnvironment),
        IRequestHandler<CasinoDetailsExcelQuery, ExcelFileViewModel>
{
    private int currentRowIndex = 1;
    private Workbook<TicketDto> workbook = workbook;
    private readonly ICasinoDetailsService casinoDetailsService = casinoDetailsService;

    public async Task<ExcelFileViewModel> Handle(CasinoDetailsExcelQuery message, CancellationToken cancellationToken)
    {
        var param = new CasinoDetailsParam(
            message.RefNo,
            message.CustId,
            message.TransType,
            message.ProductId,
            message.PageIndex,
            message.PageSize,
            message.DisplayVNIP);
        var pagedBetListData = await casinoDetailsService.GetBetList(param);
        var viewModel = new CasinoDetailsViewModel(
            message,
            new BetListData(pagedBetListData.DataSource, null),
            new PagerViewModel(pagedBetListData.PageIndex, pagedBetListData.PageSize, pagedBetListData.TotalRecords),
            null,
            TicketHelper);

        workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
        RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);

        var fileContent = workbook
            .Header(Resources.ResourceManager.GetString)
            .Title(viewModel.Title)
            .Body(pagedBetListData.DataSource, GetBodyRowMapper(message.ViewerLevel, message.DisplayVNIP))
            .ExportToBytes();

        return new ExcelFileViewModel(viewModel.Title, fileContent);
    }

    internal Mapper<TicketDto> GetBodyRowMapper(_UserLevel viewerLevel, bool displayVNIP)
    {
        var mapper = new Mapper<TicketDto>()
            .Map(A, _ => currentRowIndex++)
            .Map(B, ticket => BuildTrans(ticket))
            .Map(C, ticket => BuildChoice(ticket))
            .Map(D, ticket => BuildOdds(ticket))
            .Map(E, ticket => BuildStake(ticket))
            .Map(F, ticket => BuildWinLoss(ticket))
            .Map(G, ticket => BuildStatus(ticket, displayVNIP))
            .Map(H, ticket => BuildPtCommmission(ticket, null, viewerLevel));

        if (viewerLevel.IsMasterLevel())
        {
            mapper.Map(I, ticket => BuildPtCommmission(ticket, null, _UserLevel.Agent));
        }

        if (viewerLevel.IsSuperLevel())
        {
            mapper
                .Map(I, ticket => BuildPtCommmission(ticket, null, _UserLevel.Master))
                .Map(J, ticket => BuildPtCommmission(ticket, null, _UserLevel.Agent));
        }

        AddPlatform(mapper, viewerLevel);

        return mapper;
    }

    private IRichTextString BuildChoice(ITicket ticket)
        => ExcelBuilder
            .CreateChoiceBuilder(ticket.BetTypeId)
            .RenderRTF(ticket, TicketHelper, null, false, RtfHelper)
            .FirstOrDefault();

    private IRichTextString BuildOdds(ITicket ticket)
        => ExcelBuilder
            .CreateOddsBuilder(ticket.BetTypeId)
            .RenderRTF(ticket, null, GetOddsTypeName, RtfHelper);

    private IRichTextString BuildStatus(TicketDto ticket, bool displayVNIP)
    {
        RtfHelper.RTFRenderer.AddText(ExcelBuilder
            .CreateStatusBuilder(ticket.BetTypeId)
            .RenderRTF(ticket, TicketHelper, null, displayVNIP, RtfHelper)
            .ToString());

        if (!string.IsNullOrWhiteSpace(ticket.Country))
        {
            RtfHelper.RTFRenderer.AddText("\n" + ticket.Country);
        }

        var rtfText = RtfHelper.RTFRenderer.Render();
        RtfHelper.RTFRenderer.Clear();

        return rtfText;
    }
}