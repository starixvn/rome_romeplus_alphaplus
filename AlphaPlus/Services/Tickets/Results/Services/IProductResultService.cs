namespace AlphaPlus.Results.Services;

public interface IProductResultService
{
    Task<string> GetResult(NormalResultQuery request);

    MemoryStream ExportExcel(ExcelNormalResultQuery request);

    string GetResultDetails(NormalResultDetailsQuery detailsRequest);
}