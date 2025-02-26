namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public interface IProductResultService
{
    Task<string> GetResult(NormalResultQuery message);

    Task<MemoryStream> ExportExcel(ExcelNormalResultQuery message);

    Task<ResultDetailModel> GetResultDetails(NormalResultDetailsQuery message);
}