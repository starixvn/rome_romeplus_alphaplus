using AlphaPlus.Settings.Site.Features.PositionTaking.Requests;
using AlphaPlus.Settings.Site.Features.PositionTaking.Services;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.PositionTaking.Handlers;

public class PositionTakingHandler(IPositionTakingService positionTakingService)
    : IRequestHandler<PositionTakingRequest, object>, IRequestHandler<DownlinePositionTakingRequest, object>
{
    public async Task<object> Handle(PositionTakingRequest request, CancellationToken cancellationToken)
    {
        return await positionTakingService.GetPT(request);
    }

    public async Task<object> Handle(DownlinePositionTakingRequest request, CancellationToken cancellationToken)
    {
        return await positionTakingService.GetPT(request);
    }
}