using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.BetChoiceMatching;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using BetListNPOI.SS.UserModel;
using BetListNPOI.SS.Util;
using EnumsNET;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Utils;
using Microsoft.Extensions.Options;
using ONEbook.SpreadSheets;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.BetChoiceMatching;

public interface IBetChoiceMatchingWorkbook
{
    ExcelFileViewModel BuildExcelFile(BetChoiceMatchingExcelQuery message, IEnumerable<BetChoiceMatchingEventModel> reportData);
}

public class BetChoiceMatchingWorkbook : ReportNpoiWorkbook<BetChoiceMatchingEventModel>, IBetChoiceMatchingWorkbook
{
    private readonly IBetListExcelBuilder excelBuilder;
    private readonly ITicketHelper ticketHelper;
    private IEnumerable<BetChoiceMatchingEventModel> reportData;
    private decimal totalStake = 0;
    private decimal totalMemberWinLoss = 0;
    private decimal totalSuperWinLoss = 0;
    private decimal totalCompany = 0;
    private bool totalIsRunning = true;

    private int currentRowIndex;
    private Row templateEventRow;
    private Row templateEvenUserTotalRow;
    private Row templateEvenUserDataRow;
    private Row templateOddUserTotalRow;
    private Row templateOddUserDataRow;
    private Row templateTotalRow;
    private Row templateNoInfoRow;
    private ISheet npoiReportSheet;
    private RTFHelper rtfHelper;

    public BetChoiceMatchingWorkbook(
        IWebHostEnvironment webHostEnvironment,
        IBetListExcelBuilder excelBuilder,
        ITicketHelper ticketHelper,
        IOptions<AppSettings> options) : base(webHostEnvironment)
    {
        this.excelBuilder = excelBuilder;
        this.ticketHelper = ticketHelper;
        this.ticketHelper.HideSabaText = options.Value.IsSiteLabel777;
    }

    public ExcelFileViewModel BuildExcelFile(BetChoiceMatchingExcelQuery message, IEnumerable<BetChoiceMatchingEventModel> reportData)
    {
        this.reportData = reportData;

        var fileContent = UseTemplate(
            new TemplateInfo(
                GetExcelTemplatePath("Super"),
                noInfomationText: Resources.NoInformationIsAvailable,
                bodyStartIndex: 3))
            .Title(GetReportTitle(message))
            .Header(Resources.ResourceManager.GetString)
            .ExportToBytes();

        return new ExcelFileViewModel($"{Resources.BetChoiceMatching}-{message.CustName}-{message.EventDate.ToShortDateWithSlash()}", fileContent);
    }

    private static string GetReportTitle(BetChoiceMatchingExcelQuery message)
    {
        var filters = new List<string>
        {
            $"{Resources.MatchingBets} >= {message.BetCount}"
        };

        if (message.Stake.HasValue)
        {
            filters.Add($"{Resources.TotalStake} >= {string.Format("{0:n0}", message.Stake.Value)}");
        }

        if (message.MemberWinLossValue.HasValue)
        {
            filters.Add($"{Resources.TotalMemberWL} {message.MemberWinLossOperator.AsString(EnumFormat.Description)} {message.MemberWinLossValue.Value.FormatNumber()}");
        }

        if (!string.IsNullOrWhiteSpace(message.Username))
        {
            filters.Add($"{Resources.Username}: {message.Username}");
        }

        return $"{Resources.BetChoiceMatching} {message.EventDate.ToShortDateWithSlash()} - {string.Join("; ", filters)}";
    }

    protected override Sheet LoadTemplateSheet()
    {
        currentRowIndex = 0;
        var templateSheet = GetSheetAt(TemplateInfo.SheetIndex);
        templateEventRow = templateSheet.GetRow(currentRowIndex++);
        templateEvenUserTotalRow = templateSheet.GetRow(currentRowIndex++);
        templateEvenUserDataRow = templateSheet.GetRow(currentRowIndex++);
        templateOddUserTotalRow = templateSheet.GetRow(currentRowIndex++);
        templateOddUserDataRow = templateSheet.GetRow(currentRowIndex++);
        templateTotalRow = templateSheet.GetRow(currentRowIndex++);
        templateNoInfoRow = templateSheet.GetRow(currentRowIndex);

        var npoiTemplateSheet = Workbook.GetSheetAt(TemplateInfo.SheetIndex);
        var templateCellStyle = npoiTemplateSheet.GetRow(currentRowIndex).GetCell(0).CellStyle;
        var templateFont = templateCellStyle.GetFont(Workbook);
        rtfHelper = new RTFHelper
        {
            NegFont = templateFont,
            NormalFont = templateFont,
            PosFont = templateFont,
            NegFontCrossed = templateFont,
            NormalFontCrossed = templateFont,
            PosFontCrossed = templateFont,
            RTFRenderer = new RtfTextRender()
        };

        var reportSheetIndex = TemplateInfo.SheetIndex + 1;
        npoiReportSheet = Workbook.GetSheetAt(reportSheetIndex);

        return GetSheetAt(reportSheetIndex);
    }

    protected override void FillBody(Sheet sheet)
    {
        if (sheet == null)
        {
            throw new ArgumentNullException(nameof(sheet));
        }

        RemoveTemplateRows(sheet);

        if (reportData.Any())
        {
            FillBodyRows(sheet);
            FillFooterRows(sheet);
        }
        else
        {
            FillNoInfoRow(sheet);
        }
    }

    protected override void RemoveTemplateRows(Sheet sheet)
        => Workbook.RemoveSheetAt(TemplateInfo.SheetIndex);

