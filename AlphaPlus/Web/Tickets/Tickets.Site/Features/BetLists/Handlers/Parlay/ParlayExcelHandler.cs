using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.Parlay;
using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.Parlay;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using MediatR;
using ONEbook.SpreadSheets;
using _UserLevel = AlphaPlus.Core.Models.UserLevel;
using static ONEbook.SpreadSheets.Column;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.Parlay;
using BetListNPOI.SS.UserModel;
using Fanex.BetList.Core.Utils;
using Microsoft.Extensions.Options;
using AlphaPlus.Tickets.Site.Features.Shared.Models;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.Parlay;

public class ParlayExcelHandler(
    IParlayService parlayService,
    IBetListHTMLBuilder builder,
    IBetListExcelBuilder excelBuilder,
    ITicketHelper ticketHelper,
    IPService ipService,
    IOptions<AppSettings> options,
    IWebHostEnvironment webHostEnvironment,
    IComboParlayDetailWorkbook workbook) :
        BetListExcelBaseHandler(builder, excelBuilder, ticketHelper, null, ipService, options, webHostEnvironment),
        IRequestHandler<ComboParlayDetailExcelQuery, ExcelFileViewModel>
{
    private readonly IParlayService parlayService = parlayService;
    private readonly IComboParlayDetailWorkbook workbook = workbook;

    public async Task<ExcelFileViewModel> Handle(ComboParlayDetailExcelQuery message, CancellationToken cancellationToken)
    {
        var comboParlayDetailViewModel = await GetComboParlayDetail(message);

        var onebookWorkbook = workbook
            .SetData(comboParlayDetailViewModel)
            .SetRowMapper(
                DetailsMapper(),
                DetailsSubTotalMapper(comboParlayDetailViewModel),
                FooterMapper(comboParlayDetailViewModel, isTotalWinLost: false),
                FooterMapper(comboParlayDetailViewModel, isTotalWinLost: true))
            .UseTemplate(GetTemplateInfo(message.ViewerLevel));

        RtfHelper = message.RtfHelper ?? InitRTFHelper(workbook.Workbook);

        var fileContent = onebookWorkbook
            .Sheet(comboParlayDetailViewModel.Title)
            .Title(comboParlayDetailViewModel.Title)
            .Header(Resources.ResourceManager.GetString)
            .ExportToBytes();

        return new ExcelFileViewModel("SystemParlay_Details", fileContent);
    }

    protected internal override TemplateInfo GetTemplateInfo(_UserLevel viewerLevel)
        => new TemplateInfo(
            GetExcelTemplatePath("ComboParlayDetail"),
            noInfomationText: Resources.NoInformationIsAvailable,
            bodyStartIndex: 3,
            bodyEndIndex: 5,
            footerEndIndex: 7);

    private Mapper<IEnumerable<ComboParlayDetailWrapper>> DetailsSubTotalMapper(ComboParlayDetailViewModel viewModel)
    {
        var mapper = new Mapper<IEnumerable<ComboParlayDetailWrapper>>();

        return mapper
            .Map(A, _ => $"{Resources.subTotal}:")
            .Map(E, details => RenderNumber(viewModel.SubTotalWinLost(details)));
    }

    private Mapper<IEnumerable<ComboParlayDetailWrapper>> FooterMapper(ComboParlayDetailViewModel viewModel, bool isTotalWinLost)
    {
        var mapper = new Mapper<IEnumerable<ComboParlayDetailWrapper>>();

        mapper
            .Map(A, _ => $"{(isTotalWinLost ? Resources.total : Resources.outstanding)}:")
            .Map(E, _ => RenderNumber(isTotalWinLost ? viewModel.TotalWinLost : viewModel.OutStanding));

        return mapper;
    }

    private Mapper<IEnumerable<ComboParlayDetailWrapper>> DetailsMapper()
    {
        var mapper = new Mapper<IEnumerable<ComboParlayDetailWrapper>>();

        return mapper
            .Map(A, details => details.First().WinLostDate)
            .Map(B, details => RenderEvent(details.First()))
            .Map(C, details => RenderNumber(details.First().Odds))
            .Map(D, details => details.First().Stake)
            .Map(E, details => RenderWinLossComm(details.First()))
            .Map(F, details => details.First().Status);
    }

    private IRichTextString RenderEvent(ComboParlayDetailWrapper detail)
    {
        for (var i = 0; i < detail.Events.Count; i++)
        {
            RtfHelper.RTFRenderer.AddText((i > 0 ? "\n" : string.Empty) + detail.Events[i]);
        }

        var eventRtfText = RtfHelper.RTFRenderer.Render();
        RtfHelper.RTFRenderer.Clear();

        return eventRtfText;
    }

    private IRichTextString RenderWinLossComm(ComboParlayDetailWrapper detail)
    {
        var rtfNumber = new RTFNumber(RtfHelper.RTFRenderer, RtfHelper.PosFont, RtfHelper.NegFont);

        rtfNumber.Render(detail.WinLost + "\n");
        rtfNumber.Render(detail.Comm);

        var rtfText = RtfHelper.RTFRenderer.Render();
        RtfHelper.RTFRenderer.Clear();

        return rtfText;
    }

    private IRichTextString RenderNumber(string numberValue)
    {
        var rtfNumber = new RTFNumber(RtfHelper.RTFRenderer, RtfHelper.PosFont, RtfHelper.NegFont);

        rtfNumber.Render(numberValue);

        var rtfText = RtfHelper.RTFRenderer.Render();
        RtfHelper.RTFRenderer.Clear();

        return rtfText;
    }

    private async Task<ComboParlayDetailViewModel> GetComboParlayDetail(ComboParlayDetailQuery message)
    {
        var comboParlayDetailData = await parlayService.GetComboParlayDetail(
            message.RefNo,
            message.CustId,
            message.Language);

        return new ComboParlayDetailViewModel(message, comboParlayDetailData, TicketHelper, isMobile: false, isExportingExcel: true);
    }
}