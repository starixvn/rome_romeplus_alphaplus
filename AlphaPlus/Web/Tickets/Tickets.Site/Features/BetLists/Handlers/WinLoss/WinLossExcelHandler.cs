using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.BetLists.WinLoss;
using AlphaPlus.Core.Models;
using AlphaPlus.SmartPunter.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using Microsoft.Extensions.Options;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.BetListNPOI;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.WinLoss;

public class WinLossExcelHandler(
    IProductService productService,
    IWinLossService winLossService,
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    ISmartPunterBetListMonitor monitorService,
    IPService ipService,
    IOptions<AppSettings> options,
    AllowedSitesToShareLoad allowedSitesToShareLoad,
    IWebHostEnvironment webHostEnvironment,
    Workbook<TicketDto> workbook) :
        WinLossBaseHandler(productService, winLossService, builder, excelBuilder, ticketHelper, monitorService, ipService, options, allowedSitesToShareLoad, webHostEnvironment),
        IRequestHandler<WinLossExcelQuery, ExcelFileViewModel>
{
    private Workbook<TicketDto> workbook = workbook;

    public async Task<ExcelFileViewModel> Handle(WinLossExcelQuery message, CancellationToken cancellationToken)
    {
        var viewModel = await GetBetList(message, appSettings.BetListMaxTotalRowExportExcel);
        workbook = workbook.UseTemplate(GetTemplateInfo(message.ViewerLevel));
        RtfHelper = message.RtfHelper ?? InitRTFHelper((workbook as NpoiWorkbook<TicketDto>).Workbook);
        var title = string.IsNullOrWhiteSpace(viewModel.OutputParams.ErrMsg)
            ? viewModel.Title
            : $"{viewModel.OutputParams.ErrMsg}{Environment.NewLine}{viewModel.Title}";

        var fileContent = workbook
            .Header(Resources.ResourceManager.GetString)
            .Title(title)
            .Body(viewModel.BetListData.Tickets, GetBodyRowMapper(message.ViewerLevel, viewModel))
            .Footer(new TicketDto(), GetFooterRowMapper(viewModel.Footer))
            .ExportToBytes();

        return new ExcelFileViewModel(viewModel.Title, fileContent);
    }
}