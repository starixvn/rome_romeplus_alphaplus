using AlphaPlus.Tickets.Site.Features.Shared;
using Fanex.Result.Core.SabaClub;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class SabaClubHandler(Fanex.Logging.ILogger logger) : IRequestHandler<SabaClubResultQuery, ThirdPartyProductViewModel>
{
    public async Task<ThirdPartyProductViewModel> Handle(SabaClubResultQuery message, CancellationToken cancellationToken)
    {
        string result;
        bool isSuccess;

        try
        {
            var service = new SabaClubResult();
            result = service.GetResult(message.CustId, message.GameRoundId, message.GameId, message.SiteId, message.SportId, message.Language);
            isSuccess = true;
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
            result = ex.Message;
            isSuccess = false;
        }

        return new ThirdPartyProductViewModel(
            $"{CacheRefServiceHelper.GetSportName(message.SportId)} - {CacheRefServiceHelper.GetBetTypeName(Convert.ToInt32(message.GameId))}",
            result,
            isSuccess);
    }
}