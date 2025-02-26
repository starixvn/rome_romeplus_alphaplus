
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using Fanex.Result.Core.AllBetCasino;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class AllBetCasinoResultService(IOptions<AppSettings> options) : IProductResultService
{
    private readonly IAllbetResultBuilder resultBuilder = new AllbetResultBuilder(new AllbetConfigs() { ImagePath = options.Value.AllbetResultImagePath });

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