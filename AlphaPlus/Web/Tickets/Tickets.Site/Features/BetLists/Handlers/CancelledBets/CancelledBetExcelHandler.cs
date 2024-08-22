using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.CancelledBets;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CancelledBets;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CancelledBets;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;
using static ONEbook.SpreadSheets.Column;
using _UserLevel = AlphaPlus.Core.Models.UserLevel;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.CancelledBets;

public class CancelledBetExcelHandler(
    ICancelledBetService cancelledBetService,
    IProductService productService,
    ISmartPunterBetListMonitor monitorService,
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    IPService ipService,
    IOptions<AppSettings> options,
    IWebHostEnvironment webHostEnvironment,
    Workbook<TicketDto> workbook) :
        BetListExcelBaseHandler(builder, excelBuilder, ticketHelper, monitorService, ipService, options, webHostEnvironment),
        IRequestHandler<CancelledBetExcelQuery, ExcelFileViewModel>
{
    private readonly AppSettings appSettings = options.Value;
    private readonly IProductService productService = productService;
    private Workbook<TicketDto> workbook = workbook;
    private readonly CancelledBetHandler cancelledBetHandler = new CancelledBetHandler(
        cancelledBetService,
        productService,
        builder,
        ticketHelper,
        monitorService,
        ipService,
        options);

    public async Task<ExcelFileViewModel> Handle(CancelledBetExcelQuery message, CancellationToken cancellationToken)
    {
        var viewModel = await cancelledBetHandler.Handle(message, cancellationToken);

        workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
        RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);
        var fileContent = workbook
            .Header(Resources.ResourceManager.GetString)
            .Title(GetExcelTitle(viewModel))
            .Body(viewModel.BetListData.Tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))
            .ExportToBytes();
        var fileName = $"{Resources.CancelledBet}-{message.CustName}-{message.FromDate.ToShortDate()}-{message.ToDate.ToShortDate()}";

        return new ExcelFileViewModel(fileName, fileContent);
    }

    protected internal override string ExcelTemplatePath(_UserLevel viewerLevel)
        => GetExcelTemplatePath(viewerLevel.DisplayName);

    protected internal override Mapper<TicketDto> GetBodyRowMapper(_UserLevel viewerLevel, BetListBaseViewModel viewModel)
        => GetTicketDtoRowMapperIncludeUsername(
            viewModel,
            showWinLoss: false).Map(H, ticket => BuildPlatform(ticket.Platform));

    private string GetExcelTitle(CancelledBetViewModel viewModel)
    {
        var status = viewModel.StatusIds.Select(id =>
            id switch
            {
                TicketStatusConstants.Reject => Resources.rejected,
                TicketStatusConstants.Void => Resources.voided,
                TicketStatusConstants.Refund => Resources.refunded,
                _ => string.Empty
            }).JoinWithComma("/");
        var productNames = productService.ConcatProductNames(
            viewModel.ProductViewModel.UserSelectedProductIds.ToListInt(),
            appSettings.IsSiteLabel777).JoinWithComma(", ");

        return $"{status} {Resources.BetList} {viewModel.DateRangePicker.FromDate} --> {viewModel.DateRangePicker.ToDate} {Resources.of} {productNames}.";
    }
}
