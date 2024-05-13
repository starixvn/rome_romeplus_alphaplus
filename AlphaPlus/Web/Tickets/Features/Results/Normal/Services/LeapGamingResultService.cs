using Fanex.Result.Core.Base;
using Fanex.Result.Core.LeapGaming.DTO;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class LeapGamingResultService(IResultEngineAdapter resultEngineAdapter) : IProductResultService
{
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
        var resultSetting = new ResultSetting
        {
            SportType = SportType.LeapGamingVirtualSports,
            IsPopUp = true,
            LanguageCode = message.Language
        };

        var leapgamings = new List<LeapGaming> { new LeapGaming { TransDesc = message.TransDesc } };
        var resultDetail = new ResultDetailModel(resultEngineAdapter.HtmlParser(leapgamings, resultSetting));

        return Task.FromResult(resultDetail);
    }
}