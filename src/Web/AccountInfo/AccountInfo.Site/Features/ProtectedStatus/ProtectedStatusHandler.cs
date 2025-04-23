using AlphaPlus.AccountInfo.Site.Features.ProtectedStatus.Params;
using AlphaPlus.AccountInfo.Site.Features.ProtectedStatus.Requests;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.ProtectedStatus
{
    public class ProtectedStatusHandler(IProtectedStatusService protectedStatusService) :
        IRequestHandler<EditMultipleRequest, EditMultipleViewModel>,
        IRequestHandler<EditMultipleCommand, IEnumerable<Result>>
    {
        private readonly IProtectedStatusService protectedStatusService = protectedStatusService;

        public async Task<EditMultipleViewModel> Handle(EditMultipleRequest request, CancellationToken cancellationToken)
        {
            var protectedStatus = await protectedStatusService.GetProtectedStatus(
                new GetProtectedStatusParam(request.ViewerId, request.ViewerLevelId, request.CustIds));

            return new EditMultipleViewModel(protectedStatus);
        }

        public Task<IEnumerable<Result>> Handle(EditMultipleCommand request, CancellationToken cancellationToken)
            => protectedStatusService.UpdateMultiple(
                new UpdateMultipleProtectedStatusParam(
                    request.ProtectedStatus,
                    request.ViewerId,
                    request.ViewerName,
                    request.SubAccountId,
                    request.LangKey,
                    request.IP,
                    request.IsInternal,
                    request.ServerId));
    }
}