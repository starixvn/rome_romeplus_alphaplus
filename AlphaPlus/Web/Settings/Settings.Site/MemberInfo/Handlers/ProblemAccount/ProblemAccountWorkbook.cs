namespace Alpha.MemberInfo.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Settings.MemberInfo.Domain.Constants;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.ProblemAccount;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;

    public interface ProblemAccountWorkbook
    {
        Workbook<ProblemAccountModel> UseTemplate(TemplateInfo templateInfo);

        ProblemAccountWorkbook SetData(
            IEnumerable<ProblemAccountModel> data);

        ProblemAccountWorkbook SetRowMappers(
            Mapper<ProblemAccountModel> rowMapper);
    }

    public class ProblemAccountWorkbookImpl :
        NpoiWorkbook<ProblemAccountModel>,
        ProblemAccountWorkbook
    {
        private const int ReportSheetIndex = 1;

        internal IEnumerable<ProblemAccountModel> Data { get; set; }

        internal Mapper<ProblemAccountModel> RowMapper { get; set; }

        internal Row MatchTitleRowTemplate { get; set; }

        internal Row OpenRowTemplate { get; set; }

        internal Row LockedRowTemplateFalsePassword { get; set; }

        internal Row CloseRowTemplateByAdmin { get; set; }

        internal Row CloseRowTemplateByUpline { get; set; }

        internal Row LockedRowTemplateFalseOTPSC { get; set; }

        internal Row CloseRowTemplateByArchive { get; set; }

        internal Row SuspendedRowTemplate { get; set; }

        internal Row DisabledRowTemplate { get; set; }

        public ProblemAccountWorkbook SetData(
            IEnumerable<ProblemAccountModel> data)
        {
            Data = data;

            return this;
        }

        public ProblemAccountWorkbook SetRowMappers(
            Mapper<ProblemAccountModel> rowMapper)
        {
            RowMapper = rowMapper;

            return this;
        }

        internal void FillProblemAccount(
            Sheet sheet,
            int rowIndex,
            ProblemAccountModel account)
        {
            var row = sheet.GetRow(rowIndex);
            row.CopyCellsFrom(GetTemplateRow(account.Status, account.ClosedReason, account.Locked));
            FillCells(row, account, RowMapper.Mappings);
        }

        private Row GetTemplateRow(byte accountStatus, int closedReason, byte locked)
        {
            var templateRow = OpenRowTemplate;

            switch (accountStatus)
            {
                case ProblemAccountStatusConstants.Disabled:
                    templateRow = DisabledRowTemplate;
                    break;

                case ProblemAccountStatusConstants.Closed:
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

                case ProblemAccountStatusConstants.Suspended:
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

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            var currentRowIndex = TemplateInfo.BodyStartIndex;

            OpenRowTemplate = sheet.GetRow(currentRowIndex++);
            SuspendedRowTemplate = sheet.GetRow(currentRowIndex++);
            LockedRowTemplateFalsePassword = sheet.GetRow(currentRowIndex++);
            CloseRowTemplateByAdmin = sheet.GetRow(currentRowIndex++);
            CloseRowTemplateByUpline = sheet.GetRow(currentRowIndex++);
            LockedRowTemplateFalseOTPSC = sheet.GetRow(currentRowIndex++);
            CloseRowTemplateByArchive = sheet.GetRow(currentRowIndex++);
            DisabledRowTemplate = sheet.GetRow(currentRowIndex);

            return GetSheetAt(ReportSheetIndex);
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
                FillProblemAccount(sheet, currentRowIndex++, item);
            }
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