using Fanex.Result.Core.AllBetCasino;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class AllBetCasinoResultService(IAllbetResultBuilder resultBuilder = null) : IProductResultService
{
    private readonly IAllbetResultBuilder resultBuilder = resultBuilder ?? new AllbetResultBuilder();

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