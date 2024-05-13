using Fanex.Result.Core.SexyGaming;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class SexyGamingResultService(ISexyGamingResultBuilder resultBuilder = null) : IProductResultService
{
    private readonly ISexyGamingResultBuilder resultBuilder = resultBuilder ?? new SexyGamingResultBuilder();

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