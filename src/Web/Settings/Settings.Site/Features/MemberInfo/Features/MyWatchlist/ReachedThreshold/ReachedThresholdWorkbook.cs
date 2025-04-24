using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.NPOI;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold
{
    public interface ReachedThresholdWorkbook
    {
        Workbook<ReachlistAccountModel> UseTemplate(TemplateInfo templateInfo);
        ReachedThresholdWorkbook SetData(IEnumerable<ReachlistAccountModel> data);
        ReachedThresholdWorkbook SetRowMappers(Mapper<ReachlistAccountModel> rowMapper);
    }

    public class ReachedThresholdWorkbookImpl : NpoiWorkbook<ReachlistAccountModel>, ReachedThresholdWorkbook
    {
        private const int ReportSheetIndex = 1;

        internal IEnumerable<ReachlistAccountModel> Data { get; set; }

        internal Mapper<ReachlistAccountModel> RowMapper { get; set; }

        internal Row OpenRowTemplate { get; set; }

        internal Row LockedRowTemplateFalsePassword { get; set; }

        internal Row CloseRowTemplateByAdmin { get; set; }

        internal Row CloseRowTemplateByUpline { get; set; }

        internal Row LockedRowTemplateFalseOTPSC { get; set; }

        internal Row CloseRowTemplateByArchive { get; set; }

        internal Row SuspendedRowTemplate { get; set; }

        internal Row DisabledRowTemplate { get; set; }

        public ReachedThresholdWorkbook SetData(IEnumerable<ReachlistAccountModel> data)
        {
            Data = data;

            return this;
        }

        public ReachedThresholdWorkbook SetRowMappers(Mapper<ReachlistAccountModel> rowMapper)
        {
            RowMapper = rowMapper;

            return this;
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            var currentRowIndex = TemplateInfo.BodyStartIndex;

            OpenRowTemplate = sheet.GetRow(currentRowIndex++);
            SuspendedRowTemplate = sheet.GetRow(currentRowIndex++);
            LockedRowTemplateFalsePassword = sheet.GetRow(currentRowIndex++);
            CloseRowTemplateByAdmin = sheet.GetRow(currentRowIndex++);
            CloseRowTemplateByUpline = sheet.GetRow(currentRowIndex++);
            CloseRowTemplateByArchive = sheet.GetRow(currentRowIndex++);
            DisabledRowTemplate = sheet.GetRow(currentRowIndex++);
            LockedRowTemplateFalseOTPSC = sheet.GetRow(currentRowIndex);

            return GetSheetAt(ReportSheetIndex);
        }

        protected override void FillBody(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            RemoveTemplateRows(sheet);

            var hasData = (Data != null && Data.Any());

            if (hasData)
            {
                FillBodyRows(sheet);
            }
            else
            {
                FillNoInfoRow(sheet);
            }
        }

        protected override void FillBodyRows(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            var currentRowIndex = TemplateInfo.BodyStartIndex;

            foreach (var item in Data)
            {
                FillReachlistAccounts(sheet, currentRowIndex++, item);
            }
        }

        internal void FillReachlistAccounts(Sheet sheet, int rowIndex, ReachlistAccountModel account)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(GetTemplateRow(account.Status, account.ClosedReason, account.Locked));
            FillCells(row, account, RowMapper.Mappings);
        }

        private Row GetTemplateRow(string accountStatus, int closedReason, byte locked)
        {
            var templateRow = OpenRowTemplate;

            switch (accountStatus)
            {
                case nameof(ReachedThresholdStatusConstants.Disabled):
                    templateRow = DisabledRowTemplate;
                    break;

                case nameof(ReachedThresholdStatusConstants.Closed):
                    if (closedReason == CloseReasonConstants.ByAdmin)
                    {
                        templateRow = CloseRowTemplateByAdmin;
                    }
                    else if (closedReason == CloseReasonConstants.ByUpline)
                    {
                        templateRow = CloseRowTemplateByUpline;
                    }
                    else if (closedReason == CloseReasonConstants.ByArchive)
                    {
                        templateRow = CloseRowTemplateByArchive;
                    }
                    break;

                case nameof(ReachedThresholdStatusConstants.Suspended):
                    templateRow = SuspendedRowTemplate;
                    break;

                default:
                    if (locked == LockedAccessConstant.FalseOTPSC)
                    {
                        templateRow = LockedRowTemplateFalseOTPSC;
                    }
                    else if (locked == LockedAccessConstant.FalsePassword)
                    {
                        templateRow = LockedRowTemplateFalsePassword;
                    }
                    break;
            }

            return templateRow;
        }

        protected override void FillNoInfoRow(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            var rowIndex = TemplateInfo.BodyStartIndex;
            var colIndex = Column.GetColIndex(TemplateInfo.ColumnStartName);

            sheet.SetCellRangeValue(
                rowIndex,
                colIndex,
                rowIndex,
                OpenRowTemplate.LastCellIndex - 1,
                TemplateInfo.NoInfomationText);

            sheet.GetCell(rowIndex, colIndex).SetCenterHorizontalAlignment();
        }

        protected override void RemoveTemplateRows(Sheet sheet)
        {
            Workbook.RemoveSheetAt(TemplateInfo.SheetIndex);
        }

        protected override void FillSheetName(int index, string name)
        {
            Workbook.SetSheetName(ReportSheetIndex, name);
        }
    }
}