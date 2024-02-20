using AlphaPlus.Results.Criteria.Products;
using AlphaPlus.Results.DataAccesses;
using Fanex.Result.Core.Base;

namespace AlphaPlus.Results.Services.Products;

public class ParlayResultService : IProductResultService
{
    private readonly IParlayResultDataAccess parlayResultDataAccess;
    private readonly IResultEngineAdapter resultEngineAdapter;

    public ParlayResultService(IParlayResultDataAccess parlayResultDataAccess, IResultEngineAdapter resultEngineAdapter)
        => (this.parlayResultDataAccess, this.resultEngineAdapter) = (parlayResultDataAccess, resultEngineAdapter);

    public async Task<string> GetResultDetails(NormalResultDetailsQuery detailsRequest)
    {
        var results = await parlayResultDataAccess.GetParlayResults(new GetParlayResultCriteria
        {
            RefNo = detailsRequest.RefNum
        });

        var resultSetting = new ResultSetting
        {
            IsPopupOpenFromBetList = detailsRequest.IsViewResultFromBetList,
            IsOutright = false,
            IsParlayPopUp = true,
            IsPopUp = true,
            VisiblePopUpTitle = false,
            LanguageCode = detailsRequest.Language,
            VisibleMatchCodeIDColumn = false,
            VisibleFreeHeader = true,
            VisibleDetailedLink = true,
            HighlightWinnerTeam = true
        };

        return resultEngineAdapter.HtmlParser(results, resultSetting);
    }

    public string GetResult(NormalResultQuery request)
    {
        throw new NotImplementedException();
    }

    public MemoryStream ExportExcel(ExcelNormalResultQuery request)
    {
        throw new NotImplementedException();
    }
}