namespace Alpha.MemberInfo.Site.Features.DormantAccountList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Models;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;

    public interface DormantAccountWorkbook
    {
        DormantAccountWorkbook SetData(IEnumerable<DormantAccount> dormantAccounts, DormantAccount total);

        DormantAccountWorkbook SetRowMappers(Mapper<DormantAccount> dormantAccountMapper, Mapper<DormantAccount> totalMapper);

        Workbook<DormantAccount> UseTemplate(TemplateInfo templateInfo);
    }

    public class DormantAccountWorkbookImpl : NpoiWorkbook<DormantAccount>, DormantAccountWorkbook
    {
        private const int DataSheetIndex = 1;
        private int currentRowIndex;

        internal IEnumerable<DormantAccount> DormantAccounts { get; set; }
        internal DormantAccount Total { get; set; }

        internal Mapper<DormantAccount> DormantAccountMapper { get; set; }
        internal Mapper<DormantAccount> TotalMapper { get; set; }

        private Row NormalRowTemplate { get; set; }
        private Row WarningTicketRowTemplate { get; set; }
        private Row WarningTicketRedDateTimeRowTemplate { get; set; }
        private Row RedDateTimeRowTemplate { get; set; }
        internal Row TotalRowTemplate { get; set; }

        public DormantAccountWorkbook SetData(IEnumerable<DormantAccount> dormantAccounts, DormantAccount total)
        {
            DormantAccounts = dormantAccounts;
            Total = total;

            return this;
        }

        public DormantAccountWorkbook SetRowMappers(Mapper<DormantAccount> dormantAccountMapper, Mapper<DormantAccount> totalMapper)
        {
            DormantAccountMapper = dormantAccountMapper;
            TotalMapper = totalMapper;

            return this;
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);
            currentRowIndex = TemplateInfo.BodyStartIndex;

            TotalRowTemplate = sheet.GetRow(currentRowIndex++);
            NormalRowTemplate = sheet.GetRow(currentRowIndex++);
            WarningTicketRowTemplate = sheet.GetRow(currentRowIndex++);
            WarningTicketRedDateTimeRowTemplate = sheet.GetRow(currentRowIndex++);
            RedDateTimeRowTemplate = sheet.GetRow(currentRowIndex++);


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

            if (DormantAccounts.Any())
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

            if (Total != null)
            {
                var row = sheet.GetRow(currentRowIndex++);
                row.CopyCellsFrom(GetRowTemplate(Total));
                FillCells(row, Total, DormantAccountMapper.Mappings);
            }

            foreach (var item in DormantAccounts)
            {
                var row = sheet.GetRow(currentRowIndex++);
                row.CopyCellsFrom(GetRowTemplate(item));

                FillCells(row, item, DormantAccountMapper.Mappings);
            }
        }

        private Row GetRowTemplate(DormantAccount account)
        {
            Row templateRow;

            if (account.IsWarningTicket)
            {
                templateRow = WarningTicketRowTemplate;

                if (account.IsRedDateTime)
                {
                    templateRow = WarningTicketRedDateTimeRowTemplate;
                }
            }
            else if (account.IsRedDateTime)
            {
                templateRow = RedDateTimeRowTemplate;
            }
            else if (account.IsSummaryRow)
            {
                templateRow = TotalRowTemplate;
            }
            else
            {
                templateRow = NormalRowTemplate;
            }

            return templateRow;
        }

        protected override void FillSheetName(int index, string name)
            => Workbook.SetSheetName(DataSheetIndex, name);
    }
}