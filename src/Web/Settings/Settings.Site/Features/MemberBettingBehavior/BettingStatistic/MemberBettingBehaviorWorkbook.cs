using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.NPOI;

namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior.BettingStatistic
{
    public interface IMemberBettingBehaviorWorkbook
    {
        Workbook<BettingStatisticExportRow> UseTemplate(TemplateInfo templateInfo);

        IMemberBettingBehaviorWorkbook SetData(IEnumerable<BettingStatisticExportRow> bettingStatistics, BettingStatisticExportRow total);

        IMemberBettingBehaviorWorkbook SetRowMappers(Mapper<BettingStatisticExportRow> memberBettingBehaviorMapper, Mapper<BettingStatisticExportRow> totalMapper);
    }

    public class MemberBettingBehaviorWorkbook : NpoiWorkbook<BettingStatisticExportRow>, IMemberBettingBehaviorWorkbook
    {
        private const int DataSheetIndex = 1;
        private int currentRowIndex;

        internal IEnumerable<BettingStatisticExportRow> BettingStatistics { get; set; }

        internal BettingStatisticExportRow Total { get; set; }

        internal Mapper<BettingStatisticExportRow> MemberBettingBehaviorMapper { get; set; }

        internal Mapper<BettingStatisticExportRow> TotalMapper { get; set; }

        private Row NormalRowTemplate { get; set; }

        internal Row TotalRowTemplate { get; set; }

        public IMemberBettingBehaviorWorkbook SetData(IEnumerable<BettingStatisticExportRow> bettingStatistics, BettingStatisticExportRow total)
        {
            BettingStatistics = bettingStatistics;
            Total = total;
            return this;
        }

        public IMemberBettingBehaviorWorkbook SetRowMappers(Mapper<BettingStatisticExportRow> memberBettingBehaviorMapper, Mapper<BettingStatisticExportRow> totalMapper)
        {
            MemberBettingBehaviorMapper = memberBettingBehaviorMapper;
            TotalMapper = totalMapper;
            return this;
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);
            var currentRowIndex = TemplateInfo.BodyStartIndex;

            NormalRowTemplate = sheet.GetRow(currentRowIndex++);
            TotalRowTemplate = sheet.GetRow(currentRowIndex++);

            var reportSheet = GetSheetAt(DataSheetIndex);
            reportSheet.FooterRowsTemplate = new List<Row> { TotalRowTemplate };
            // OddRowTemplate is for showing No Data.
            reportSheet.OddRowTemplate = NormalRowTemplate;

            return reportSheet;
        }

        protected override void FillBody(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            RemoveTemplateRows(sheet);

            if (BettingStatistics.Any())
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

            foreach (var item in BettingStatistics)
            {
                var row = sheet.GetRow(currentRowIndex++);
                row.CopyCellsFrom(NormalRowTemplate);
                FillCells(row, item, MemberBettingBehaviorMapper.Mappings);
            }

            var totalRow = sheet.GetRow(currentRowIndex++);
            totalRow.CopyCellsFrom(TotalRowTemplate);
            FillCells(totalRow, Total, TotalMapper.Mappings);
        }
    }
}