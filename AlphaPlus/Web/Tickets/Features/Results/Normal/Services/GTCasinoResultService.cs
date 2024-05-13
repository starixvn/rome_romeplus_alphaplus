using Fanex.Result.Core.GTCasino;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class GTCasinoResultService(IGTCasinoResultBuilder resultBuilder = null) : IProductResultService
{
    private readonly IGTCasinoResultBuilder resultBuilder = resultBuilder ?? new GTCasinoResultBuilder();

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