using Fanex.Result.Core.WCasino;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class WCasinoResultService(IWCasinoResultBuilder resultBuilder) : IProductResultService
{
    private readonly IWCasinoResultBuilder resultBuilder = resultBuilder;

    public Task<ResultDetailModel> GetResultDetails(NormalResultDetailsQuery message)
    {
        var resultDetail = new ResultDetailModel(resultBuilder.Render(
            message.TransDesc,
            message.BetCheck,
            message.BetTypeId,
            message.Language));

        return Task.FromResult(resultDetail);
    }

    public Task<string> GetResult(NormalResultQuery message) => throw new NotImplementedException();

    public Task<MemoryStream> ExportExcel(ExcelNormalResultQuery message) => throw new NotImplementedException();
}