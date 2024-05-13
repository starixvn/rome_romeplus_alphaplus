using Fanex.Result.Core.SAGaming;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class SAGamingResultService(ISAGamingResultBuilder resultBuilder = null) : IProductResultService
{
    private readonly ISAGamingResultBuilder resultBuilder = resultBuilder ?? new SAGamingResultBuilder();

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
            message.BetTypeId.ToString(),
            message.Language));

        return Task.FromResult(resultDetail);
    }
}