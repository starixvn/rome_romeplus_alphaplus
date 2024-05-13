using Fanex.Result.Core.WM;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class WMResultService(IWMResultBuilder resultBuilder = null) : IProductResultService
{
    private readonly IWMResultBuilder resultBuilder = resultBuilder ?? new WMResultBuilder();

    public Task<string> GetResult(NormalResultQuery message)
    {
        throw new NotImplementedException();
    }

    public Task<MemoryStream> ExportExcel(ExcelNormalResultQuery message)
    {
        throw new NotImplementedException();
    }

    public Task<ResultDetailModel> GetResultDetails(NormalResultDetailsQuery message)
    {
        var resultDetail = new ResultDetailModel(resultBuilder.Render(
            message.TransDesc,
            message.BetCheck,
            message.BetTypeId,
            message.Language));

        return Task.FromResult(resultDetail);
    }
}