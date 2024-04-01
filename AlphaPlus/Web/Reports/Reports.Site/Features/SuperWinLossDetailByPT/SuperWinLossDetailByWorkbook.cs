using AlphaPlus.Reports.Features.SuperWinLossDetailByPT;
using AlphaPlus.Reports.Site.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;

namespace AlphaPlus.Reports.Site.Features.SuperWinLossDetailByPT;

public interface ISuperWinLossDetailByWorkbook
{
    ExcelFileViewModel BuildExcelFile(SuperWinLossDetailByPTExcelQuery message);
}

public class SuperWinLossDetailByWorkbook(IWebHostEnvironment webHostEnvironment) :
    ReportNpoiWorkbook<SuperWinLossDetailByPTModel>(webHostEnvironment),
    ISuperWinLossDetailByWorkbook
{
    public ExcelFileViewModel BuildExcelFile(SuperWinLossDetailByPTExcelQuery message)
    {
        throw new NotImplementedException();
    }
}