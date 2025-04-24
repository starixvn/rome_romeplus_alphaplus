using AlphaPlus.PartialUM.Services;
using Fanex.Data.Repository;
using OutrightDTO = Fanex.Result.Core.Sportsbook.DTO.Outright;

namespace AlphaPlus.Results.Features.Outright;

public interface IOutrightService
{
    Task<IEnumerable<OutrightDTO>> GetOutrights(OutrightParam param);

    IEnumerable<OutrightDTO> FilterOutrights(IEnumerable<OutrightDTO> outrights, int sportType, int leagueId);
}

public class OutrightService(
    IDynamicRepository repository,
    IPartialUMService partialUMService) : IOutrightService
{
    public async Task<IEnumerable<OutrightDTO>> GetOutrights(OutrightParam param)
    {
        var criteria = new GetOutRightResultCriteria(
            param.FromDate.Date,
            new DateTime(param.ToDate.Year, param.ToDate.Month, param.ToDate.Day, 23, 59, 59),
            param.IsShowAll);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var rawdata = await repository.FetchAsync<OutrightDTO>(criteria);

        return rawdata.OrderBy(outright => outright.SportType);
    }

    public IEnumerable<OutrightDTO> FilterOutrights(IEnumerable<OutrightDTO> outrights, int sportType, int leagueId)
    {
        if (sportType != 0)
        {
            outrights = outrights.Where(outright => outright.SportType == sportType);
        }

        if (leagueId != 0)
        {
            outrights = outrights.Where(outright => outright.LeagueId == leagueId);
        }

        return outrights;
    }
}