    protected override void FillBodyRows(Sheet sheet)
    {
        currentRowIndex = TemplateInfo.BodyStartIndex;

        foreach (var eventModel in reportData)
        {
            var row = sheet.GetRow(currentRowIndex++);
            row.CopyCellsFrom(templateEventRow);
            FillEventRow(row, eventModel);
            var userIndex = 0;
            foreach (var user in eventModel.Users)
            {
                // calculate total
                totalStake += user.Stake;
                if (!user.IsRunning)
                {
                    totalMemberWinLoss += user.MemberWinLoss;
                    totalSuperWinLoss += user.SuperWinLoss;
                    totalCompany += user.Company;
                    totalIsRunning = false;
                }
                // end of calculate total

                var isEvenRow = userIndex % 2 == 0;
                var totalRowIndex = currentRowIndex++;
                var totalRow = sheet.GetRow(totalRowIndex);
                totalRow.CopyCellsFrom(isEvenRow ? templateEvenUserTotalRow : templateOddUserTotalRow);
                FillUserTotalRow(totalRow, user);

                var templateUserDataRow = isEvenRow ? templateEvenUserDataRow : templateOddUserDataRow;
                foreach (var ticket in user.Tickets)
                {
                    row = sheet.GetRow(currentRowIndex++);
                    row.CopyCellsFrom(templateUserDataRow);
                    FillTicketRow(row, ticket);
                }

                npoiReportSheet.AddMergedRegion(new CellRangeAddress(totalRowIndex, totalRowIndex + user.Tickets.Count(), 0, 0));
                userIndex++;
            }
        }
    }

    private void FillEventRow(Row row, BetChoiceMatchingEventModel eventModel)
    {
        var choice = BuildChoice(eventModel);
        row.SetCellValue(Column.GetColIndex(Column.A), choice);
        row.SetCellValue(Column.GetColIndex(Column.H), eventModel.GlobalShowTime);
    }

    private IRichTextString BuildChoice(BetChoiceMatchingEventModel eventModel)
    {
        var ticket = new Ticket
        {
            LeagueId = eventModel.LeagueId,
            LeagueGroupId = eventModel.LeagueGroupId,
            SportTypeId = eventModel.SportType,
            BetTypeId = eventModel.BetType,
            Handicap1 = eventModel.Hdp1,
            Handicap2 = eventModel.Hdp2,
            HomeId = eventModel.HomeId,
            AwayId = eventModel.AwayId,
            BetTeam = eventModel.BetTeam,
            IsLive = eventModel.IsLive,
            LiveHomeScore = eventModel.LiveHomeScore,
            LiveAwayScore = eventModel.LiveAwayScore
        };

        var choiceBuilder = excelBuilder.CreateChoiceBuilder(ticket.BetTypeId);
        return choiceBuilder.RenderRTF(ticket, ticketHelper, null, false, rtfHelper).FirstOrDefault();
    }

    private static void FillUserTotalRow(Row row, BetChoiceMatchingUserModel user)
    {
        row.SetCellValue(Column.GetColIndex(Column.A), user.Username);
        row.SetCellValue(Column.GetColIndex(Column.B), Resources.Total);
        row.SetCellValue(Column.GetColIndex(Column.F), user.Stake);
        if (!user.IsRunning)
        {
            row.SetCellValue(Column.GetColIndex(Column.G), user.MemberWinLoss);
            row.SetCellValue(Column.GetColIndex(Column.H), user.SuperWinLoss);
            row.SetCellValue(Column.GetColIndex(Column.I), user.Company);
        }
    }

    private static void FillTicketRow(Row row, BetChoiceMatchingTicketModel ticket)
    {
        row.SetCellValue(Column.GetColIndex(Column.A), ticket.Username);
        row.SetCellValue(Column.GetColIndex(Column.B), ticket.TransDate);
        row.SetCellValue(Column.GetColIndex(Column.C), ticket.TransIdString);
        row.SetCellValue(Column.GetColIndex(Column.D), BuildOdds(ticket));
        row.SetCellValue(Column.GetColIndex(Column.E), ticket.IP ?? string.Empty);
        row.SetCellValue(Column.GetColIndex(Column.F), ticket.Stake);
        if (!ticket.IsRunning)
        {
            row.SetCellValue(Column.GetColIndex(Column.G), ticket.MemberWinLoss);
            row.SetCellValue(Column.GetColIndex(Column.H), ticket.SuperWinLoss);
            row.SetCellValue(Column.GetColIndex(Column.I), ticket.Company);
        }
    }

    private static string BuildOdds(BetChoiceMatchingTicketModel ticket)
    {
        if (ticket.Odds == 0)
        {
            return "-";
        }

        return string.IsNullOrEmpty(ticket.DisplayOddsType)
            ? ticket.DisplayOdds
            : $"({ticket.DisplayOddsType}) {ticket.DisplayOdds}";
    }

    protected override void FillFooterRows(Sheet sheet)
    {
        var row = sheet.GetRow(currentRowIndex++);
        row.CopyCellsFrom(templateTotalRow);
        row.SetCellValue(Column.GetColIndex(Column.A), Resources.Total);
        row.SetCellValue(Column.GetColIndex(Column.F), totalStake);
        if (!totalIsRunning)
        {
            row.SetCellValue(Column.GetColIndex(Column.G), totalMemberWinLoss);
            row.SetCellValue(Column.GetColIndex(Column.H), totalSuperWinLoss);
            row.SetCellValue(Column.GetColIndex(Column.I), totalCompany);
        }
    }

    protected override void FillNoInfoRow(Sheet sheet)
    {
        var row = sheet.GetRow(TemplateInfo.BodyStartIndex);
        row.CopyCellsFrom(templateNoInfoRow);
        row.SetCellValue(Column.GetColIndex(TemplateInfo.ColumnStartName), TemplateInfo.NoInfomationText);
    }
}