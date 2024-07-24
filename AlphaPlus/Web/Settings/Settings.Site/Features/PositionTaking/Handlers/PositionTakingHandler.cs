using AlphaPlus.Settings.Site.Features.PositionTaking.Requests;
using AlphaPlus.Settings.Site.Features.PositionTaking.Services;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.PositionTaking.Handlers;

public class PositionTakingHandler(IPositionTakingService positionTakingService)
    : IRequestHandler<PositionTakingRequest, object>
{
    public async Task<object> Handle(PositionTakingRequest request, CancellationToken cancellationToken)
    {
        return await positionTakingService.GetPT(request);
    }
}