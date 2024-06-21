using AlphaPlus.Tickets.Site.Features.Shared;
using Fanex.Result.Core.SabaClub;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class SabaClubHandler(
    Fanex.Logging.ILogger logger,
    ICacheRefService cacheRefService,
    ISabaClubResult sabaClubResult) : IRequestHandler<SabaClubResultQuery, ThirdPartyProductViewModel>
{
    public async Task<ThirdPartyProductViewModel> Handle(SabaClubResultQuery message, CancellationToken cancellationToken)
    {
        string result;
        bool isSuccess;

        try
        {
            result = sabaClubResult.GetResult(message.CustId, message.GameRoundId, message.GameId, message.SiteId, message.SportId, message.Language);
            isSuccess = true;
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
            result = ex.Message;
            isSuccess = false;
        }

        return new ThirdPartyProductViewModel(
            $"{cacheRefService.GetSportName(message.SportId)} - {cacheRefService.GetBetTypeName(Convert.ToInt32(message.GameId))}",
            result,
            isSuccess);
    }
}