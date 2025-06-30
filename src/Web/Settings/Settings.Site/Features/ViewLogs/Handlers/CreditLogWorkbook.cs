using AlphaPlus.Settings.ViewLogs.API.Models;
using ONEbook.SpreadSheets;
using ONEbook.SpreadSheets.NPOI;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Handlers
{
    public interface CreditLogWorkbook
    {
        Workbook<Log> UseTemplate(TemplateInfo templateInfo);
        CreditLogWorkbook SetData(IEnumerable<Log> data);
        CreditLogWorkbook SetRowMappers(Mapper<Log> rowMapper);
        CreditLogWorkbook Sheet(string sheetName);
        CreditLogWorkbook Title(string titleText);
        CreditLogWorkbook Header(Func<string, string> headerMapping);
        CreditLogWorkbook Body(IEnumerable<Log> data, Mapper<Log> rowMapper);
        byte[] ExportToBytes();
    }

    public class CreditLogWorkbookImpl : NpoiWorkbook<Log>, CreditLogWorkbook
    {
        private const int ReportSheetIndex = 1;

        internal IEnumerable<Log> Data { get; set; }

        internal Mapper<Log> RowMapper { get; set; }

        internal Row DataRowTemplate { get; set; }

        public CreditLogWorkbook SetData(IEnumerable<Log> data)
        {
            Data = data;
            return this;
        }

        public CreditLogWorkbook SetRowMappers(Mapper<Log> rowMapper)
        {
            RowMapper = rowMapper;
            return this;
        }

        public new CreditLogWorkbook Sheet(string sheetName)
        {
            return this;
        }

        public new CreditLogWorkbook Title(string titleText)
        {
            return this;
        }

        public new CreditLogWorkbook Header(Func<string, string> headerMapping)
        {
            return this;
        }

        public new CreditLogWorkbook Body(IEnumerable<Log> data, Mapper<Log> rowMapper)
        {
            Data = data;
            RowMapper = rowMapper;
            return this;
        }

        public new byte[] ExportToBytes()
        {
            return base.ExportToBytes();
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);
            if (sheet == null)
            {
                throw new InvalidOperationException($"Sheet at index {TemplateInfo.SheetIndex} could not be loaded");
            }

            var currentRowIndex = TemplateInfo.BodyStartIndex;
            DataRowTemplate = sheet.GetRow(currentRowIndex);
            
            if (DataRowTemplate == null)
            {
                throw new InvalidOperationException($"Template row at index {currentRowIndex} could not be found in the sheet");
            }

            return sheet;
        }

        protected override void FillBodyRows(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            if (DataRowTemplate == null)
            {
                throw new InvalidOperationException("DataRowTemplate is not initialized. Make sure LoadTemplateSheet was called successfully.");
            }

            if (Data == null)
            {
                throw new InvalidOperationException("Data is not initialized. Make sure SetData was called successfully.");
            }

            if (RowMapper?.Mappings == null)
            {
                throw new InvalidOperationException("RowMapper is not initialized. Make sure SetRowMappers was called successfully.");
            }

            var currentRowIndex = TemplateInfo.BodyStartIndex;

            foreach (var log in Data)
            {
                if (log == null)
                {
                    currentRowIndex++;
                    continue; // Skip null logs
                }

                var row = sheet.GetRow(currentRowIndex);
                if (row == null)
                {
                    // If row doesn't exist, we need to handle this differently
                    // For now, let's skip this row and continue
                    currentRowIndex++;
                    continue;
                }

                // Copy template and fill cells
                row.CopyCellsFrom(DataRowTemplate);
                FillCells(row, log, RowMapper.Mappings);
                currentRowIndex++;
            }
        }

        protected override void FillNoInfoRow(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            if (DataRowTemplate == null)
            {
                throw new InvalidOperationException("DataRowTemplate is not initialized. Make sure LoadTemplateSheet was called successfully.");
            }

            if (string.IsNullOrWhiteSpace(TemplateInfo?.NoInfomationText))
            {
                throw new InvalidOperationException("No information text is not set in TemplateInfo.");
            }

            var rowIndex = TemplateInfo.BodyStartIndex;
            var colIndex = Column.GetColIndex(TemplateInfo.ColumnStartName);

            // Ensure we have a valid last cell index
            var lastCellIndex = DataRowTemplate.LastCellIndex;
            if (lastCellIndex <= 0)
            {
                lastCellIndex = (short)(colIndex + 4); // Fallback to a reasonable default
            }

            sheet.SetCellRangeValue(
                rowIndex,
                colIndex,
                rowIndex,
                lastCellIndex - 1,
                TemplateInfo.NoInfomationText);
        }

        protected override void FillBody(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            if (Data != null && Data.Any())
            {
                FillBodyRows(sheet);
            }
            else
            {
                FillNoInfoRow(sheet);
            }
        }
    }
} 